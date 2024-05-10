using GameMaster.Output;

namespace GameMaster
{
    public class TestLevel : ILevel
    {
        private Game game = Game.GetInstance();
        public void Setup()
        {
            Console.WriteLine("Setup Level ID is: " + game.LevelID);
        }
        public void BuzzerPress(int BuzzerID)
        {

            BasicAudioPlayer b = new(@"C:\Users\felix\Downloads\");
            b.PlayWAV("Zu_gut_fuer_dich");
            Console.WriteLine("BuzzerPress" + BuzzerID.ToString());
            if (BuzzerID == 0) 
            { 
                if (!game.NextLevel()) 
                { 
                    game.LevelID = 0;
                }
            }
            Console.WriteLine("Level ID is: " + game.LevelID);
            
        }

        public void BuzzerRelease(int BuzzerID)
        {
            Console.WriteLine("BuzzerRelease" + BuzzerID.ToString());
        }

        public void Clear()
        {
            Console.WriteLine("Clear");
            BasicAudioPlayer a = new(@"C:\Users\felix\Downloads\");
            a.PlayWAV("Test-1");

        }
    }
}
