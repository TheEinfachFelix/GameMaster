using NAudio.CoreAudioApi;
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
    public class BuzzerControllerMngr
    {
        [JsonIgnore]
        Game game;

        public bool isDisabeled { get; set; }
        public List<int> NormalCollor { get; set; }
        public List<int> PressCollor { get; set; }
        public List<int> BlockCollor { get; set; }

        public List<BuzzerController> BuzzerControllerList { get; set; } = [];
        

        public void Setup()
        {
            BuzzerControllerList.Add(new(8, 3, 12));

            game = Game.GetInstance();
            if (isDisabeled) return;

            int i = 0;
            foreach (BuzzerController item in BuzzerControllerList)
            {
                item.Setup(i);
                i++;
            }
        }

        public void TasterEvent(int parentID, int BuzzerID, bool Value)
        {
            if (isDisabeled) return;
            if (game.CLevel == null) { throw new Exception("CLevel is null"); }

            game.CLevel.TasterEvent(BuzzerID, Value);
        }
        public void BuzzerPress(int parentID, int BuzzerID)
        {
            if (isDisabeled) return;
            if (game.CLevel == null) { throw new Exception("CLevel is null"); }

            game.CLevel.BuzzerPress(BuzzerID);
        }
        public void BuzzerRelease(int parentID, int BuzzerID)
        {
            if (isDisabeled) return;
            if (game.CLevel == null) { throw new Exception("CLevel is null"); }

            game.CLevel.BuzzerRelease(BuzzerID);
        }
    }

    public class BuzzerController
    {
        private Game game;
        [JsonIgnore]
        public List<Buzzer> BuzzerList = [];
        public int AmountOfBuzzer { get; set; }

        [JsonIgnore]
        public List<Taster> TasterList = [];
        public int AmountOfTaster { get; set; }

        [JsonIgnore]
        public List<LED> LEDListe = [];
        public int AmountOfLED { get; set; }


        public int ComPort { get; set; }
        public int Baudrate { get; set; }
        public bool Dummy { get; set; }
        public List<int> dot2Valus { get; set; } = [0,5];


        private bool _BuzzerSisabeled = false;
        [JsonIgnore]
        public bool BuzzerDisabeled 
        {  get
            {
                return _BuzzerSisabeled;
            }
            set
            {
                _BuzzerSisabeled= value;

                if (_BuzzerSisabeled)
                {
                    // disabeled collor
                    var Collor = Game.GetInstance().BuzzerControll.BlockCollor;
                    foreach (Buzzer item in BuzzerList)
                    {
                        LEDListe[item.myID].SetLEDColor(Collor[0], Collor[1], Collor[2]);
                    }
                } else
                {
                    // restore collor
                    foreach (Buzzer item in BuzzerList)
                    {
                        // get previouse collor
                        var Collor = Game.GetInstance().BuzzerControll.NormalCollor;
                        if (item.TasterState)
                        {
                            Collor = Game.GetInstance().BuzzerControll.PressCollor;
                        }
                        LEDListe[item.myID].SetLEDColor(Collor[0], Collor[1], Collor[2]);
                    }
                }

            }
        }


        [JsonIgnore]
        public BuzzerControllerInterface BuzzerControlerInterface;
        [JsonIgnore]
        public int ID;

        public BuzzerController(int pamountOfBuzzer, int pamountOfTaster, int pamountOfLED) 
        {
            AmountOfBuzzer =  pamountOfBuzzer;
            AmountOfTaster = pamountOfTaster;
            AmountOfLED = pamountOfLED;

        }
        public void Setup(int id)
        {
            game = Game.GetInstance();
            ID = id;
            for (int i = 0; i < AmountOfBuzzer; i++)
            {
                BuzzerList.Add(new(i,this));
            }

            for (int i = 0; i < AmountOfTaster; i++)
            {
                TasterList.Add(new(i, this));
            }

            for (int i = 0; i < AmountOfLED; i++)
            {
                LEDListe.Add(new(i,this));
            }

            if (Dummy) return;
            BuzzerControlerInterface = new(ComPort, Baudrate, this);
        }

        public void TasterEvent(int TasterID, bool Value)
        {
            // taster 0 ist disable
            if (TasterID == 0)
            {
                BuzzerDisabeled = Value;
                return;
            }
            
            // event weitergeben
            game.BuzzerControll.TasterEvent(ID,TasterID, Value);
        }
        public void BuzzerPress(int BuzzerID)
        {
            if (BuzzerDisabeled) return;

            // setzt die passende Farbe
            var Collor = game.BuzzerControll.PressCollor;
            LEDListe[BuzzerID].SetLEDColor(Collor[0], Collor[1], Collor[2]);

            // make dot2 licht stuff
            game.dot2ConnectorList[0].SendButtonPress(dot2Valus[BuzzerID]);

            // event weitergeben
            game.BuzzerControll.BuzzerPress(ID,BuzzerID);
        }
        public void BuzzerRelease(int BuzzerID)
        {
            if (BuzzerDisabeled) return;

            // setzt die passende Farbe
            var Collor = game.BuzzerControll.NormalCollor;
            LEDListe[BuzzerID].SetLEDColor(Collor[0], Collor[1], Collor[2]);

            // event weitergeben
            game.BuzzerControll.BuzzerRelease(ID,BuzzerID);
        }

        public void SendData(string json)
        {
            if(Dummy) return;
            if (BuzzerControlerInterface == null) throw new Exception("not properly setup");
            BuzzerControlerInterface.SendData(json);
        }
    }

    public class BuzzerControllerInterface
    {
        private BuzzerController controller;
        private SerialPort? port;
        private string msg = "";

        public BuzzerControllerInterface(int ComPort, int Baudrate, BuzzerController ccontroller)
        {   
            controller = ccontroller;

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
                foreach (string mitem in pmsg.Split("{"))
                {
                    if (mitem != "")
                    {
                        var item = mitem;
                        item = "{" + item;
                        HandleData(item);
                    }
                }

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
                    controller.BuzzerList[index].TasterState = (bool)JsonMsg.Value;
                    break;
                case "Taster":
                    controller.TasterList[index].TasterState = (bool)JsonMsg.Value;
                    break;
                default:
                    throw new Exception("Buzzer PCB Output_Type not found");
            }
        }
        public void SendData(string json)
        {
            if (port == null) return;
            port.Write(json);
        }
    }

    public class Buzzer
    {
        private BuzzerController parent;

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
                        parent.BuzzerPress(myID);
                    }
                    else
                    {
                        parent.BuzzerRelease(myID);
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
                parent.SendData("{"+ $"\"Output_Type\" : \"Buzzer\", \"ID\" : {myID+1}, \"Value\" : {_LEDState.ToString().ToLower()}" + "}");
            }
        }

        public Buzzer(int pID, BuzzerController pparent)
        {
            myID = pID;
            parent = pparent;
        }
    }
    public class Taster
    {
        private BuzzerController parent;

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
                    parent.TasterEvent(myID, newVal);
                }
                _TasterState = newVal;
            }
        }


        public Taster(int pID, BuzzerController pparent)
        {
            myID = pID;
            parent = pparent;
        }
    }
    public class LED
    {
        private BuzzerController parent;
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

        public LED(int pID, BuzzerController pparent)
        {
            myID = pID;
            parent = pparent;
        }
        private void Send() 
        {
            parent.SendData("{" + $"\"Output_Type\" : \"LED\", \"ID\" : {myID}, \"Value\" : " +"{"+$"\"R\":{R}, \"G\":{G}, \"B\":{B}" + "}}");
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
