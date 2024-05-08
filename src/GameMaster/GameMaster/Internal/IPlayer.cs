using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaster
{
    public interface IPlayer
    {
        string Name { get; }
        int Points { get; set; }
    }
}
