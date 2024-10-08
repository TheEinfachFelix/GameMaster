﻿using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Ports;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;


namespace GameMaster.Input
{
    public class BuzzerControllerInterface
    {
        static readonly string missingKeyError = "the recieved Json is missing the key ";
        private BuzzerController controller;
        private SerialPort? port;
        private string msg = "";
        readonly int ComPort;
        readonly int Baudrate;

        private  SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        string RecieveData = "";
        bool EventBlock = false;

        public BuzzerControllerInterface(int ComPort, int Baudrate, BuzzerController ccontroller)
        {   
            controller = ccontroller;
            this.ComPort = ComPort;
            this.Baudrate = Baudrate;
        }
        public void Setup()
        {
            if (port != null) { throw new Exception("Buzzer connection alredy open"); }

            port = new SerialPort("COM" + ComPort.ToString(), Baudrate, Parity.None, 8, StopBits.One);
            port.Handshake = Handshake.RequestToSend; // DAS IST EXTREM WICHTIG

            // Setup Eventhandler
            port.DataReceived += new SerialDataReceivedEventHandler(DataReceiver);

            // Begin communications
            port.Open();
            port.DiscardInBuffer();

            //Task.Run(() => DataReciverAsync());
        }
        public void Stop()
        {
            if (port == null) { throw new Exception("Buzzer connection alredy Closed"); }

            port.Close();
            port = null;
        }
        private void DataReceiver(object sender, SerialDataReceivedEventArgs e)
        {
            if (port == null) { return; }
            if (EventBlock){ return; }

            msg += port.ReadExisting();
            CheckDataTransmissionDone(msg);
        }
        private void CheckDataTransmissionDone(string pmsg)
        {
            // Cleans the input msg
            pmsg = new string(pmsg.Where(c => !char.IsControl(c)).ToArray());

            // splits the msg
            string regex = "\\{.*?\\}";
            var matches = Regex.Matches(pmsg, regex).Cast<Match>().Select(m => m.Value).ToArray();

            // sends of every json induvidual
            foreach (string item in matches)
            {

                    Trace.WriteLine("Rrecived Item:"+item);
                    HandleData(DataToJson(ParseData(item)));

            }


            // leaves the last unfinished json msg in the msg
            string regexes = "\\{.[^\\}]*$";
            var matches2 = Regex.Matches(pmsg, regexes).Cast<Match>().Select(m => m.Value).ToArray();
            if (matches2.Count() == 0) 
            { 
                msg = ""; 
                return; 
            }
            msg = matches2[matches2.Count()-1] ?? "";
        }
        private string ParseData(string input)
        {
            // adds a } if needed
            int countOpen = input.Split('{').Length - 1;
            int countClose = input.Split('}').Length - 1;
            if (countClose < countOpen)// catch more close than open
            {
                input += "}";
            }
            
            return input;
        }
        private BuzzerJson DataToJson(string input)
        {
            BuzzerJson JsonMsg;
            JsonMsg = JsonConvert.DeserializeObject<BuzzerJson>(input)!;

            if (JsonMsg == null) { throw new Exception("Buzzer Controller Hardware recieved Json not readebel"); }

            return JsonMsg;
        }
        private void HandleData(BuzzerJson pJson)
        {
            string type = pJson.Type ?? throw new Exception("Recieved Json is not vaillide");

            if (type == "Request") {throw new Exception("The recieved Json is a Request. We dont do that!");}
            if (type == "Debug")
            {
                string outp = "Error MSG is: " + pJson.MSG + " and the Value is" + pJson.Value;
                if (pJson.Critical == true) {throw new Exception(outp);}
                Trace.TraceError(outp);
                return;
            }
            if (type == "Event") 
            {
                if (EventBlock) { return; }
                HandleEvent(pJson); 
                return;
            }

            if (type == "Response") {HandleResponse(pJson); return;}

            throw new Exception("der Request existiert nicht");
        }
        private void HandleResponse(BuzzerJson pJson)
        {
            if (!pJson.Success ?? throw new Exception(missingKeyError + "Success"))
            {
                string outp = "Execution was not successfull. the error is: " + pJson.Error;
                if (pJson.Critical == true) {throw new Exception(outp);}
                Trace.TraceError(outp);
            }
            RecieveData = pJson.Value ?? throw new Exception(missingKeyError + "Value");
            if (RecieveData == "") throw new Exception("Value is emty");
        }
        private void HandleEvent(BuzzerJson pJson)
        {
            int index = pJson.ID ?? throw new Exception(missingKeyError + "ID");
            bool OldValue = pJson.OldValue ??throw new Exception(missingKeyError + "OldValue");
            bool NewValue = pJson.NewValue ??throw new Exception(missingKeyError + "NewValue");
            switch (pJson.IOType ?? throw new Exception(missingKeyError + "IOType"))
            {
                case "Buzzer":
                    controller.BuzzerList[index].HandleEvent(OldValue, NewValue);
                    break;
                case "Taster":
                    controller.TasterList[index].HandleEvent(OldValue, NewValue);
                    break;
                default:
                    throw new Exception("the Type is not valide >" + pJson.IOType);
            }
        }
        public async Task<string> GetData(string Json)
        {
            await semaphore.WaitAsync();
            try
            {
                int timeout = 1000000;
                var startTime = DateTime.Now;
                RecieveData = "";
                msg = "";
                
                // send the data
                if (port == null) throw new Exception("Serial is not Open");
                Trace.WriteLine("Sending:"+Json);
                port.BaseStream.Flush();
                EventBlock = true;
                port.Write(Json);
                
                // await response or timeout
                while (string.IsNullOrEmpty(RecieveData)) 
                {
                    if ((DateTime.Now-startTime).TotalMilliseconds > timeout)
                    {
                        throw new TimeoutException("the GetData() response took to long");
                    }
                    //await Task.Delay(50);

                    msg += port.ReadExisting();
                    CheckDataTransmissionDone(msg);
                }

                return RecieveData;
            }
            finally
            {
                Trace.WriteLine("Release");
                EventBlock = false;
                semaphore.Release();
            }
        }
    }
    public class BuzzerJson
    {
        // for response
        public string? Type { get; set; }
        public bool? Success { get; set; }
        public string? Value { get; set; }
        // for error
        public string? Error { get; set; }
        public bool? Critical { get; set; }
        // for debug
        public string? MSG { get; set; }

        // for event
        public string? IOType { get; set; }
        public int? ID {get; set;}
        public bool? NewValue { get; set; }
        public bool? OldValue { get; set; }

    }
}
