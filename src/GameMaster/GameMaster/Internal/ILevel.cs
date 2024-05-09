namespace GameMaster
{
    public interface ILevel
    {
        public void Setup();
        public void BuzzerPress(int BuzzerID);
        public void BuzzerRelease(int BuzzerID);
    }
}
