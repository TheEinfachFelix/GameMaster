using GameMaster.Output;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster.Level
{
    public class EinfachesLevel : ILevel
    {
        private Game game = Game.GetInstance();
        dot2Connector dot2;
        public string Name { get; set; }
        public string Beschreibung { get; set; }
        public int Points { get; set; }

        [JsonIgnore]
        public int CStep { get; set; } = -1;
        public string displayContent { get; set; }
        public int displayFontSize { get; set; }

        public bool BuzzerEnabled { get; set; } 
        [JsonIgnore]
        public bool BuzzerDisabeled { get; set; }

       
        public void BuzzerPress(int BuzzerID)
        {
            if ((!BuzzerEnabled) || BuzzerDisabeled) { return; }
            BuzzerDisabeled = true;
            if (BuzzerID == 0)
            {
                dot2.SendButtonPress(101);
            }
            if (BuzzerID == 1)
            {
                dot2.SendButtonPress(102);
            }
        }

        public void BuzzerRelease(int BuzzerID)
        {
        }
        public void TasterEvent(int TasterID, bool Value)
        {
        }
        public void Clear()
        {
            game.obsConnectorList[0].SetScene("normal");
        }

        public void GO(int steps = 1)
        {
            CStep += steps;
        }

        public void Setup()
        {
            BuzzerDisabeled = false;
            game = Game.GetInstance();
            dot2 = game.dot2ConnectorList[0];
            CStep = 0;
            game.obsConnectorList[0].SetScene("FragenLVL");

        }

        public void WinnerIs(int PlayerID)
        {
            BuzzerDisabeled = true;
            game.Players[PlayerID].Points += Points;
            CStep++;
        }
    }
}

