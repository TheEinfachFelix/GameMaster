using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.CompilerServices;


namespace GameMaster.Input
{
    public class Buzzer
    {
        private BuzzerController parent;

        public int myID { get; set; } = -1;

        public Buzzer(int pID, BuzzerController pparent)
        {
            myID = pID;
            parent = pparent;
        }

        public void HandleEvent(bool Oval, bool Nval)
        {
            if (Oval == Nval)return;
            if (!Nval)
            {
                parent.BuzzerPress(myID);
            }
            else
            {
                parent.BuzzerRelease(myID);
            }
        }

        public bool InputState
        {
            get
            {
                return parent.GetData("{\"Type\":\"Request\"," + $"\"IOType\" : \"Buzzer\",\"RequestType\":\"Get\",\"Request\":\"InputState\", \"ID\" : {myID}" + "}");
            }
            set
            {
                       parent.GetData("{\"Type\":\"Request\"," + $"\"IOType\" : \"Buzzer\",\"RequestType\":\"Set\",\"Request\":\"InputState\", \"ID\" : {myID}, \"Value\":{value.ToString().ToLower()}" + "}");
            }
        }

        public bool LEDState
        {
            get
            {
                return parent.GetData("{\"Type\":\"Request\"," + $"\"IOType\" : \"Buzzer\",\"RequestType\":\"Get\",\"Request\":\"State\", \"ID\" : {myID}" + "}");
            }
            set
            {
                       parent.GetData("{\"Type\":\"Request\"," + $"\"IOType\" : \"Buzzer\",\"RequestType\":\"Set\",\"Request\":\"State\", \"ID\" : {myID}, \"Value\":{value.ToString().ToLower()}" + "}");
            }
        }
    }
}