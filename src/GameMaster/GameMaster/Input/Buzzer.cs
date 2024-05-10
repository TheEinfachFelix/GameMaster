using System.IO.Ports;

namespace GameMaster.Input
{
    public class TestBuzzer
    {
        private SerialPort? port;
        private String msg = "";
        private Game game = Game.GetInstance();

        public void Start(int ComPort, int Rate)
        {
            if (port != null) { throw new Exception("Connection alredy open"); }

            port = new SerialPort("COM" + ComPort.ToString(), Rate, Parity.None, 8, StopBits.One);

            port.DataReceived += new SerialDataReceivedEventHandler(DataReceived);

            // Begin communications
            port.Open();

        }
        public void Stop()
        {
            if (port == null) { throw new Exception("Connection alredy Closed"); }

            port.Close();
            port = null;

            Console.WriteLine("stop");
        }
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (port == null) { return; }
            msg += port.ReadExisting();

            CheckDataTransmissionDone(msg);
        }
        private void CheckDataTransmissionDone(string pmsg)
        {
            if (!pmsg.Contains("-")) { return; }
            if (pmsg.Count() < 2) { return; }
            if (pmsg.First() == '-') 
            {  
                msg = "";
                return;
            }

            CleanData(pmsg);
        }
        private void CleanData(string pmsg)
        {
            pmsg = pmsg.Replace(" ", "");
            pmsg = pmsg.Replace("\n", "");
            pmsg.ToLower();

            HandleData(pmsg);
        }
        private void HandleData(string pmsg)
        {
            // get Buzzer Index
            int index = pmsg.ElementAt(0) - '0';
            // get Buzzer Event
            string? myevent = pmsg.ElementAt(1).ToString();
            
            switch (myevent)
            {
                case "r":
                    game.BuzzerRelease(index);
                    break;
                case "p":
                    game.BuzzerPress(index);
                    break;
            }
            msg = "";
        }
    }
}
