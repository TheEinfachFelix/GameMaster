using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Ports;
using System.Text.RegularExpressions;


namespace GameMaster.Input
{
    public class BuzzerControllerInterface
    {
        static string missingKeyError = "the recieved Json is missing the key ";
        private BuzzerController controller;
        private SerialPort? port;
        private string msg = "";
        int ComPort;
        int Baudrate;

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

            msg += port.ReadExisting();

            Trace.WriteLine("Recieved: "+msg);

            CheckDataTransmissionDone(msg);
        }
        private void CheckDataTransmissionDone(string pmsg)
        {
            pmsg = new string(pmsg.Where(c => !char.IsControl(c)).ToArray());

            string regex = "\\{.*?\\}";
            var matches = Regex.Matches(pmsg, regex)
                .Cast<Match>()
                .Select(m => m.Value)
                .ToArray();

            foreach (string item in matches)
            {
                HandleData(DataToJson(ParseData(item)));
            }
        }
        private string ParseData(string input)
        {
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
            }

            if (type == "Event") {HandleEvent(pJson);}

            if (type == "Response") {HandleResponse(pJson);}

            // do Error catching




        }
        
        private void HandleResponse(BuzzerJson pJson)
        {
            if(!pJson.Success ?? throw new Exception(missingKeyError + "Success"))
            {
                string outp = "Execution was not successfull. the error is: " + pJson.Error;
                if (pJson.Critical == true) {throw new Exception(outp);}
                Trace.TraceError(outp);
            }

            // give the data to to the function that requested it
        }

        private void HandleEvent(BuzzerJson pJson)
        {
            int index = pJson.ID ?? throw new Exception(missingKeyError + "ID");
            bool OldValue = pJson.OldValue ??throw new Exception(missingKeyError + "OldValue");
            bool NewValue = pJson.NewValue ??throw new Exception(missingKeyError + "NewValue");

            switch (pJson.IOType ?? throw new Exception(missingKeyError + "IOType"))
            {
                case "Buzzer":
                    controller.BuzzerList[index].setBuzzerState(OldValue, NewValue);
                    break;
                case "Taster":
                    controller.TasterList[index].setTasterState(OldValue, NewValue);
                    break;
                default:
                    throw new Exception("the Type is not valide >" + pJson.IOType);
            }

        }


        public void SendData(string json)
        {
            if (port == null) return;
            port.Write(json);
            //Trace.WriteLine("sending: "+json);
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
