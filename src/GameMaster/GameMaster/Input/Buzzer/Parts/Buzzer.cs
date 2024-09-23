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

        string msgStart = "{\"Type\":\"Request\", \"IOType\" : \"Buzzer\",\"RequestType\":\"";

        public Buzzer(int pID, BuzzerController pparent)
        {
            myID = pID;
            parent = pparent;
        }

        public void HandleEvent(bool Oval, bool Nval)
        {
            if (Oval == Nval) return;
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
                return bool.Parse(parent.GetData(msgStart + $"Get\",\"Request\":\"InputState\", \"ID\" : {myID.ToString()}" + "}"));
            }
            set
            {
                parent.GetData(msgStart + $"Set\",\"Request\":\"InputState\", \"ID\" : {myID.ToString()}, \"Value\":{value.ToString().ToLower()}" + "}");
            }
        }

        public bool LedState
        {
            get
            {
                return bool.Parse(parent.GetData(msgStart + $"Get\",\"Request\":\"State\", \"ID\" : {myID.ToString()}" + "}"));
            }
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
        public bool isDisabeled
        {
            get
            {
                return bool.Parse(parent.GetData(msgStart + $"Get\",\"Request\":\"isDisabeled\"" + "}"));
            }
            set
            {
                parent.GetData(msgStart + $"Set\",\"Request\":\"isDisabeled\", \"Value\":{value.ToString().ToLower()}" + "}");
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
        public int LedPin
        {
            get
            {
                return int.Parse(parent.GetData(msgStart + $"Get\",\"Request\":\"LedPin\", \"ID\" : {myID.ToString()}" + "}"));
            }
        }
    }
}