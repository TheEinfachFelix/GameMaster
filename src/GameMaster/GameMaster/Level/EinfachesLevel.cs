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
        public string Name { get; set; }
        public string Beschreibung { get; set; }
        public int Points { get; set; }

        [JsonIgnore]
        public int CStep { get; set; } = -1;
        public bool BuzzerEnabled { get; set; } 
        [JsonIgnore]
        public bool BuzzerDisabeled { get; set; }

        public string displayContent { get; set; }

        public void BuzzerPress(int BuzzerID)
        {
            if ((!BuzzerEnabled) || BuzzerDisabeled) { return; }
            BuzzerDisabeled = true;
            AudioPlayer.PlaySound("C:/Users/felix/Downloads/buz.wav");

        }

        public void BuzzerRelease(int BuzzerID)
        {
        }
        public void TasterEvent(int TasterID, bool Value)
        {
        }
        public void Clear()
        {
        }

        public void GO(int steps = 1)
        {
            CStep += steps;
            //game.obsConnectorList[0].SetTextfieldValue("Text", "Uiiiiii");
        }

        public void Setup()
        {
            game = Game.GetInstance();
            BuzzerDisabeled = false;
            CStep = 0;
            game.obsConnectorList[0].SetScene(obsConnector.normal);
            game.obsConnectorList[0].SetMainText(displayContent);
        }

        public void WinnerIs(int PlayerID)
        {
            BuzzerDisabeled = true;
            game.Players[PlayerID].Points += Points;
            CStep++;
        }
    }
}

