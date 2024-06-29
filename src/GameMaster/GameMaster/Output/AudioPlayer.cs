

using NAudio.Wave;

namespace GameMaster.Output
{
    
    public class AudioPlayer
    {
        public AudioPlayer()
        {

        }

        public static void PlaySound(string sound)
        {
            WaveStream mystream;
            WaveOutEvent myevent;
            mystream = new AudioFileReader(sound);
            myevent = new();
            myevent.Init(mystream);
            myevent.Play();
        }


    }
}
