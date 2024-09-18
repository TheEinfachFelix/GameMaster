using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Ports;


namespace GameMaster.Input
{
    public class BuzzerController
    {
        static readonly string missingCollor = "No collor was set";
        Game? game;
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
        public List<int> Dot2Valus { get; set; };


        private bool _IODisabeled = false;
        [JsonIgnore]
        public bool IODisabeled 
        {  get
            {
                return _IODisabeled;
            }
        }
        [JsonIgnore]
        public BuzzerControllerInterface? BuzzerControlerInterface;
        [JsonIgnore]
        public int ID;
        public void SetIODisabeled(bool val)
        {
            _IODisabeled = val;

            if (val)
            {
                // disabeled collor for buzzer
                var Collor = Game.GetInstance().BuzzerControll.BlockCollor ?? throw new Exception(missingCollor);
                foreach (Buzzer item in BuzzerList)
                {
                    LEDListe[item.myID].SetLEDColor(Collor[0], Collor[1], Collor[2]);
                }
                // taster
                foreach (Taster item in TasterList)
                {
                    LEDListe[item.myID + 9].SetLEDColor(Collor[0], Collor[1], Collor[2]);
                }
            }
            else
            {
                Game game = Game.GetInstance();
                // restore collor
                foreach (Buzzer item in BuzzerList)
                {
                    // get previouse collor
                    var Collor = game.BuzzerControll.NormalCollor ?? throw new Exception(missingCollor);
                    if (item.InputState)
                    {
                        Collor = game.BuzzerControll.PressCollor ?? throw new Exception(missingCollor);
                    }
                    LEDListe[item.myID].SetLEDColor(Collor[0], Collor[1], Collor[2]);

                }
                // Taster
                foreach (Taster item in TasterList)
                {
                    // get previouse collor
                    var Collor = game.BuzzerControll.NormalCollor ?? throw new Exception(missingCollor);
                    if (item.InputState)
                    {
                        Collor = game.BuzzerControll.PressCollor ?? throw new Exception(missingCollor);
                    }
                    LEDListe[item.myID + 9].SetLEDColor(Collor[0], Collor[1], Collor[2]);

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

            SetIODisabeled(false);
        }
        public void TasterEvent(int TasterID, bool Value)
        {
            if (game == null) throw new Exception("Game is null");
            if (IODisabeled) return;
            // taster 0 ist disable
            if (TasterID == 0)
            {
                SetIODisabeled(Value);
                return;
            }

            var Collor = game.BuzzerControll.NormalCollor ?? throw new Exception(missingCollor);
            if (Value)
            {
                Collor = game.BuzzerControll.PressCollor ?? throw new Exception(missingCollor);
            }
            LEDListe[TasterID + 9].SetLEDColor(Collor[0], Collor[1], Collor[2]);
            
            // event weitergeben
            game.BuzzerControll.TasterEvent(ID,TasterID, Value);
        }
        public void BuzzerPress(int BuzzerID)
        {
            if (game == null) throw new Exception("Game is null");
            if (IODisabeled) return;

            // setzt die passende Farbe
            var Collor = game.BuzzerControll.PressCollor ?? throw new Exception(missingCollor);
            LEDListe[BuzzerID].SetLEDColor(Collor[0], Collor[1], Collor[2]);

            // make dot2 licht stuff
            game.dot2ConnectorList[0].SendButtonPress(Dot2Valus[BuzzerID]);

            // event weitergeben
            game.BuzzerControll.BuzzerPress(ID,BuzzerID);
        }
        public void BuzzerRelease(int BuzzerID)
        {
            if (game == null) throw new Exception("Game is null");
            if (IODisabeled) return;

            // setzt die passende Farbe
            var Collor = game.BuzzerControll.NormalCollor ?? throw new Exception(missingCollor);
            LEDListe[BuzzerID].SetLEDColor(Collor[0], Collor[1], Collor[2]);

            // event weitergeben
            game.BuzzerControll.BuzzerRelease(ID,BuzzerID);
        }
        public string GetData(string json)
        {
            if(Dummy) return"";
            if (BuzzerControlerInterface == null) throw new Exception("Buzzer Controller not properly setup");
            string outp = BuzzerControlerInterface.GetData(json);
            return outp;
        }
    }
}
