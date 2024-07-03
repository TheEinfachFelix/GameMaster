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
    public class FragenLevel : ILevel
    {
        private Game game = Game.GetInstance();
        dot2Connector dot2;
        public string Name { get; set; }
        public string Beschreibung { get; set; }
        [JsonIgnore]
        public int Points { get; set; }

        private int _CStep;
        [JsonIgnore]
        public int CStep 
        { 
            get 
            {
                return _CStep; 
            } 
            set 
            {
                if (QuestionList.Count()-1 < value)
                {
                    return;
                }
                _CStep = value;
                Points = QuestionPoints[CStep];
                displayContent = QuestionList[CStep];
            } 
        }
        [JsonIgnore]
        public string displayContent { get; set; }
        public int displayFontSize { get; set; }

        public List<String> QuestionList { get; set; }
        public List<int> QuestionPoints { get; set; }

        public void BuzzerPress(int BuzzerID)
        {
            if (BuzzerID == 0) { 
                dot2.SendButtonPress(101);
            }
            if (BuzzerID == 1)
            {
                dot2.SendButtonPress(102);
            }
            Trace.WriteLine(BuzzerID);
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

            game = Game.GetInstance();
            dot2 = game.dot2ConnectorList[0];
            CStep = 0;
            game.obsConnectorList[0].SetScene("FragenLVL");

        }

        public void WinnerIs(int PlayerID)
        {
            game.Players[PlayerID].Points += Points;
            CStep++;
        }
    }
}
