using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.CompilerServices;


namespace GameMaster.Input
{
    public class Taster
    {
        private BuzzerController parent;
        string msgStart = "{\"Type\":\"Request\", \"IOType\" : \"Taster\",\"RequestType\":\"";

        public int myID { get; private set; } = -1;
        public Taster(int pID, BuzzerController pparent)
        {
            myID = pID;
            parent = pparent;
        }

        public void HandleEvent(bool Oval, bool Nval)
        {
            if (Oval == Nval)return;
            if (Nval != Oval)
            {
                parent.TasterEvent(myID, Nval);
            }
        }

        public bool InputState
        {
            get
            {
                return bool.Parse(parent.GetData(msgStart + $"Get\",\"Request\":\"InputState\", \"ID\" : {myID.ToString()}" + "}"));
            }
            set
            {
                       parent.GetData(msgStart + $"Set\",\"Request\":\"InputState\", \"ID\" : {myID}, \"Value\":{value.ToString().ToLower()}" + "}");
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
                return int.Parse(parent.GetData(msgStart + $"Get\",\"Request\":\"Pin\", \"ID\" : {myID.ToString()}" + "}"));
            }
        }
    }
}