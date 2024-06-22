namespace GameMaster
{
    public interface ILevel
    {
        public void Setup();
        public void Clear();
        public void BuzzerPress(int BuzzerID);
        public void BuzzerRelease(int BuzzerID);

        public void WinnerIs(int PlayerID);

        public void GO(int steps = 1);

        public string Name { get; set; }
        public string Beschreibung { get; set; }
        public int Points {  get; set; }
        public int CStep { get; set; }

        public string displayContent { get; set; }
    }
}
