using GameMaster.Output;
using Newtonsoft.Json;
using System.Diagnostics;

namespace GameMaster
{
    public class TestLevel : ILevel
    {
        private Game game = Game.GetInstance();

        public string Name { get; set; }
        public string Beschreibung { get; set; }
        public int Points { get; set; } = 3;
        public int CStep { get; set; } = 0;
        public string displayContent { get; set; }
        public int displayFontSize { get; set; }

        [JsonIgnore]
        public bool BuzzerDisabeled { get; set; }
        public string path { get; set; } = "C:/Users/felix/Downloads/";

        public void Setup()
        {
            BuzzerDisabeled = false;
            game = Game.GetInstance();
        }
        public void BuzzerPress(int BuzzerID)
        {
            if (BuzzerDisabeled) { return; }
            BuzzerDisabeled = true;
            AudioPlayer.PlaySound(path + "Zu_gut_fuer_dich.wav");

            if (BuzzerID == 1) 
            { 
                if (!game.NextLevel()) 
                { 
                    game.LevelID = 0;
                }
            }
            Trace.WriteLine("Level ID is: " + game.LevelID);
            
        }

        public void BuzzerRelease(int BuzzerID)
        {
            Trace.WriteLine("BuzzerRelease" + BuzzerID.ToString());
        }
        public void TasterEvent(int TasterID, bool Value)
        {
        }

        public void Clear()
        {
            Trace.WriteLine("Clear");
            AudioPlayer.PlaySound(path + "Test-1.wav");

        }

        public void WinnerIs(int PlayerID)
        {
            game.Players[PlayerID].Points += Points;
        }

        public void GO(int steps = 1)
        {
            CStep += steps;
        }
    }
}
