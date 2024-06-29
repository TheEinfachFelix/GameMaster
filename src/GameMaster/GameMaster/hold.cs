using GameMaster;
using GameMaster.Input;
using GameMaster.Output;


namespace WebGameController.Models
{
    public class FirstLevel : ILevel
    {
        private Game game = Game.GetInstance();
        //dot2Connector dot2 = new();

        public string Name { get; set; } = "Bzzer to .2";
        public string Beschreibung { get; set; } = "Setzt auf BuzzerPress dot2 Values";
        public int Points { get; set; } = 2;
        public int CStep { get; set; } = 1;
        public string displayContent { get; set; } = "peter";
        public int displayFontSize { get; set; } = 200;

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
            //dot2.Close();
        }

        public void GO(int steps = 1)
        {
            CStep += steps;
        }

        public void Setup()
        {
            game = Game.GetInstance();
            //dot2.Open();
            //while (!dot2.Ready) { }
        }

        public void WinnerIs(int PlayerID)
        {
            game.Players[PlayerID].Points += Points;
        }
    } 
}
