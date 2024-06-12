using GameMaster.Output;
using System.Diagnostics;

namespace GameMaster
{
    public class TestLevel : ILevel
    {
        private Game game = Game.GetInstance();

        public string Name { get; set; } = "Sound";
        public string Beschreibung { get; set; } = "Spielt Sound";
        public int Points { get; set; } = 3;
        public int CStep { get; set; } = 0;

        public void Setup()
        {
            game = Game.GetInstance();
            Trace.WriteLine("Setup Level ID is: " + game.LevelID);
        }
        public void BuzzerPress(int BuzzerID)
        {

            BasicAudioPlayer b = new(@"C:\Users\felix\Downloads\");
            b.PlayWAV("Zu_gut_fuer_dich");
            Trace.WriteLine("BuzzerPress" + BuzzerID.ToString());
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

        public void Clear()
        {
            Trace.WriteLine("Clear");
            //BasicAudioPlayer a = new(@"C:\Users\felix\Downloads\");
            //a.PlayWAV("Test-1");

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
