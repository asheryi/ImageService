using SharedResources.Logging;
using SharedResources.Communication;
using SharedResources.Commands;


using ImageService.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharedResources;

namespace ImageService.Comunication
{
    class ClientHandler:IClientHandler
    {
       
        public void HandleClient(TcpClient client)
        { }
        public void HandleClient(TcpClient client, ILoggingService logger)
        {
            new Task(() =>
            {
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);
               

                //logger.Log("Got command: {0}", Logging.Model.MessageTypeEnum.INFO);
                //string result = ExecuteCommand(commandLine, client);
                //writer.Write(commandType * commandType);



              //  ServiceReply reply = new ServiceReply(CommandEnum.GetAllLogsCommand, JsonConvert.SerializeObject(log));

              //  string result = ObjectConverter<ServiceReply>.Serialize(reply);
               // writer.Write(result);

                //while (true)
                //{

                //    Log log = new Log(MessageTypeEnum.INFO, "Hey you All");

                //    logger.Log(log.Message, log.Type);


                //    String responseData = reader.ReadString();

                //    logger.Log(log.Message + "AFTER READ",log.Type);

                //    // byte[] byData = stream.ReadByte();

                //    // string commandType = reader.ReadLine();
                //    logger.Log("the command is " + responseData, MessageTypeEnum.INFO);

                //    //logger.Log("Got command: {0}", Logging.Model.MessageTypeEnum.INFO);
                //    //string result = ExecuteCommand(commandLine, client);
                //    //writer.Write(commandType * commandType);



                //    ServiceReply reply = new ServiceReply(CommandEnum.GetAllLogsCommand, JsonConvert.SerializeObject(log));

                //    string result = ObjectConverter<ServiceReply>.Serialize(reply);
                //    writer.Write(result);

                //}
                reader.Close();
                writer.Close();
                //client.Close();
            }).Start();
        }
       
    }
}
