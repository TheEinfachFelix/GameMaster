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
                // Stop still Played songs
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

                int songID = calcSongID(value);
                Trace.WriteLine("songID" +  songID);
                // Limit cStep Value
                if (songID >= AudioList.Count)
                {
                    _CStep = calcPlayDurCountUpTo(AudioList.Count - 1)
                           + AudioPlayDuratrion[^1].Count;
                    return;
                }

                Points = QuestionPoints[songID];
                _CStep = value;
                BuzzerDisabeled = false;

                int durID = value - calcPlayDurCountUpTo(songID);
                Trace.WriteLine("durID" +  durID);

                // show result
                if (durID > AudioPlayDuratrion[songID].Count()-1)
                {
                    game.obsConnectorList[0].SetMainText(AudioList[songID]);
                }
                else
                {
                    game.obsConnectorList[0].SetMainText("");
                    LastPlayed = new(Path + AudioList[songID] + ".mp3", AudioStartOffset[songID]);
                    LastPlayed.PlaySound(AudioPlayDuratrion[songID][durID]);
                }
                Trace.WriteLine("done");
            } 
        }
        public List<string> AudioList { get; set; }
        public List<int> AudioStartOffset { get; set; }
        public List<List<int>> AudioPlayDuratrion { get; set; }
        private int calcPlayDurCountUpTo(int song)
        {
            int count = 1;

            for (int j = 0; j < song; j++)
            {
                count += AudioPlayDuratrion[j].Count + 1;
            }

            return count;
        }
        private int calcSongID(int step)
        {
            int songId = 0;
            while (step >= calcPlayDurCountUpTo(songId+1))
                songId ++;

            return songId;
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

            int songID = calcSongID(CStep);
            CStep = calcPlayDurCountUpTo(songID) + AudioPlayDuratrion[songID].Count;
        }
    }
}
