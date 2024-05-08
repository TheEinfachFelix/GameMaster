using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster
{
    public interface ILevel
    {
        public void Setup();
        public void BuzzerPress(int BuzzerID);
        public void BuzzerRelease(int BuzzerID);
    }
}
