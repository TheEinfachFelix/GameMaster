﻿using Newtonsoft.Json;
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
        private static string def = "{\"Type\":\"Request\"," + $"\"IOType\" : \"Buzzer\",\"RequestType\":\"Get\",\"Request\":\"Collor\", \"ID\" : {myID}, \"Key\":\"";
        private BuzzerController parent;
        public int myID { get; private set; } = -1;

        private int _R;
        public int R
        {
            get
            {
                return parent.GetData(def + "R\"}");
            }
            set
            {
                SetLEDColor(value,G,B);
            }
        }
        private int _G;
        public int G
        {
            get
            {
                return parent.GetData(def + "G\"}");
            }
            set
            {
                SetLEDColor(R,value,B);
            }
        }
        private int _B;
        public int B
        {
            get
            {
                return parent.GetData(def + "B\"}");
            }
            set
            {
                SetLEDColor(R,G,value);
            }
        }

        public LED(int pID, BuzzerController pparent)
        {
            myID = pID;
            parent = pparent;
        }
        public void SetLEDColor(int pR, int pG, int pB)
        {
            parent.GetData("{\"Type\":\"Request\"," + $"\"IOType\" : \"LED\",\"RequestType\":\"Set\",\"Request\":\"Collor\", \"ID\" : {myID}, \"R\":{pR}, \"G\":{pG}, \"B\":{pB}"+"}");
        }
    }
}
