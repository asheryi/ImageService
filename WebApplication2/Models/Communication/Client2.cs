using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SharedResources.Communication;
using SharedResources;
using SharedResources.Commands;
using WebApplication2.Models.Logging;

namespace WebApplication2.Model.Communication
{
    /// <summary>
    /// Responsible of communication on the GUI side.
    /// </summary>
   public class Client2
    {
       
        private TcpClient client;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer ;
        private IMessageHandler messageHandler;//handling received messages.
        private IPEndPoint ep;
        private CommunicationMessageGenerator messageGenerator;
        private Settings settings;
        /// <summary>
        /// Client's constructor
        /// </summary>
        /// <param name="responser">handling received messages.</param>
        public Client2(IMessageHandler responser)
        {
            messageGenerator = new CommunicationMessageGenerator();
            ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
           client = new TcpClient();
           this.messageHandler = responser;
        }
        public Client2()
        {
            messageGenerator = new CommunicationMessageGenerator();
            ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            client = new TcpClient();
            Start();
            recieveSettings();
            Debug.WriteLine("Client Created");


        }
        public Settings getSettings()
        {
            return settings;
        }
        public void recieveSettings()
        {
            Debug.WriteLine("HERE 1");

            //logs.Clear();
            string requestString = messageGenerator.Generate(CommandEnum.GetAllLogsCommand, "");
            writer.Write(requestString);
            Debug.WriteLine("HERE 2");
            CommunicationMessage reply = null;
            ContentEventArgs settings_ = null;
            CommandEnum command = CommandEnum.SendLog;
            while (command != CommandEnum.GetConfigCommand)
            {
                string raw_data = reader.ReadString();
                Debug.WriteLine("HERE 3");

                reply = ObjectConverter.Deserialize<CommunicationMessage>(raw_data);//<CommunicationMessage>
                settings_ = new ContentEventArgs(reply.Content);
                command = reply.CommandID;
            }
            settings = settings_.GetContent<Settings>();
           
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

        public void askRemoveHandler(DirectoryDetails handler)
        {
            Debug.WriteLine("first ask Remove");

            string requestString = messageGenerator.Generate(CommandEnum.CloseHandlerCommand, handler);
            Debug.WriteLine("requestSTRING: "+requestString);

            writer.Write(requestString);
            CommunicationMessage reply = null;
            ContentEventArgs list = null;
            CommandEnum command = CommandEnum.SendLog;
            while (command != CommandEnum.CloseHandlerCommand)
            {
                string raw_data = reader.ReadString();
                Debug.WriteLine("command " + raw_data);
                // Debug.WriteLine("HERE 3");

                reply = ObjectConverter.Deserialize<CommunicationMessage>(raw_data);//<CommunicationMessage>
                list = new ContentEventArgs(reply.Content);
                command = reply.CommandID;
                Debug.WriteLine("command "+command.ToString());

            }
            Debug.WriteLine("ask Remove");

            // Debug.WriteLine("THE COMMAND IS " + reply.CommandID.ToString());

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
        public void getAllLogs(ICollection<Log> logs)
        {
            Debug.WriteLine("HERE 1");

            //logs.Clear();
                    string requestString = messageGenerator.Generate(CommandEnum.GetAllLogsCommand, "");
                    writer.Write(requestString);
            Debug.WriteLine("HERE 2");
            CommunicationMessage reply=null;
            ContentEventArgs list=null;
            CommandEnum command = CommandEnum.SendLog;
            while (command != CommandEnum.GetAllLogsCommand)
            {
                string raw_data = reader.ReadString();
                Debug.WriteLine("HERE 3");

                 reply = SharedResources.ObjectConverter.Deserialize<CommunicationMessage>(raw_data);//<CommunicationMessage>
                 list = new ContentEventArgs(reply.Content);
                command = reply.CommandID;
            }
            Debug.WriteLine("THE COMMAND IS " + reply.CommandID.ToString());
                if (reply.CommandID == CommandEnum.GetAllLogsCommand)
                {
                    ICollection<Log> logs_ = list.GetContent<ICollection<Log>>();
                    foreach (Log log in logs_)
                    {
                        logs.Add(log);
                    }
                }
            
        }

    }
}
