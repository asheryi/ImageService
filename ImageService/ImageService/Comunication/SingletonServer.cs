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
using ImageService.Controller;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageService.Comunication
{
    class SingletonServer
    {
        // Locks the Tcp COnnections
        private object lockThis = new object();
        EventHandler<Bitmap> imageTransffer;
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
        public EventHandler<Bitmap> BitmapTransfer
        {
            get
            {
                return imageTransffer;
            }
            set
            {
                imageTransffer = value;
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
                        if (found.Length == 0)
                        {
                            return;
                        }

                        desired_client = found[0];
                        BinaryWriter writer = new BinaryWriter(desired_client.GetStream());
                        writer.Write(replyString);
                    } catch (Exception)
                    {

                    }
                }

            }).Start();

        }


        public void Start()
        {
            IPEndPoint ep = new
                             // IPEndPoint(IPAddress.Any, Port); 
                             //  IPEndPoint(IPAddress.Any, 45632);
                             IPEndPoint(IPAddress.Any, 45632);

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
            listener.Stop();
        }
        /// <summary>
        /// Recieves data from the specific client
        /// </summary>
        /// <param name="client"></param>        public void recieveRequests2(TcpClient client)
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
        }        public void recieveRequests(TcpClient client)
        {
            Task task = new Task(() =>
            {
               
                BinaryWriter bw = new BinaryWriter(client.GetStream());
                BinaryReader br = new BinaryReader(client.GetStream());

                while (true)
                {

                    byte[] imgName=new byte[getDataSize(br)];
                    fillArraysFromConnection(imgName, false,br, bw);
                    byte[] imgBytes = new byte[getDataSize(br)]; ;
                    fillArraysFromConnection(imgBytes,true,br, bw);
                   
                    try
                    {
                        

                        System.Drawing.Bitmap img =new System.Drawing.Bitmap(new MemoryStream(imgBytes));

                        string imgNameString = Encoding.Default.GetString(imgName);
                        Bitmap bitmap = new Bitmap(img, imgNameString);

                        imageTransffer?.Invoke(this, bitmap);

                    }
                    catch (Exception e)
                    {
                        
                    }
                    bw.Write(true);
                    byte[] hasMore=br.ReadBytes(1);
                    if(hasMore[0] == 0)
                    {
                        break;
                    }
                }
                bw.Close();
                br.Close();
                client.Close();

            }
            
            );
            task.Start();
        }

        private int getDataSize(BinaryReader br)
        {
           
            Byte[] sizeInBytes = br.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(sizeInBytes);
           return BitConverter.ToInt32(sizeInBytes, 0);
        }
        private void fillArraysFromConnection(byte [] data,bool img, BinaryReader br, BinaryWriter bw)
        {
          
            int dataSize = data.Length;
            if (img)
            {
                int count = Math.Min(1024, dataSize);
                int offset = 0;
                while (count > 0)
                {

                    br.Read(data, offset, count);
                   
                    offset += count;
                    count = Math.Min(dataSize - offset, count);
                    bw.Write(true);

                }

            }
            else
            {
                br.ReadBytes(dataSize).CopyTo(data,0);
            }
            
        }

    }

}
