using Newtonsoft.Json;

namespace GameMaster
{
    public class Game
    {
        private static Game? instance;

        public List<IPlayer> Players { get; set; }
        public List<ILevel> Levels { get; set; }

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
            Players = [];
            Levels = [];
        }
        public void ResetAll()
        {
            Players = [];
            Levels = [];
            pLevelID = 0;
            pCLevel = null;
        }

        // Setup the Game
        public static Game GetInstance()
        {
            instance ??= new Game();
            return instance;
        }

        // Load and Store Config
        public static Game LoadFromJson(string inputJson)
        {
            Game newGame = JsonConvert.DeserializeObject<Game>(inputJson, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            })!;
            instance = newGame;
            return newGame;
        }
        public static Game LoadFromFile(string FileAddress)
        {
            using StreamReader r = new(FileAddress);
            string readJson = r.ReadToEnd();
            return LoadFromJson(readJson);
        }
        public static string SaveToJson()
        {
            return JsonConvert.SerializeObject(instance, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
            });
        }
        public static void SaveToFile(string saveToFile)
        {
            File.WriteAllText(saveToFile, SaveToJson());
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
            if (pCLevel == null) { throw new Exception("Clevel is Null Something went wrong"); }
            CLevel?.Setup();
        }
    }
}
