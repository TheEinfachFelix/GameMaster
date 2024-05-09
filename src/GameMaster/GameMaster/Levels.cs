namespace GameMaster
{
    public class TestLevel : ILevel
    {        
        public void Setup()
        {
            Console.WriteLine("Setup");
        }
        public void BuzzerPress(int BuzzerID)
        {
            Console.WriteLine("BuzzerPress");
        }

        public void BuzzerRelease(int BuzzerID)
        {
            Console.WriteLine("BuzzerRelease");
        }
    }
}
