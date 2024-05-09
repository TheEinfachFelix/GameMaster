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
        }
    }
}
