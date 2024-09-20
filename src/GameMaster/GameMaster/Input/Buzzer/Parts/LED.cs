using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.CompilerServices;


namespace GameMaster.Input
{
    public class LED
    {
        private BuzzerController parent;
        public int myID { get; private set; } = -1;

        private string def = "";

        private int _R;
        public int R
        {
            get
            {
                return int.Parse(parent.GetData(def + "R\"}"));
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
                return int.Parse(parent.GetData(def + "G\"}"));
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
                return int.Parse(parent.GetData(def + "B\"}"));
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
            def = "{\"Type\":\"Request\"," + $"\"IOType\" : \"Buzzer\",\"RequestType\":\"Get\",\"Request\":\"Collor\", \"ID\" : {myID.ToString()}, \"Key\":\"";
        }
        public void SetLEDColor(int pR, int pG, int pB)
        {
            parent.GetData("{\"Type\":\"Request\"," + $"\"IOType\" : \"LED\",\"RequestType\":\"Set\",\"Request\":\"Collor\", \"ID\" : {myID}, \"R\":{pR}, \"G\":{pG}, \"B\":{pB}"+"}");
        }
    }
}
