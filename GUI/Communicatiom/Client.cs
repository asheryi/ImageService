using Newtonsoft.Json;
using SharedResources;
using SharedResources.Commands;
using SharedResources.Communication;
using SharedResources.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Model
{
    class Client
    {
        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer ;
        private IMessageHandler messageHandler;
        private IPEndPoint ep;
        //private EventHandler<ContentEventArgs> logReceive;
        //private EventHandler<ContentEventArgs> configReceive;
        // IDictionary<int, EventHandler<Log>> eventHandlerDic;




        //public bool register(CommandEnum command,<ContentEventArgs>)




        //public EventHandler<ContentEventArgs> LogReceive
        //{
        //    set
        //    {
        //        logReceive = value;
        //        eventHandlerDic.Add(CommandEnum.SendLog, null);
        //    }
        //    get
        //    {
        //        return logReceive;
        //    }
        //}
        //EventArgs<Service>
        public Client(IMessageHandler responser)
        {
           ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            client = new TcpClient();
            this.messageHandler = responser;

            //eventHandlerDic = new Dictionary<int, EventHandler<EventArgs>>()
            //{
            //    { (int)CommandEnum.SendLog,logReceive }

            //};

           

        }
        public void Start()
        {

            client.Connect(ep);
            Debug.WriteLine("You are connected");

            stream = client.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }
        private object lockThis = new object();
        public void Recieve()
        {
            new Task(() =>
            {
                while (true)
                {
                    try
                    {
                            string responseData = reader.ReadString();
                            messageHandler.Handle(responseData);
                    }
                    catch(Exception)
                    {
                        Debug.WriteLine("Deserialize fails");
                    }
                }

            }).Start();
        }
        public void Send(string requestString)
        {
            new Task(() =>
            {
                    try
                    {
                         writer.Write(requestString);
                }
                catch (Exception)
                    {
                        Debug.WriteLine("Deserialize fails");

                    }
            }).Start();
        }

    }
}
