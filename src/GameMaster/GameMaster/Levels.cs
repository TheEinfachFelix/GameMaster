using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
