using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Websocket.Client;
using Newtonsoft.Json;
using System.Globalization;
using System.Security.Principal;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;

namespace GameMaster.Output
{
    // Danke philip für deine dot2 Websocet Implementaion für den apc mini gedöns
    public class dot2Connector
    {
        Uri url = new("ws://localhost:80/");
        WebsocketClient client;
        private int session;
        private int request;
        private string text = "";
        public int pageIndex = 0;
        public bool Ready = false;


        public bool Open()
        {
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
            //Task.Run(() => client.Send("{ message }"));
            //exitEvent.WaitOne();
            return true;
        }
        public void Close() 
        { 
            client.Dispose();
            Ready = false;
        }
        private void ParseMSG(string pmsg)
        {
            JsonMSG msg = JsonConvert.DeserializeObject<JsonMSG>(pmsg);
            HandleMSG(msg);
        }

        private void HandleMSG(JsonMSG obj)
        {
            request = request + 1;
            if (request >= 9)
            {
                client.Send("{\"session\":" + session + "}");
                client.Send(
                  "{\"requestType\":\"getdata\",\"data\":\"set\",\"session\":" +
                    session +
                    ",\"maxRequests\":1}"
                );
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
                //setInterval(interval, 100); //TODO
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
                //wsUpdateLights(obj); //TODO
            }
        }

        //interval function requests data from dot2 endpoint
        private void interval()
        {
            if (session > 0)
            {
                //client.Send(
                //  "{"requestType":"playbacks","startIndex":[300,400,500,600,700,800],"itemsCount":[16,16,16,16,16,16],"pageIndex":" +
                //    pageIndex +
                //    ","itemsType":[3,3,3,3,3,3],"view":3,"execButtonViewMode":2,"buttonsViewMode":0,"session":" +
                //    session +
                //    ","maxRequests":1}"
                //);
                //client.Send(
                //  "{"requestType":"playbacks","startIndex":[6,106,206],"itemsCount":[8,8,8],"pageIndex":" +
                //    pageIndex2 +
                //    ","itemsType":[2,3,3],"view":2,"execButtonViewMode":1,"buttonsViewMode":0,"session":" +
                //    session +
                //    ","maxRequests":1}"
                //);
                client.Send(
                    "{\"requestType\":\"playbacks\",\"startIndex\":[0,100,200],\"itemsCount\":[6,6,6],\"pageIndex\":" +
                    pageIndex +
                    ",\"itemsType\":[2,3,3],\"view\":2,\"execButtonViewMode\":1,\"buttonsViewMode\":0,\"session\":" +
                    session +
                    ",\"maxRequests\":1}"
                );
            }
        }
        
        public void SendButtonPress(int ButtonID)
        {
            ButtonID--;
            client.Send(
            "{\"requestType\":\"playbacks_userInput\",\"cmdline\":\"\",\"execIndex\":" +
            ButtonID +
            ",\"pageIndex\":" +
            pageIndex +
            ",\"buttonId\":0,\"pressed\":true,\"released\":false,\"type\":0,\"session\":" +
            session +
            ",\"maxRequests\":0}");
        }

    }

    public class JsonMSG
    {
        // {"status":"server ready", "appType":"dot2"}
        public string status { get; set; }
        public string appType { get; set; }

        // {"realtime":false,"session":1,"forceLogin":true,"worldIndex":0}
        public bool realtime { get; set; }
        public int? session {  get; set; }
        public bool forceLogin { get; set; }
        public int worldIndex { get; set; }



        // {"prompt":" [Fixture]>","promptcolor":"#808080","realtime":false,"responseType":"login","result":true,"worldIndex":0} //TODO
        public string responseType { get; set; }
        public bool result { get; set; }



        // other stuff
        public string? text { get; set; }


        // {"realtime":false,"connections_limit_reached":"","session":-2} //TODO
    }

    //client.Send(
    //"{\"command\":\"SpecialMaster 2.1 At 0\",\"session\":" +
    //session +
    //",\"requestType\":\"command\",\"maxRequests\":0}"
    //);
}



