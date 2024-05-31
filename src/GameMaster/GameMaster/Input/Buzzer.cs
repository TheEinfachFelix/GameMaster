using System.IO.Ports;

using Newtonsoft.Json;


namespace GameMaster.Input
{
    public class TestBuzzer
    {
        private SerialPort? port;
        private String msg = "";
        private Game game = Game.GetInstance();

        public void Start(int ComPort, int Rate)
        {
            if (port != null) { throw new Exception("Buzzer connection alredy open"); }

            port = new SerialPort("COM" + ComPort.ToString(), Rate, Parity.None, 8, StopBits.One);

            // Setup Eventhandler
            port.DataReceived += new SerialDataReceivedEventHandler(DataReceived);

            // Begin communications
            port.Open();
            Console.WriteLine("pe");
            port.Write("{\"Output_Type\" : \"LED\", \"ID\" : 2, \"Value\" : {\"R\":50, \"G\":10, \"B\":0}}");
        }
        public void Stop()
        {
            if (port == null) { throw new Exception("Buzzer connection alredy Closed"); }

            port.Close();
            port = null;

            Console.WriteLine("stop");
        }
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("peinsi");
            if (port == null) { return; }
            msg += port.ReadExisting();

            Console.WriteLine($"{msg}");

            CheckDataTransmissionDone(msg);
        }
        private void CheckDataTransmissionDone(string pmsg)
        {
            int countOpen = pmsg.Split('{').Length - 1;
            int countClose = pmsg.Split('}').Length - 1;

            if (countClose > countOpen)// catch more close than open
            {
                msg = "";
                throw new Exception("there where to many } send");
            }
            if (countClose == countOpen && countOpen != 0) // the msg is copletly recived
            {
                HandleData(pmsg);
            }
        }

        private void HandleData(string pmsg)
        {
            BuzzerJson JsonMsg = JsonConvert.DeserializeObject<BuzzerJson>(pmsg)!;

            // do Error catching
            if (JsonMsg == null) { throw new Exception("Buzzer PCB recieved Json not readebel"); }
            if (JsonMsg.Error != null) { throw new Exception("Buzzer PCB Error was" + JsonMsg.Error); }
            if (JsonMsg.Value == null || JsonMsg.ID == null || JsonMsg.Output_Type == null) { throw new Exception("Json missing Data"); }

            int index = (int)JsonMsg.ID;

            switch (JsonMsg.Output_Type)
            {
                case "Buzzer":
                    if ((bool)JsonMsg.Value)
                    {
                        game.BuzzerPress(index);
                    }
                    else
                    {
                        game.BuzzerRelease(index);
                    }
                    break;
                case "Taster":
                    // TODO: Implement666666
                    break;
                default:
                    throw new Exception("Buzzer PCB Output_Type not found");
            }
            msg = "";
        }
    }

    public class BuzzerJson
    {
        // {"status":"server ready", "appType":"dot2"}
        public string? Output_Type { get; set; }
        public string? Error { get; set; }
        public int? ID { get; set; }
        public bool? Value { get; set; }

    }
}
