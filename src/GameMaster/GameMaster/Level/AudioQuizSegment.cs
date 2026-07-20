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

        public List<AudioQuestion> Questions { get; set; } = [];

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
                StopCurrentAudio();

                if (value == 0)
                {
                    ShowStartScreen();
                    _CStep = 0;
                    return;
                }

                ShowStep(value);
            } 
        }

        private int calcPlayDurCountUpTo(int song)
        {
            int count = 1;

            for (int j = 0; j < song; j++)
            {
                count += Questions[j].Durations.Count + 1;
            }

            return count;
        }

        private int calcSongID(int step)
        {
            for (int song = 0; song < Questions.Count; song++)
            {
                if (step < calcPlayDurCountUpTo(song + 1))
                    return song;
            }

            return Questions.Count;
        }

        private (int Song, int StepInSong) GetStepInfo(int globalStep)
        {
            int song = calcSongID(globalStep);
            int stepInSong = globalStep - calcPlayDurCountUpTo(song);

            return (song, stepInSong);
        }

        private void ShowStep(int step)
        {
            var (songID, stepInSong) = GetStepInfo(step);

            if (songID >= Questions.Count)
            {
                _CStep = calcPlayDurCountUpTo(Questions.Count - 1)
                       + Questions[^1].Durations.Count;
                return;
            }

            var question = Questions[songID];

            _CStep = step;
            Points = question.Points;
            BuzzerDisabeled = false;

            if (stepInSong >= question.Durations.Count)
            {
                game.obsConnectorList[0].SetMainText(question.File);
            }
            else
            {
                game.obsConnectorList[0].SetMainText("");

                LastPlayed = new AudioPlayerSegment(
                    Path + question.File + ".mp3",
                    question.StartOffset);

                LastPlayed.PlaySound(question.Durations[stepInSong]);
            }
        }

        public string Path { get; set; }

        [JsonIgnore]
        public bool BuzzerDisabeled { get; set; }

        private AudioPlayerSegment? LastPlayed;
        private void StopCurrentAudio()
        {
            LastPlayed?.StopSound();
        }

        private void ShowStartScreen()
        {
            game.obsConnectorList[0].SetMainText(displayContent);
        }

        public void BuzzerPress(int BuzzerID)
        {
            if (BuzzerDisabeled) {return; }
            BuzzerDisabeled = true;
            AudioPlayer.PlaySound("C:/Users/felix/Downloads/buz.wav");
            StopCurrentAudio();

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

            int songID = calcSongID(CStep);
            CStep = calcPlayDurCountUpTo(songID) + Questions[songID].Durations.Count;
        }
    }
    public class AudioQuestion
    {
        public string Name { get; set; } = "";
        public string Beschreibung { get; set; } = "";
        public string displayContent { get; set; } = "";
        public string Path { get; set; } = "";
    }
}
