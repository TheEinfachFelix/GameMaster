using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Ports;
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

        bool RecieveLock = false;
        string RecieveData = "";

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

                Trace.WriteLine("R Item:"+item);
                HandleData(DataToJson(ParseData(item)));
            }
            string regexes = "\\{.[^\\}]*$";
            var matches2 = Regex.Matches(pmsg, regexes)
                .Cast<Match>()
                .Select(m => m.Value)
                .ToArray();
            if (matches2.Count() == 0) { msg = ""; return; }
            msg = matches2[matches2.Count()-1] ?? "";
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
                return;
            }

            if (type == "Event") {HandleEvent(pJson); return;}

            if (type == "Response") {HandleResponse(pJson); return;}

            throw new Exception("der Request existiert nicht");
        }
        private void HandleResponse(BuzzerJson pJson)
        {
            if(!pJson.Success ?? throw new Exception(missingKeyError + "Success"))
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
                    controller.BuzzerList[index].setBuzzerState(OldValue, NewValue);
                    break;
                case "Taster":
                    controller.TasterList[index].setTasterState(OldValue, NewValue);
                    break;
                default:
                    throw new Exception("the Type is not valide >" + pJson.IOType);
            }
        }
        public string GetData(string Json)
        {
            // await ready to go
            while(RecieveLock)
            {}
            // block to recieving
            RecieveLock = true;
            // reset the Data
            RecieveData = "";
            // send the data
            if (port == null) throw new Exception("Serial is not Open");
            Trace.WriteLine("Sending:"+Json);
            port.Write(Json);
            // await response or timeout
            int i = 0;
            while(RecieveData == "")  //TODO the code hanges here because i think because of threading
            {
                if (i > 50) throw new Exception("the serial request timeouted");
                i += 1;
                Thread.Sleep(10);
            }
            RecieveLock = false;
            return RecieveData;
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
