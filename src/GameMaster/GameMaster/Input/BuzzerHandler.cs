using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster.Input
{
    public class BuzzerHandler
    {
        [JsonIgnore]
        public List<Buzzer> BuzzerList = [];
        public int AmountOfBuzzer { get; private set; }

        [JsonIgnore]
        public List<Taster> TasterList = [];
        public int AmountOfTaster { get; private set; }

        [JsonIgnore]
        public List<LED> LEDListe = [];
        public int AmountOfLED { get; private set; }


        private SerialPort? port;
        private string msg = "";

        public BuzzerHandler(int pamountOfBuzzer, int pamountOfTaster, int pamountOfLED) 
        {
            AmountOfBuzzer =  pamountOfBuzzer;
            AmountOfTaster = pamountOfTaster;
            AmountOfLED = pamountOfLED;
            Setup();
        }
        private void Setup()
        {
            for (int i = 0; i < AmountOfBuzzer; i++)
            {
                BuzzerList.Add(new(i,this));
            }

            for (int i = 0; i < AmountOfTaster; i++)
            {
                TasterList.Add(new(i));
            }

            for (int i = 0; i < AmountOfLED; i++)
            {
                LEDListe.Add(new(i,this));
            }
        }

        public void Start(int ComPort, int Rate)
        {
            if (port != null) { throw new Exception("Buzzer connection alredy open"); }

            port = new SerialPort("COM" + ComPort.ToString(), Rate, Parity.None, 8, StopBits.One);
            port.Handshake = Handshake.RequestToSend; // DAS IST EXTREM WICHTIG

            // Setup Eventhandler
            port.DataReceived += new SerialDataReceivedEventHandler(DataReceived);

            // Begin communications
            port.Open();
            port.DiscardInBuffer();
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
            if (port == null) { return; }
            msg += port.ReadExisting();

            //Console.WriteLine(msg);

            CheckDataTransmissionDone(msg);
        }
        private void CheckDataTransmissionDone(string pmsg)
        {
            pmsg = new string(pmsg.Where(c => !char.IsControl(c)).ToArray());
            int countOpen = pmsg.Split('{').Length - 1;
            int countClose = pmsg.Split('}').Length - 1;

            if (countClose > countOpen)// catch more close than open
            {
                msg = "";
                throw new Exception("there where to many } send");
            }
            if (countClose == countOpen && countOpen != 0 && pmsg != "") // the msg is copletly recived
            {
                msg = "";
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

            int index = (int)JsonMsg.ID - 1;

            switch (JsonMsg.Output_Type)
            {
                case "Buzzer":
                    BuzzerList[index].TasterState = (bool)JsonMsg.Value;
                    break;
                case "Taster":
                    TasterList[index].TasterState = (bool)JsonMsg.Value;
                    break;
                default:
                    throw new Exception("Buzzer PCB Output_Type not found");
            }
        }

        public void sendData(string json)
        {
            if (port == null) return;
            port.Write(json);
        }
    }

    public class Buzzer
    {
        private Game game = Game.GetInstance();
        private BuzzerHandler parent;

        public int myID { get; set; } = -1;

        private bool _TasterState = false;
        public bool TasterState 
        { 
            get 
            { 
                return _TasterState; 
            } set 
            {
                bool newVal = value;

                if (newVal != _TasterState) 
                {
                    if (!newVal)
                    {
                        game.BuzzerPress(myID);
                    }
                    else
                    {
                        game.BuzzerRelease(myID);
                    }
                }
                _TasterState = newVal;
            } 
        }

        private bool _LEDState = false;
        public bool LEDState
        {
            get
            {
                return _LEDState;
            }
            set
            {
                _LEDState = value;
                parent.sendData("{"+ $"\"Output_Type\" : \"Buzzer\", \"ID\" : {myID+1}, \"Value\" : {_LEDState.ToString().ToLower()}" + "}");
            }
        }

        public Buzzer(int pID, BuzzerHandler pparent)
        {
            myID = pID;
            parent = pparent;
        }
    }
    public class Taster
    {
        private Game game = Game.GetInstance();
        

        public int myID { get; private set; } = -1;

        private bool _TasterState = false;
        public bool TasterState
        {
            get
            {
                return _TasterState;
            }
            set
            {
                bool newVal = value;

                if (newVal != _TasterState)
                {
                    if (newVal)
                    {
                        //game.BuzzerPress(myID);
                    }
                    else
                    {
                        //game.BuzzerRelease(myID); //TODO: handle this
                    }
                }
                _TasterState = newVal;
            }
        }


        public Taster(int pID)
        {
            myID = pID;
        }
    }
    public class LED
    {
        private BuzzerHandler parent;
        public int myID { get; private set; } = -1;

        private int _R;
        public int R
        {
            get
            {
                return _R;
            }
            set
            {
                _R = value;
                Send();
            }
        }
        private int _G;
        public int G
        {
            get
            {
                return _G;
            }
            set
            {
                _G = value;
                Send();
            }
        }
        private int _B;
        public int B
        {
            get
            {
                return _B;
            }
            set
            {
                _B = value;
                Send();
            }
        }

        public LED(int pID, BuzzerHandler pparent)
        {
            myID = pID;
            parent = pparent;
        }
        private void Send() 
        {
            parent.sendData("{" + $"\"Output_Type\" : \"LED\", \"ID\" : {myID}, \"Value\" : " +"{"+$"\"R\":{R}, \"G\":{G}, \"B\":{B}" + "}}");
        }
        public void SetLEDColor(int pR, int pG, int pB)
        {
            _R = pR;
            _G = pG;
            _B = pB;
            Send();
        }
    }

    public class BuzzerJson
    {
        public string? Output_Type { get; set; }
        public string? Error { get; set; }
        public int? ID { get; set; }
        public bool? Value { get; set; }
    }
}
