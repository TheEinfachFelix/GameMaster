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
    public class AudioQuizSegment : ILevel
    {
        private Game game = Game.GetInstance();

        public string Name { get; set; }
        public string Beschreibung { get; set; }
        [JsonIgnore]
        public int Points { get; set; }

        public string displayContent { get; set; }

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
                    LastPlayed.StopSound();
                }
                // initial Content setzen
                if (value == 0)
                {
                    game.obsConnectorList[0].SetMainText(displayContent);
                    _CStep = 0;
                    return;
                }

                int PlayStep = (value - 1) / 2;

                // Limit cStep Value
                if (AudioList.Count()-1 <PlayStep ) // TODO
                {
                    _CStep = (AudioList.Count() - 1) * 2;
                    return;
                }

                // Calc Song 
                int songID = 0;
                int prevCount = 0;
                for (int i = 0; i < AudioPlayDuratrion.Count(); i++)
                {
                    int val = calcPlayDurCountUpTo(i);
                    if ( val < prevCount)
                    {
                        prevCount = val;
                        songID = i;
                    }
                        
                }


                int durValue = PlayStep - calcPlayDurCountUpTo(songID);


                Points = QuestionPoints[songID];
                _CStep = value;
                BuzzerDisabeled = false;

                // Gerade ungerade Zahl Verhalten
                if (value % 2 == 0)
                {
                    game.obsConnectorList[0].SetMainText(AudioList[CStep / 2 - 1]);
                }
                else
                {
                    game.obsConnectorList[0].SetMainText("");
                    LastPlayed = new(Path + AudioList[songID] + ".mp3", AudioStartOffset[songID]);
                    LastPlayed.PlaySound(AudioPlayDuratrion[songID][durValue]);
                }                
            } 
        }
        public List<string> AudioList { get; set; }
        public List<int> AudioStartOffset { get; set; }
        public List<List<int>> AudioPlayDuratrion { get; set; }
        private int calcPlayDurCountUpTo(int i)
        {
            int count = 0;
            for (int j = 0; j < Math.Min(AudioPlayDuratrion.Count()-1,i); j++)
            {
                count += AudioPlayDuratrion[j].Count();
            }
            return count;
        }

        public List<int> QuestionPoints { get; set; }

        public string Path { get; set; }

        [JsonIgnore]
        public bool BuzzerDisabeled { get; set; }

        private AudioPlayerSegment? LastPlayed;

        public void BuzzerPress(int BuzzerID)
        {
            if (BuzzerDisabeled) {return; }
            BuzzerDisabeled = true;
            AudioPlayer.PlaySound("C:/Users/felix/Downloads/buz.wav");
            if (LastPlayed != null)
            {
                LastPlayed.StopSound();
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
}
