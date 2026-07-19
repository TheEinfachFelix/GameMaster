

using NAudio.Wave;
using System.IO;
using System.Threading.Tasks;

namespace GameMaster.Output
{

    public delegate void PlayingDone();

    public class AudioPlayerSegment
    {
        private WaveStream mystream;
        private TimeSpan StartPoint;
        public bool status;
        public event PlayingDone? ProcessCompleted;
        WaveOutEvent myevent;

        public AudioPlayerSegment(string sound, int startTimeMS)
        {
            if (!Game.GetInstance().AudioEnable) { throw new Exception("Audio not enabled"); }

            mystream = new AudioFileReader(sound);
            myevent = new();
            StartPoint = new TimeSpan(startTimeMS);

            // skip to start time
            mystream.CurrentTime = StartPoint;
        }
        public async void PlaySound(int duratrionMs)
        {
            if (status) return;
            
            myevent.Init(mystream);
            myevent.Play();
            status = true;

            await InvokeDelayedAsync(() => StopSound(), duratrionMs);
        }

        public void StopSound()
        {
            myevent.Stop();
            myevent = new();

            mystream.CurrentTime = StartPoint;

            status = false;
            ProcessCompleted?.Invoke();  
        }


        private static async Task InvokeDelayedAsync(Action action, double delayInMs)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(delayInMs));
            action();
        }
    }
}
