using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Comunication
{
    class ClientHandler:IClientHandler
    {
        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (BinaryReader reader = new BinaryReader(stream))
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    int commandLine = reader.ReadInt32();
                    Console.WriteLine("Got command: {0}", commandLine);
                    //string result = ExecuteCommand(commandLine, client);
                    writer.Write(commandLine * commandLine);
                    //writer.Write(result);
                }
                client.Close();
            }).Start();
        }
    }
}
