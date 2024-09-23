using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.CompilerServices;


namespace GameMaster.Input
{
    public class LEDs
    {
        private BuzzerController parent;
        string msgStart = "{\"Type\":\"Request\", \"IOType\" : \"LED\",\"RequestType\":\"";

        public LEDs(BuzzerController pparent)
        {
            parent = pparent;
        }

        public string LedMode
        {
            get
            {
                return parent.GetData(msgStart + $"Get\",\"Request\":\"LedMode\"" + "}");
            }
            set
            {
                       parent.GetData(msgStart + $"Set\",\"Request\":\"LedMode\", \"Value\":\"{value}\"" + "}");
            }
        }
        public int Amount
        {
            get
            {
                return int.Parse(parent.GetData(msgStart + $"Get\",\"Request\":\"Amount\"" + "}"));
            }
        }
        public int Pin
        {
            get
            {
                return int.Parse(parent.GetData(msgStart + $"Get\",\"Request\":\"Pin\"" + "}"));
            }
        }
    }
}