using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SharedResources.Communication;
using System.IO;
using System.Windows.Data;
using System.Linq;
using ShaeredResources.Comunication;

namespace ImageService.Comunication
{
    class SingletonServer
    {
        // Locks the Tcp COnnections
        private object lockThis = new object();

        private EventHandler<ClientID> clientConnected; // invokes when client is connected to get all logs etc..
        public int Port { get; set; }
        private TcpListener listener;

        private ICollection<TcpClient> clients;

        private static SingletonServer singleServer; // This is singleton

        public IMessageHandler MessageHandler { get; set; } // Handle the messages through the communication
        private SingletonServer()
        {
            clients = new List<TcpClient>();
            BindingOperations.EnableCollectionSynchronization(clients, clients);
            Port = 8000;
        }

        public static SingletonServer Instance
        {
            get
            {
                if (singleServer == null)
                {
                    singleServer = new SingletonServer();
                    
                }
                return singleServer;
            }
        }
        public EventHandler<ClientID> ClientConnected
        {
            get
            {
                return clientConnected;
            }
            set
            {
                clientConnected = value;
            }
        }
      
        /// <summary>
        /// Send the given string (the data) through the connection to all connected clients.
        /// </summary>
        /// <param name="replyString">The data to send to all</param>
        public void SendToAll(string replyString)
        {
            // On new task ..
            new Task(() =>
            {   
                    
                    foreach (TcpClient client in new List<TcpClient>(clients))
                    {
                        try
                        {
                            lock (lockThis)
                            {
                                BinaryWriter writer = new BinaryWriter(client.GetStream());
                                writer.Write(replyString);
                            }
                        }
                        catch (Exception)
                        {
                            // If occured a problem , remoes it from the connectes clients as requested.
                            clients.Remove(client);
                        }
                    }
                
            }).Start();

        }
        /// <summary>
        /// Send to specific client
        /// </summary>
        /// <param name="replyString">The data to send</param>
        /// <param name="clientID">Client details</param>
        public void SendToClient(string replyString, ClientID clientID)
        {
            new Task(() =>
            {
                lock (lockThis)
                {
                    TcpClient desired_client = null;
                    TcpClientID tcpclientID = new TcpClientID(clientID.getArgs());
                    try
                    {
                        TcpClient[] found = clients.Where(c => tcpclientID.compare(new TcpClientID(new string[] { c.Client.RemoteEndPoint.ToString() }))).ToArray();
                        if(found.Length == 0)
                        {
                            return;
                        }

                        desired_client = found[0];
                        BinaryWriter writer = new BinaryWriter(desired_client.GetStream());
                        writer.Write(replyString);
                    }catch(Exception)
                    {

                    }
                }

            }).Start();

        }
      
      
        public void Start()
        {
            IPEndPoint ep = new
            IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);
            listener = new TcpListener(ep);
            listener.Start();
            // STARTS RECIEVING NEW CLIENTS IN NEW TASK
            Task task = new Task(() => {
            while (true)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    IPEndPoint pass = (IPEndPoint)(client.Client.RemoteEndPoint);
                        clients.Add(client);
                        recieveRequests(client);
                        // invokes client connected
                        clientConnected?.Invoke(this, new TcpClientID(new string[] { pass.ToString() }));
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
            });
            task.Start();
        }
        public void Stop()
        {
            foreach (TcpClient client in clients)
                client.Close();
                listener.Stop();
        }        /// <summary>
        /// Recieves data from the specific client
        /// </summary>
        /// <param name="client"></param>        public void recieveRequests(TcpClient client)
        {
            Task task = new Task(() =>
              {
                  while (true)
                  {

                      BinaryReader reader = new BinaryReader(client.GetStream());
                      string request = reader.ReadString();

                  
                      IPEndPoint pass = (IPEndPoint)(client.Client.RemoteEndPoint);
                      MessageHandler.Handle(request, new TcpClientID(new string[] { pass.ToString() }));
                   
                  }
              });
            task.Start();
        }
    }
}
