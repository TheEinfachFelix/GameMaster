using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.CompilerServices;


namespace GameMaster.Input
{
    public class Taster
    {
        private BuzzerController parent;

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
                return bool.Parse(parent.GetData("{\"Type\":\"Request\"," + $"\"IOType\" : \"Taster\",\"RequestType\":\"Get\",\"Request\":\"InputState\", \"ID\" : {myID.ToString()}" + "}"));
            }
            set
            {
                       parent.GetData("{\"Type\":\"Request\"," + $"\"IOType\" : \"Taster\",\"RequestType\":\"Set\",\"Request\":\"InputState\", \"ID\" : {myID}, \"Value\":{value.ToString().ToLower()}" + "}");
            }
        }
    }
}