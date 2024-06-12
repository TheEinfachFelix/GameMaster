
using Websocket.Client;
using Newtonsoft.Json;

namespace GameMaster.Output
{
    // Danke philip für deine dot2 Websocet Implementaion für den apc mini gedöns
    public class dot2Connector
    {
        Uri url = new("ws://localhost:80/");
        WebsocketClient? client;
        private int session;
        private int request;
        private string text = "";
        public int pageIndex = 0;
        public bool Ready = false;

        private static string notConnectedError = "client not connected";

        public bool Open()
        {
            if (client != null) { throw new Exception("already connected"); }
            client = new(url)
            {
                ReconnectTimeout = TimeSpan.FromSeconds(30)
            };
            client.ReconnectionHappened.Subscribe(info =>
            {
                Console.WriteLine("Reconnection happened, type: " + info.Type);
            });

            client.MessageReceived.Subscribe(msg =>
            {
                Console.WriteLine("Message received: " + msg);
                ParseMSG(msg.ToString());
            });

            client.Start();

            return true;
        }
        public void Close() 
        { 
            if (client == null) { return; }
            client.Dispose();
            Ready = false;
        }
        private void ParseMSG(string? pmsg)
        {
            if (pmsg == null) { return; }
            JsonMSG msg = JsonConvert.DeserializeObject<JsonMSG>(pmsg)!;
            HandleMSG(msg);
        }

        private void HandleMSG(JsonMSG obj)
        {
            if (client == null) { throw new Exception(notConnectedError); }
            request++;
            if (request >= 9)
            {
                client.Send("{\"session\":" + session + "}");
                client.Send(
                    "{\"requestType\":\"getdata\",\"data\":\"set\",\"session\":" +  session +
                    ",\"maxRequests\":1}");
                request = 0;
            }

            // open connection
            if (obj.status == "server ready")
            {
                Console.WriteLine("dot2 endpoint redy");
                client.Send("{ \"session\":0}");
            }
            if (obj.forceLogin == true)
            {
                Console.WriteLine("try to login...");
                if (obj.session != null) {session = (int)obj.session;}
                
                client.Send(
                    "{\"requestType\":\"login\",\"username\":\"remote\",\"password\":\"2c18e486683a3db1e645ad8523223b72\",\"session\":" +
                    obj.session +
                    ",\"maxRequests\":10}"
                );
            }
            //Check if session is ok
            if (obj.session == 0)
            {
                Console.WriteLine("CONNECTION ERROR");
                client.Send("{\"session\":" + session + "}");
            }
            if (obj.session != null)
            {
                if (obj.session == -1)
                {
                    Console.WriteLine("Please enable Web Remote, and set password to \"remote\"");

                    //reconnect after one second //TODO
                    //setTimeout(function() {
                    //    connect();
                    //}, 1000);
                }
                else if (obj.session == -2)
                {
                    Console.WriteLine("connections limit is reached");
                }
                else
                {
                    session = (int)obj.session;
                }
            }

            if (obj.text != null)
            {
                Console.WriteLine(obj.text);
                text = obj.text;
            }
            if (obj.responseType == "login" && obj.result == true)
            {
                //setInterval(interval, 100); //TODO: Set interval to recieve data from dot
                Console.WriteLine("logged in!");
                Console.WriteLine("session: " + session);
                Ready = true;
            }
            if (obj.responseType == "presetTypeList")
            {
                Console.WriteLine("Preset Type List");
            }
            if (obj.responseType == "presetTypes")
            {
                Console.WriteLine("Preset Types");
            }
            if (obj.responseType == "getdata")
            {
                Console.WriteLine("Get Data");
            }
            if (obj.responseType == "playbacks")
            {
                //wsUpdateLights(obj); //TODO: parese the interval data
            }
        }

        //interval function requests data from dot2 endpoint
        private void interval()
        {
            if (client == null) { throw new Exception(notConnectedError); }
            if (session > 0)
            {
                //client.Send(
                //  "{"requestType":"playbacks","startIndex":[300,400,500,600,700,800],"itemsCount":[16,16,16,16,16,16],"pageIndex":" + pageIndex +
                //    ","itemsType":[3,3,3,3,3,3],"view":3,"execButtonViewMode":2,"buttonsViewMode":0,"session":" + session +
                //    ","maxRequests":1}");
                //client.Send(
                //  "{"requestType":"playbacks","startIndex":[6,106,206],"itemsCount":[8,8,8],"pageIndex":" + pageIndex2 +
                //    ","itemsType":[2,3,3],"view":2,"execButtonViewMode":1,"buttonsViewMode":0,"session":" + session +
                //    ","maxRequests":1}");
                client.Send(
                    "{\"requestType\":\"playbacks\",\"startIndex\":[0,100,200],\"itemsCount\":[6,6,6],\"pageIndex\":" + pageIndex +
                    ",\"itemsType\":[2,3,3],\"view\":2,\"execButtonViewMode\":1,\"buttonsViewMode\":0,\"session\":"   + session +
                    ",\"maxRequests\":1}");
            }
        }
        
        public void SendButtonPress(int ButtonID)
        {
            if (client == null) { throw new Exception(notConnectedError);}

            client.Send(
                "{\"requestType\":\"playbacks_userInput\",\"cmdline\":\"\",\"execIndex\":" + (ButtonID-1) +
                ",\"pageIndex\":" + pageIndex +
                ",\"buttonId\":0,\"pressed\":true,\"released\":false,\"type\":0,\"session\":" + session +
                ",\"maxRequests\":0}");
        }

        public void SetFaderValue(int FaderID, double FaderValue)
        {
            if (client == null) { throw new Exception(notConnectedError);}

            client.Send(
                "{\"requestType\":\"playbacks_userInput\",\"execIndex\":" + (FaderID-1) +
                ",\"pageIndex\":" + pageIndex +
                ",\"faderValue\":" + (FaderValue/100).ToString().Replace(",",".") +
                ",\"type\":1,\"session\":" + session +
                ",\"maxRequests\":0}");
        }

        public void SetBlackOut(bool IsBlackOutOn)


        {
            if (client == null) { throw new Exception(notConnectedError); }

            int faderValue = 100;
            if (IsBlackOutOn) { faderValue = 0; }

            client.Send(
            "{\"command\":\"SpecialMaster 2.1 At "+
            faderValue +
            "\",\"session\":" +
            session +
            ",\"requestType\":\"command\",\"maxRequests\":0}"
            );
        }

    }

    public class JsonMSG
    {
        // {"status":"server ready", "appType":"dot2"}
        public string? status { get; set; }
        public string? appType { get; set; }

        // {"realtime":false,"session":1,"forceLogin":true,"worldIndex":0}
        public bool realtime { get; set; }
        public int? session {  get; set; }
        public bool forceLogin { get; set; }
        public int worldIndex { get; set; }



        // {"prompt":" [Fixture]>","promptcolor":"#808080","realtime":false,"responseType":"login","result":true,"worldIndex":0}
        public string? prompt { get; set; }
        public string? promptcolor { get; set; }
        public string? responseType { get; set; }
        public bool result { get; set; }


        // other stuff
        public string? text { get; set; }


        // {"realtime":false,"connections_limit_reached":"","session":-2}
        public string? connections_limit_reached { get; set; }
    }

    //client.Send(
    //"{\"command\":\"SpecialMaster 2.1 At 0\",\"session\":" +
    //session +
    //",\"requestType\":\"command\",\"maxRequests\":0}"
    //);
}

//{"realtime":false,"responseType":"playbacks","responseSubType":2,"iPage":1,"itemGroups":[{"itemsType":2,"cntPages":23,"items":[[{"i":{"t":"1","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":0,"isRun":0,"executorBlocks":[{"button1":{"id":0,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}},"button2":{"id":1,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}},"fader":{"v":0.000,"min":0.000,"max":0.000},"button3":{"id":2,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}}}]}],[{"i":{"t":"2","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":1,"isRun":0,"executorBlocks":[{"button1":{"id":0,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}},"button2":{"id":1,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}},"fader":{"v":0.000,"min":0.000,"max":0.000},"button3":{"id":2,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}}}]}],[{"i":{"t":"3","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":2,"isRun":0,"executorBlocks":[{"button1":{"id":0,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}},"button2":{"id":1,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}},"fader":{"v":0.000,"min":0.000,"max":0.000},"button3":{"id":2,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}}}]}],[{"i":{"t":"4","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":3,"isRun":0,"executorBlocks":[{"button1":{"id":0,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}},"button2":{"id":1,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}},"fader":{"v":0.000,"min":0.000,"max":0.000},"button3":{"id":2,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}}}]}],[{"i":{"t":"5","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":4,"isRun":0,"executorBlocks":[{"button1":{"id":0,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}},"button2":{"id":1,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}},"fader":{"v":0.000,"min":0.000,"max":0.000},"button3":{"id":2,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}}}]}],[{"i":{"t":"6","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":5,"isRun":0,"executorBlocks":[{"button1":{"id":0,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}},"button2":{"id":1,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}},"fader":{"v":0.000,"min":0.000,"max":0.000},"button3":{"id":2,"t":"Empty","s":false,"c":"#808080","leftLED":{},"rightLED":{}}}]}]]},{"itemsType":3,"iExecOff":100,"cntPages":23,"items":[[{"i":{"t":"101","c":"#FFFFFF"},"oType":{"t":"LT","c":"#FFFFFF"},"oI":{"t":"2","c":"#FFFFFF"},"tt":{"t":"Sequ","c":"#FFFFFF"},"bC":"#E8A901","bdC":"#FFFF80","cues":{"bC":"#3F3F20","items":[{"t":"    1 Cue","c":"#FFFFFF","pgs":{"v":1.000,"bC":"#0000FF"}}]},"combinedItems":1,"iExec":100,"isRun":1,"bottomButtons":{"items":[{"fader":{"v":1.000,"bC":"#808000"},"n":{"t":"Toggle","c":"#FFFFFF"}}]}}],[{"i":{"t":"102","c":"#C0C0C0"},"oType":{"t":"LT","c":"#FFFFFF"},"oI":{"t":"3","c":"#FFFFFF"},"tt":{"t":"Sequ","c":"#FFFFFF"},"bC":"#000000","bdC":"#FFFF80","cues":{"bC":"#3F3F20","items":[{"t":"    1 Cue","c":"#FFFFFF","pgs":{}}]},"combinedItems":1,"iExec":101,"isRun":0,"bottomButtons":{"items":[{"fader":{"v":1.000,"bC":"#808000"},"n":{"t":"Toggle","c":"#C0C0C0"}}]}}],[{"i":{"t":"103","c":"#C0C0C0"},"oType":{"t":"LT","c":"#FFFFFF"},"oI":{"t":"4","c":"#FFFFFF"},"tt":{"t":"Sequ","c":"#FFFFFF"},"bC":"#000000","bdC":"#FFFF80","cues":{"bC":"#3F3F20","items":[{"t":"    1 Cue","c":"#FFFFFF","pgs":{}}]},"combinedItems":1,"iExec":102,"isRun":0,"bottomButtons":{"items":[{"fader":{"v":1.000,"bC":"#808000"},"n":{"t":"Toggle","c":"#C0C0C0"}}]}}],[{"i":{"t":"104","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":103,"isRun":0}],[{"i":{"t":"105","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":104,"isRun":0}],[{"i":{"t":"106","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":105,"isRun":0}]]},{"itemsType":3,"iExecOff":200,"cntPages":23,"items":[[{"i":{"t":"201","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":200,"isRun":0}],[{"i":{"t":"202","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":201,"isRun":0}],[{"i":{"t":"203","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":202,"isRun":0}],[{"i":{"t":"204","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":203,"isRun":0}],[{"i":{"t":"205","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":204,"isRun":0}],[{"i":{"t":"206","c":"#000000"},"oType":{"t":""},"oI":{"t":""},"tt":{"t":""},"bC":"#404040","bdC":"#3D3D3D","cues":{},"combinedItems":1,"iExec":205,"isRun":0}]]}],"worldIndex":0}