using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SharedResources.Communication;
using System.IO;
using System.Windows.Data;
using System.Linq;

namespace ImageService.Comunication
{
    class SingletonServer
    {

        private object lockThis = new object();

        private EventHandler<ClientID> clientConnected;
        public int Port { get; set; }
        private TcpListener listener;
        public IClientHandler CH { get;
            set; }


        private ICollection<TcpClient> clients;

        private static SingletonServer singleServer;

        
        public IMessageHandler MessageHandler { get; set; }
        private SingletonServer()
        {
            clients = new List<TcpClient>();
            BindingOperations.EnableCollectionSynchronization(clients, clients);
            this.Port = 8000;
            this.CH = new ClientHandler();


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
      
        public void SendToAll(string replyString)
        {
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
                            clients.Remove(client);
                        }
                    }
                
            }).Start();

        }
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
                    }catch(Exception e)
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
            Task task = new Task(() => {
            while (true)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    IPEndPoint pass = (IPEndPoint)(client.Client.RemoteEndPoint);
                        clients.Add(client);
                        recieveRequests(client);
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
           
                listener.Stop();

            
        }        public void recieveRequests(TcpClient client)
        {
            Task task = new Task(() =>
              {
                  while (true)
                  {

                      BinaryReader reader = new BinaryReader(client.GetStream());
                      string request = reader.ReadString();
                      MessageHandler.Handle(request);
                   
                  }
              });
            task.Start();
        }
    }
}
