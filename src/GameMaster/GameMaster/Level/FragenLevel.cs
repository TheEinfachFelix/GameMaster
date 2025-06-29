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
                game.obsConnectorList[0].SetMainText(QuestionList[CStep]);
                BuzzerDisabeled = false;
            } 
        }
        public List<String> QuestionList { get; set; }
        public List<int> QuestionPoints { get; set; }

        public string displayContent { get; set; }

        [JsonIgnore]
        public bool BuzzerDisabeled { get; set; }

        public void BuzzerPress(int BuzzerID)
        {
            Trace.WriteLine("buzzer");
            if (BuzzerDisabeled) {return; }
            AudioPlayer.PlaySound("C:/Users/felix/Downloads/buz.wav");
            Trace.WriteLine("buzzer");
            BuzzerDisabeled = true;

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
