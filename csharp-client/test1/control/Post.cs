using System;
using Newtonsoft.Json;
using WebSocketSharp;

namespace test
{
    public class Post
    {
        private static string t = BaseBoard.GetBaseBoardStrMd5();
        static string url = "wss://ks.risid.com/wss/WebSocketHandler.ashx?code="+t;
       
        private WebSocket ws = new WebSocket(url);

        public Post()
        {
            ws.Connect();
        }

        public WebSocket GetWebSocket()
        {
            return ws;
        }
        public void PostData(Object infoStruct)
        {
            //发送数据
            string json = JsonConvert.SerializeObject(infoStruct);
            if (!ws.IsAlive)
            {
                ws.Connect();
            }
            ws.Send(json);
            Console.WriteLine("post");           
        }


    }
    





}
