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
        private IResponseHandler responser;
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
        public Client(IResponseHandler responser)
        {
           ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            client = new TcpClient();
            this.responser = responser;

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

        public void SendRequest()
        {
            
            
                // Send data to server
                //  int num = int.Parse(Console.ReadLine());
                 string  result = "1:";
                byte[] byData = System.Text.Encoding.UTF8.GetBytes(result);
                 //writer.WriteLine(byData);
                 stream.Write(byData,0,byData.Length);
                // Get result from server


                Byte[] data = new Byte[256];
                String responseData = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
                ServiceReply reply = JsonConvert.DeserializeObject<ServiceReply>(responseData);
                Log log = JsonConvert.DeserializeObject<Log>(reply.Content);
                  Debug.WriteLine("Result = {0}", log.Message);

          

           // client.Close();
        }

        public void Recieve()
        {
            new Task(() =>
            {
                while (true)
                {
                   
                    try
                    {
                        string responseData = reader.ReadString();

                        // Log log = ObjectConverter<Log>.Deserialize(reply.Content);
                        ///Log log = ObjectConverter<Log>.Deserialize(responseData);


                        responser.Handle(responseData);
                        
                    }
                    catch(Exception)
                    {
                        Debug.WriteLine("Deserialize fails");

                    }
                   
                }

            }).Start();
        }
    }
}
