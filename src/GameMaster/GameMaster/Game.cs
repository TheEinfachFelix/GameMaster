namespace GameMaster
{
    public class Game
    {
        private static Game? instance;
        public List<IPlayer>? Players { get; set; }
        public List<ILevel>? Levels { get; set; }
        public int LevelID { get; set; }
        public ILevel? CLevel { get; set; }



        private Game()
        {
            
        }
        public static Game getInstance()
        {
            instance ??= new Game();
            return instance;
        }

    }
}
