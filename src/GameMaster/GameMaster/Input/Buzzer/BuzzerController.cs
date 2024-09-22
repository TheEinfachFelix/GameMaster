using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Ports;
using System.Security.AccessControl;


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



        public int ComPort { get; set; }
        public int Baudrate { get; set; }
        public bool Dummy { get; set; }
        public List<int> Dot2Valus { get; set; }


        [JsonIgnore]
        public BuzzerControllerInterface? BuzzerControlerInterface;
        [JsonIgnore]
        public int ID;
        

        public BuzzerController(int pamountOfBuzzer, int pamountOfTaster) 
        {
            AmountOfBuzzer =  pamountOfBuzzer;
            AmountOfTaster = pamountOfTaster;
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


            if (Dummy) return;
            BuzzerControlerInterface = new(ComPort, Baudrate, this);
            BuzzerControlerInterface.Setup();

        }
        public void TasterEvent(int TasterID, bool Value)
        {
            if (game == null) throw new Exception("Game is null");
            
            // event weitergeben
            game.BuzzerControll.TasterEvent(ID,TasterID, Value);
        }
        public void BuzzerPress(int BuzzerID)
        {
            if (game == null) throw new Exception("Game is null");

            // make dot2 licht stuff
            game.dot2ConnectorList[0].SendButtonPress(Dot2Valus[BuzzerID]);

            // event weitergeben
            game.BuzzerControll.BuzzerPress(ID,BuzzerID);
        }
        public void BuzzerRelease(int BuzzerID)
        {
            if (game == null) throw new Exception("Game is null");


            // event weitergeben
            game.BuzzerControll.BuzzerRelease(ID,BuzzerID);
        }
        public string GetData(string json)
        {
            if (BuzzerControlerInterface == null) throw new Exception("Buzzer Controller not properly setup");
            if (Dummy) return"";
            
            string outp = BuzzerControlerInterface.GetData(json).Result;
            return outp;
        }
    }
}
