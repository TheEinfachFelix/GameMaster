namespace GameMaster
{
    public interface ILevel
    {
        public void Setup();
        public void Clear();
        public void BuzzerPress(int BuzzerID);
        public void BuzzerRelease(int BuzzerID);

        public String Name { get; set; }
        public String Beschreibung { get; set; }
    }
}
