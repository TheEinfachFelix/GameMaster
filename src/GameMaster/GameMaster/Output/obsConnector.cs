using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System.Diagnostics;
using System.Reflection;


namespace GameMaster.Output
{
    public class obsConnector
    {
        protected OBSWebsocket obs;

        public string IP { get; private set; }
        public string PW { get; private set; }
        public string Profile { get; private set; }
        public string SceneCol { get; private set; }


        public obsConnector(string pIP, string pPW, string pProfile = "", string pSzeneCol = "")
        {
            obs = new();

            obs.Connected += OnConnect;
            obs.Disconnected += OnDisconnect;

            IP = pIP;
            PW = pPW;
            Profile = pProfile;
            SceneCol = pSzeneCol;

            if (! Connect()) throw new Exception("OBS Connection not possible \nread Trace for more Detail");

            printError(obs.IsConnected.ToString());
            Setup();
        }

        ~obsConnector() 
        {
            obs.Disconnect();
        }

        private void printMSG(string text)
        {
            Trace.WriteLine($"obsConnector: {text}");
        }
        private void printError(string text)
        {
            printMSG($"ERROR: -->{text}<--");
        }

        private void OnConnect(object? sender, EventArgs e)
        {
            printMSG("Connected");
        }
        private void OnDisconnect(object? sender, OBSWebsocketDotNet.Communication.ObsDisconnectionInfo e)
        {
            printMSG("Disconnected");

            if (e.ObsCloseCode == OBSWebsocketDotNet.Communication.ObsCloseCodes.AuthenticationFailed)
            {
                printError("uthentication failed.");
                return;
            }
            else if (e.WebsocketDisconnectionInfo != null)
            {
                if (e.WebsocketDisconnectionInfo.Exception != null)
                {
                    printError($"Connection failed: CloseCode: {e.ObsCloseCode} Desc: {e.WebsocketDisconnectionInfo?.CloseStatusDescription} Exception:{e.WebsocketDisconnectionInfo?.Exception?.Message}\nType: {e.WebsocketDisconnectionInfo.Type}");
                }
                else
                {
                    printError($"Connection failed: CloseCode: {e.ObsCloseCode} Desc: {e.WebsocketDisconnectionInfo?.CloseStatusDescription}\nType: {e.WebsocketDisconnectionInfo.Type}");
                }
            }
            else
            {
                printError($"Connection failed: CloseCode: {e.ObsCloseCode}");
                return;
            }
        }

        public bool Connect()
        {
            if (!obs.IsConnected)
            {
                try
                {
                    obs.ConnectAsync(IP, PW);
                    while (!obs.IsConnected) { }
                    return true;
                }
                catch (Exception ex)
                {
                    printError("Connect failed : " + ex.Message);
                }
            }
            return false;
        }
        private void Setup()
        {
            if (Profile != "") obs.SetCurrentProfile(Profile);
            if (SceneCol != "") obs.SetCurrentSceneCollection(SceneCol);
        }

        public void SetScene(string SceneName)
        {
            if (!obs.IsConnected)
            {
                printError("not Connected");
                return;
            }
            obs.SetCurrentProgramScene(SceneName);
        }

    }
}
//Identifiziert