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

        public void setBuzzerState(bool Oval, bool Nval)
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

        private bool _LEDState = false;
        public bool LEDState
        {
            get
            {
                return _LEDState;
            }
            set
            {
                _LEDState = value;
                parent.SendData("{"+ $"\"Output_Type\" : \"Buzzer\", \"ID\" : {myID}, \"Value\" : {_LEDState.ToString().ToLower()}" + "}");
            }
        }

        public Buzzer(int pID, BuzzerController pparent)
        {
            myID = pID;
            parent = pparent;
        }
    }
    public class Taster
    {
        private BuzzerController parent;

        public int myID { get; private set; } = -1;

        public void setTasterState(bool Oval, bool Nval)
        {
            if (Oval == Nval)return;
            if (Nval != Oval)
                {
                    parent.TasterEvent(myID, Nval);
                }
        }

        public Taster(int pID, BuzzerController pparent)
        {
            myID = pID;
            parent = pparent;
        }
    }
    public class LED
    {
        private BuzzerController parent;
        public int myID { get; private set; } = -1;

        private int _R;
        public int R
        {
            get
            {
                return _R;
            }
            set
            {
                _R = value;
                Send();
            }
        }
        private int _G;
        public int G
        {
            get
            {
                return _G;
            }
            set
            {
                _G = value;
                Send();
            }
        }
        private int _B;
        public int B
        {
            get
            {
                return _B;
            }
            set
            {
                _B = value;
                Send();
            }
        }

        public LED(int pID, BuzzerController pparent)
        {
            myID = pID;
            parent = pparent;
        }
        private void Send() 
        {
            parent.SendData("{" + $"\"Output_Type\" : \"LED\", \"ID\" : {myID}, \"Value\" : " +"{"+$"\"R\":{R}, \"G\":{G}, \"B\":{B}" + "}}");
        }
        public void SetLEDColor(int pR, int pG, int pB)
        {
            _R = pR;
            _G = pG;
            _B = pB;
            Send();
        }
    }
}
