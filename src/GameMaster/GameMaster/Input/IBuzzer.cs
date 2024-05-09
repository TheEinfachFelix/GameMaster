namespace GameMaster.Input
{
    internal interface IBuzzer
    {

        public void Start(int ComPort, int Rate);
        public void Stop();
        
    }
}
