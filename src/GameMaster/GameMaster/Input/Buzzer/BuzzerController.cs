using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Ports;


namespace GameMaster.Input
{
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


        private bool _BuzzerDisabeled = false;
        [JsonIgnore]
        public bool BuzzerDisabeled 
        {  get
            {
                return _BuzzerDisabeled;
            }
        }
        [JsonIgnore]
        public BuzzerControllerInterface BuzzerControlerInterface;
        [JsonIgnore]
        public int ID;
        public void setBuzzerDisabeled(bool val)
        {
            _BuzzerDisabeled = val;

            if (val)
            {
                // disabeled collor
                var Collor = Game.GetInstance().BuzzerControll.BlockCollor;
                foreach (Buzzer item in BuzzerList)
                {
                    LEDListe[item.myID].SetLEDColor(Collor[0], Collor[1], Collor[2]);

                }
            }
            else
            {
                Game game = Game.GetInstance();
                // restore collor
                foreach (Buzzer item in BuzzerList)
                {
                    // get previouse collor
                    var Collor = game.BuzzerControll.NormalCollor;
                    if (item.TasterState)
                    {
                        Collor = game.BuzzerControll.PressCollor;
                    }
                    LEDListe[item.myID].SetLEDColor(Collor[0], Collor[1], Collor[2]);

                }
            }

        }

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
            BuzzerControlerInterface.Setup();


            setBuzzerDisabeled(false);
        }

        public void TasterEvent(int TasterID, bool Value)
        {
            // taster 0 ist disable
            if (TasterID == 0)
            {
                setBuzzerDisabeled(Value);
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
            if (BuzzerControlerInterface == null) throw new Exception("Buzzer Controller not properly setup");
            BuzzerControlerInterface.SendData(json);
        }
    }
}
