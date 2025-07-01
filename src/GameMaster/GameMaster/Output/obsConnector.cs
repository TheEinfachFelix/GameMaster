using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;


namespace GameMaster.Output
{
    public class obsConnector
    {
        public static string normal = "normal";
        public static string FragenLVL2x2 = "FragenLVL2x2";


        protected OBSWebsocket obs;

        public bool Enable { get; set; } = false;

        private string _ip;
        public string IP 
        { 
            get 
            { 
            return _ip;
            } 
            set 
            { 
                _ip = value;
                Connect();
            } 
        }
        private string _pw;
        public string PW
        {
            get
            {
                return _pw;
            }
            set
            {
                _pw = value;
                Connect();
            }
        }
        public string Profile { get; set; }
        public string SceneCol { get; set; }

        [JsonConstructor]
        public obsConnector(string pIP, string pPW, string pProfile = "", string pSzeneCol = "")
        {
            obs = new();

            obs.Connected += OnConnect;
            obs.Disconnected += OnDisconnect;

            _ip = pIP;
            _pw = pPW;
            IP = pIP; // run connect if all params where given
            Profile = pProfile;
            SceneCol = pSzeneCol;
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
                    printError($"Connection failed: CloseCode: {e.ObsCloseCode} Desc: {e.WebsocketDisconnectionInfo?.CloseStatusDescription} Exception:{e.WebsocketDisconnectionInfo?.Exception?.Message}\nType: {e.WebsocketDisconnectionInfo?.Type}");
                }
                else
                {
                    printError($"Connection failed: CloseCode: {e.ObsCloseCode} Desc: {e.WebsocketDisconnectionInfo?.CloseStatusDescription}\nType: {e.WebsocketDisconnectionInfo?.Type}");
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
            if (IP == null || PW == null || !Enable) return false;
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
        public void Setup()
        {
            if (!Enable) return;    
            Profile ??= "";
            SceneCol ??= "";
            if (Profile != "") obs.SetCurrentProfile(Profile);
            if (SceneCol != "") obs.SetCurrentSceneCollection(SceneCol);
        }

        public void SetScene(string SceneName)
        {
            if (!Enable) return;
            if (!obs.IsConnected)
            {
                printError("not Connected");
                return;
            }
            try
            {
                obs.SetCurrentProgramScene(SceneName);
            }
            catch (Exception e)
            {
                throw new Exception("The Scene \"" + SceneName + "\" Could not be set. This Error was thrown: " + e.Message);
            }
        }
        public void SetTextfieldValue(string Name, string value)
        {
            if (!Enable) return;
            if (!obs.IsConnected)
            {
                printError("not Connected");
                return;
            }
            var json = @"{
                  ""text"": ""val"",
                }";
            obs.SetInputSettings(inputName: Name, inputSettings: JObject.Parse(json.Replace("val",value)));
        }//obs.CreateSceneItem("1", "Text"); //add scene item Text
        public void SetItemVisibility(string scene, string item, bool value)
        {
            if (!Enable) return;
            if (!obs.IsConnected)
            {
                printError("not Connected");
                return;
            }
            obs.SetSceneItemEnabled(scene, obs.GetSceneItemId(scene, item, 0), value);

        }

        public void SetMainText(string text)
        {
            SetTextfieldValue("main",text);
            printMSG("Called"+text+"xx");
        }
    }
}