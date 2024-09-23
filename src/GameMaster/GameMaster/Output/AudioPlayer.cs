

using NAudio.Wave;

namespace GameMaster.Output
{
    
    public class AudioPlayer
    {
        public static WaveOutEvent? PlaySound(string sound)
        {
            if (!Game.GetInstance().AudioEnable) {return null; }
            WaveStream mystream;
            WaveOutEvent myevent;
            mystream = new AudioFileReader(sound);
            myevent = new();
            myevent.Init(mystream);
            myevent.Play();
            return myevent;
        }
    }
}
