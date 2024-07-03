using GameMaster;
using GameMaster.Input;
using GameMaster.Output;


namespace WebGameController.Models
{
    public class FirstLevel : ILevel
    {
        private Game game = Game.GetInstance();
        //dot2Connector dot2 = new();

        public string Name { get; set; }
        public string Beschreibung { get; set; }
        public int Points { get; set; }
        public int CStep { get; set; }
        public string displayContent { get; set; }
        public int displayFontSize { get; set; }
        public void BuzzerPress(int BuzzerID)
        {
            //dot2.SendButtonPress(101);

            //dot2.SetFaderValue(1, 100);
            //dot2.SetFaderValue(2, 100);
            //dot2.SetFaderValue(3, 100);
        }

        public void BuzzerRelease(int BuzzerID)
        {
            //dot2.SetFaderValue(1, 0);
            //dot2.SetFaderValue(2, 0);
            //dot2.SetFaderValue(3, 0);
        }

        public void Clear()
        {
        }

        public void GO(int steps = 1)
        {
            CStep += steps;
        }

        public void Setup()
        {
            game = Game.GetInstance();
        }

        public void TasterEvent(int TasterID, bool Value)
        {
        }

        public void WinnerIs(int PlayerID)
        {
            game.Players[PlayerID].Points += Points;
        }
    } 
}
