using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Ports;


namespace GameMaster.Input
{
    public class BuzzerControllerMngr
    {
        [JsonIgnore]
        Game? game;

        public bool isDisabeled { get; set; }

        public List<BuzzerController> BuzzerControllerList { get; set; } = [];

        public void Setup()
        {
            game = Game.GetInstance();
            if (isDisabeled) return;

            int i = 0;
            foreach (BuzzerController item in BuzzerControllerList)
            {
                item.Setup(i);
                i++;
            }
        }
        public void TasterEvent(int parentID, int BuzzerID, bool Value)
        {
            if (isDisabeled) return;
            if (game == null) throw new Exception("Game is null");
            if (game.CLevel == null) { throw new Exception("CLevel is null"); }

            game.CLevel.TasterEvent(BuzzerID, Value);
        }
        public void BuzzerPress(int parentID, int BuzzerID)        
        {
            if (game == null) throw new Exception("Game is null");
            if (isDisabeled) return;
            if (game.CLevel == null) { throw new Exception("CLevel is null"); }

            game.CLevel.BuzzerPress(BuzzerID);
        }
        public void BuzzerRelease(int parentID, int BuzzerID)
        {
            if (game == null) throw new Exception("Game is null");
            if (isDisabeled) return;
            if (game.CLevel == null) { throw new Exception("CLevel is null"); }

            game.CLevel.BuzzerRelease(BuzzerID);
        }
    }
}
