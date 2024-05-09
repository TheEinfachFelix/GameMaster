namespace GameMaster
{
    public class Game
    {
        private static Game? instance;

        public List<IPlayer>? Players { get; set; }
        public List<ILevel>? Levels { get; set; }

        private int pLevelID  = 0;
        public int LevelID
        {
            get 
            { 
                return pLevelID;
            }
            set
            {
                var a = value; 
                pLevelID = 0;
                SetLevel(a);
                pLevelID = a;
            }
        }

        private ILevel? pCLevel = null;
        public ILevel? CLevel 
        { get 
            { 
                return pCLevel; 
            } 
        }

        private Game()
        {
            
        }
        public void ResetAll()
        {
            Players = [];
            Levels = [];
            pLevelID = 0;
            pCLevel = null;
        }

        public static Game GetInstance()
        {
            instance ??= new Game();
            return instance;
        }

        // Buzzer Routing
        public void BuzzerPress(int BuzzerID)
        {
            if (CLevel == null) { throw new Exception("CLevel is null"); }
            
            CLevel.BuzzerPress(BuzzerID);
        }
        public void BuzzerRelease(int BuzzerID)
        {
            if (CLevel == null) { throw new Exception("CLevel is null"); }

            CLevel.BuzzerRelease(BuzzerID);
        }

        // Seting the Levels
        public bool NextLevel()
        {
            if (Levels == null) { throw new Exception("Levels is null"); }
            if (Levels.ElementAtOrDefault(LevelID+1) == null) { return false; }

            if (CLevel == null) 
            {
                LevelID *= 1;
            }
            else
            {
                LevelID++;
            }

            return true;
        }
        private void SetLevel(int LvlID)
        {
            if (Levels == null) { throw new Exception("Levels is null"); }
            if (Levels.ElementAtOrDefault(LvlID) == null) { throw new Exception("LevelID is out of range"); }


            if (CLevel != null) { CLevel.Clear(); }
            
            pCLevel = Levels.ElementAt(LvlID);
            CLevel.Setup();
        }
    }
}
