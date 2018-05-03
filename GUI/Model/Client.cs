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
        public Client()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
             client = new TcpClient();
            client.Connect(ep);
            Debug.WriteLine("You are connected");
           
        }
        public void SendRequest()
        {
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // Send data to server
                Debug.Write("Please enter a number: ");
                //  int num = int.Parse(Console.ReadLine());
                int num = 9;
                writer.Write(num);
                // Get result from server
                int result = reader.ReadInt32();
                Debug.WriteLine("Result = {0}", result);
            }
            client.Close();
        }
    }
}
