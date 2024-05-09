namespace GameMaster.Input
{
    public class TestBuzzer : IBuzzer
    {
        public void Start(int ComPort, int Rate)
        {
            Console.WriteLine("stop");
        }

        public void Stop()
        {
            Console.WriteLine("stop");
        }
    }
}
