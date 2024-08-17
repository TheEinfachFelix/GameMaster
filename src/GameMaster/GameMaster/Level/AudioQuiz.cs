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
    public class AudioQuiz : ILevel
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
                if (LastPlayed != null)
                {
                    LastPlayed.Stop();
                }
                if (value == 0)
                {
                    displayContent = "Spiel 5";
                    return;
                }

                if (AudioList.Count()-1 < (value-1)/2)
                {
                    return;
                }

  
                Points = QuestionPoints[(CStep - (CStep % 2)) / 2];
                _CStep = value;
                BuzzerDisabeled = false;

                if (value % 2 == 0)
                {
                    displayContent = AudioList[CStep / 2 - 1];
                }
                else
                {
                    displayContent = "";
                    LastPlayed = AudioPlayer.PlaySound(Path + AudioList[((CStep - 1) / 2)] + ".mp3");
                }                
            } 
        }
        [JsonIgnore]
        public string displayContent { get; set; }
        public int displayFontSize { get; set; }

        public List<String> AudioList { get; set; }
        public List<int> QuestionPoints { get; set; }

        public string Path { get; set; }

        [JsonIgnore]
        public bool BuzzerDisabeled { get; set; }

        private NAudio.Wave.WaveOutEvent? LastPlayed;

        public void BuzzerPress(int BuzzerID)
        {
            if (BuzzerDisabeled) {return; }
            BuzzerDisabeled = true;
            AudioPlayer.PlaySound("C:/Users/felix/Downloads/buz.wav");
            if (LastPlayed != null)
            {
                LastPlayed.Stop();
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
