
using System.Media;

namespace GameMaster.Output
{
    public class BasicAudioPlayer
    {
        private string path;
        public BasicAudioPlayer(string FilePath = "") 
        { 
            path = FilePath;
        }
        
        public void PlayWAV(string File)
        {
            SoundPlayer player = new SoundPlayer(path + File + ".wav");
            player.Play();
        }
    }
}
