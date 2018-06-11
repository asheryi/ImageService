using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SharedResources.Communication
{
    /// <summary>
    /// Responsible of communication on the GUI side.
    /// </summary>
    public class Client
    {
       
        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer ;
        private IPEndPoint ep;

        public IMessageHandler messageHandler { get; set; } //handling received messages.
        public bool Connected
        { get; set; }

        /// <summary>
        /// Client's constructor
        /// </summary>
        /// <param name="responser">handling received messages.</param>
        public Client()
        {
            Connected = false;
           
            
        }
       
        /// <summary>
        /// statint the communiction.
        /// </summary>
        public bool Start()
        {
            try
            {
                ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
                client = new TcpClient();
                client.Connect(ep);
                
            }
            catch(Exception e)
            {
                Debug.WriteLine("Connect: "+e.Message);
                //client.GetStream().Close();
                client.Close();
                client = null;
                //client.EndConnect(null);
                //client.Client.Disconnect(true);

                Connected = false;
                return false;
            }

           
            
            stream = client.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            Connected = true;
            return true;

        }
        /// <summary>
        /// recieves message from server.
        /// </summary>
        public void Recieve()
        {
            new Task(() =>
            {
                while (Connected)
                {
                    try
                    {
                      
                            string responseData = reader.ReadString();
                            messageHandler.Handle(responseData);
                       
                           
                    }
                    catch(Exception)
                    {
                        Connected = false;
                        //client.GetStream().Close();
                        client.Close();
                        client = null;
                        break;
                    }
                }

            }).Start();
        }
        /// <summary>
        /// sending message to server.
        /// </summary>
        /// <param name="requestString">the message for the server</param>
        public void Send(string requestString)
        {
            if(!Connected)
            {
                return;
            }
            new Task(() =>
            {
                    try
                    {
                         writer.Write(requestString);
                }
                catch (Exception)
                    {
                    //client.GetStream().Close();
                    client.Close();
                    client = null;

                    //client.EndConnect(null);
                    Connected = false;
                }
            }).Start();
        }

    }
}
