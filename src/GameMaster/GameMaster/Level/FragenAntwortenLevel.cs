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
    public class FragenAntwortenLevel : ILevel
    {
        private Game game = Game.GetInstance();
        public string Name { get; set; }
        public string Beschreibung { get; set; }
        [JsonIgnore]
        public int Points { get; set; }

        private List<string> Fields { get; } = new List<string> { "F1", "F2", "F3", "F4" };

        private int _CStep;
        [JsonIgnore]
        public int CStep //TODO
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
                int step = _CStep % 6;
                int substep = _CStep - step * 6;
                var currentQuestion = QuestionList[step];
                Points = currentQuestion.Points;
                if (substep == 0)
                {
                    game.obsConnectorList[0].SetMainText("");
                    for (global::System.Int32 i = 0; i < Fields.Count; i++)
                    {
                        var item = Fields[i];
                        game.obsConnectorList[0].SetTextfieldValue(item, "");
                    }
                }
                else if (substep == 1)
                {
                    game.obsConnectorList[0].SetMainText(currentQuestion.Question);

                }
                else {
                    int index = substep - 2;
                    game.obsConnectorList[0].SetTextfieldValue(Fields[index], currentQuestion.Answers[index]);
                }

                BuzzerDisabeled = false;
            } 
        }
        public List<QuestionItem> QuestionList { get; set; }

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
        }

        public void GO(int steps = 1)
        {
            CStep += steps;
        }

        public void Setup()
        { 
            game = Game.GetInstance();
            CStep = 0;
            game.obsConnectorList[0].SetScene(obsConnector.normal);
        }

        public void WinnerIs(int PlayerID)
        {
            game.Players[PlayerID].Points += Points;
            CStep++;
        }

    }

    public class QuestionItem
    {
        public string Question { get; set; }
        public int Points { get; set; }
        public List<string> Answers { get; set; }
    }
}
