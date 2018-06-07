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
    /// <summary>
    /// Responsible of communication on the GUI side.
    /// </summary>
    class Client
    {
       
        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer ;
        private IMessageHandler messageHandler;//handling received messages.
        private IPEndPoint ep;
        /// <summary>
        /// Client's constructor
        /// </summary>
        /// <param name="responser">handling received messages.</param>
        public Client(IMessageHandler responser)
        {
           ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
           client = new TcpClient();
           this.messageHandler = responser;
        }
        /// <summary>
        /// statint the communiction.
        /// </summary>
        public void Start()
        {

            client.Connect(ep);
            Debug.WriteLine("You are connected");

            stream = client.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }
        /// <summary>
        /// recieves message from server.
        /// </summary>
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
            new Task(() =>
            {
                    try
                    {
                         writer.Write(requestString);
                }
                catch (Exception)
                    {

                }
            }).Start();
        }

    }
}
