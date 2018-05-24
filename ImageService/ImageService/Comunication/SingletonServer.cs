using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SharedResources.Communication;
using System.IO;
using System.Windows.Data;

namespace ImageService.Comunication
{
    class SingletonServer
    {

        private object lockThis = new object();
        private  EventHandler<IPEndPoint> clientConnected;
        public int Port { get; set; }
        private TcpListener listener;
        public IClientHandler CH {
            get;
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
        public EventHandler<IPEndPoint> ClientConnected
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
                    catch (Exception e)
                    {
                        clients.Remove(client);
                    }

                }
            }).Start();

        }
        public void SendToClient(string replyString, IPEndPoint ip)
        {
            new Task(() =>
            {
                lock (lockThis)
                {
                    TcpClient desired_client = null;
                    try {
                        foreach(TcpClient client in clients)
                        {
                            IPEndPoint ip_client = (IPEndPoint)client.Client.RemoteEndPoint;
                            if (ip_client.ToString() == ip.ToString())
                            {
                                desired_client = client;
                                break;
                            }
                        }

                        if(desired_client == null)
                        {
                            return;
                        }
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
                        clientConnected?.Invoke(this, pass);
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
