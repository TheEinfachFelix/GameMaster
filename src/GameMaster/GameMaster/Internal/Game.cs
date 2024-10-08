﻿using GameMaster.Input;
using GameMaster.Output;
using Newtonsoft.Json;
using OBSWebsocketDotNet.Types;
using System.Diagnostics;

namespace GameMaster
{

    public class Game
    {
        private static Game? instance;

        public List<obsConnector> obsConnectorList { get; set; } = [];
        public List<dot2Connector> dot2ConnectorList { get; set; } = [];

        public BuzzerControllerMngr BuzzerControll { get; set;}

        [JsonIgnore]
        public string CDisplayContent
        {
            get
            {
                if (CLevel == null)
                {
                    return "";
                }
                return CLevel.displayContent;
              
            }
            set
            {
                if (CLevel != null)
                {
                    CLevel.displayContent = value;
                }
            }
        }
        [JsonIgnore]
        public int CDisplayFontSize
        {
            get
            {
                if (CLevel == null)
                {
                    return 10;
                }
                return CLevel.displayFontSize;

            }
            set
            {
                if (CLevel != null)
                {
                    CLevel.displayFontSize = value;
                }
            }
        }

        public bool AudioEnable { get; set; } = true;

        public List<IPlayer> Players { get; set; } = [];
        public List<ILevel> Levels { get; set; } = [];

        private int pLevelID  = -1;
        [JsonIgnore]
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

        [JsonIgnore]
        private ILevel? pCLevel = null;
        [JsonIgnore]
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

        public void Setup()
        {


            Trace.WriteLine("Game Setup:");
            LevelID = 0; // causes the level setup to run

            Trace.Write("Setting up OBS...");
            foreach (var obs in obsConnectorList)
            {
                obs.Setup();
                obs.SetScene("normal");
            }
            Trace.WriteLine("  -DONE-");
            Trace.Write("Setting up Dot2...");
            foreach (var dot2 in dot2ConnectorList)
            {
                dot2.Open();
                while (!dot2.Ready && dot2.Enable) { }
            }
            Trace.WriteLine("  -DONE-");
            Trace.Write("Setting up Buzzer...");
            BuzzerControll.Setup();
            Trace.WriteLine("  -DONE-");
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
