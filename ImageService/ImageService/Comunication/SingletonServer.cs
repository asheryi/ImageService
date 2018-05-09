using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SharedResources.Communication;
using System.IO;
using SharedResources;
using System.Collections;
using SharedResources.Logging;

namespace ImageService.Comunication
{
    class SingletonServer
    {

        private object lockThis = new object();
        private  EventHandler<int> clientConnected;
        public int Port { get; set; }
        private TcpListener listener;
        public IClientHandler CH {
            get;
            set; }

        private ICollection<TcpClient> clients;

        private static SingletonServer singleServer;

        private SingletonServer()
        {
            clients = new List<TcpClient>();
            
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
        public EventHandler<int> ClientConnected
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
        public void SendToAll(ServiceReply serviceReply)
        {
            new Task(() =>
            {
                foreach (TcpClient client in new List<TcpClient>(clients))
                {
                    try
                    {
                        lock (lockThis)
                        {
                            NetworkStream stream = client.GetStream();
                            BinaryWriter writer = new BinaryWriter(stream);
                            writer.Write((ObjectConverter.Serialize(serviceReply)));//serviceReply
                            //writer.WriteLine(byData);
                            //writer.BaseStream.Position = 0;

                            //writer.Close();
                        }
                    } catch(Exception e)
                    {
                        clients.Remove(client);
                    }

                }



                //client.Close();
            }).Start();

        }
        public void SendToAll(string log)
        {
            new Task(() =>
            {
                foreach (TcpClient client in new List<TcpClient>(clients))
                {
                    try
                    {
                        lock (lockThis)
                        {
                            NetworkStream stream = client.GetStream();
                            BinaryWriter writer = new BinaryWriter(stream);
                            writer.Write(log);
                            //writer.WriteLine(byData);
                            //writer.BaseStream.Position = 0;

                            //writer.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        clients.Remove(client);
                    }

                }



                //client.Close();
            }).Start();

        }

        public void SendToClient(string replyString, int clientID)
        {
            
            new Task(() =>
            {
                lock (lockThis)
                {
                    //string replyString = (ObjectConverter<ServiceReply>.Serialize(serviceReply));
                    TcpClient t = ((List<TcpClient>)clients)[clientID];
                    BinaryWriter writer = new BinaryWriter(t.GetStream());

                    writer.Write(replyString);
                }

                //client.Close();
            }).Start();

        }
        public void SendToClient(string log, int clientID, EventLog eventlog)
        {
           
            new Task(() =>
            {

               // string replyString = (ObjectConverter<ServiceReply>.Serialize(serviceReply));
                //eventlog.WriteEntry("%%%%%%%%%%" + replyString);
                TcpClient t = ((List<TcpClient>)clients)[clientID];
                lock (lockThis)
                {
                    BinaryWriter writer = new BinaryWriter(t.GetStream());

                    writer.Write(log);
                }

                //client.Close();
            }).Start();

        }

        //public void SendToClient(ServiceReply serviceReply, NetworkStream stream)
        //{
        //    new Task(() =>
        //    {

        //        BinaryWriter writer = new BinaryWriter(stream);
        //        writer.Write((ObjectConverter<ServiceReply>.Serialize(serviceReply)));

        //        //client.Close();
        //    }).Start();

        //}
        public void Start()
        {
            IPEndPoint ep = new
            IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);
            listener = new TcpListener(ep);

            listener.Start();
            Debug.WriteLine("Waiting for connections...");
           
            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        clients.Add(client);
                        Debug.WriteLine("Got new connection");
                        //                logger.Log("Got new connection", MessageTypeEnum.INFO);
                        clientConnected?.Invoke(this, ((List<TcpClient>)clients).IndexOf(client));
                       // clientConnected?.Invoke(this,ObjectConverter<NetworkStream>.Serialize(client.GetStream()));
                        //  CH.HandleClient(client);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
               
                Debug.WriteLine("Server stopped");
            });
            task.Start();
        }
        public void Stop()
        {
            listener.Stop();
        }
    }
}
