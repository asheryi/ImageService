using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedResources.Commands;
using SharedResources;
using SharedResources.Communication;

namespace ImageService.Comunication
{
    public class ServiceReplyGenerator : IReplyGenerator
    {
        public string Generate(CommandEnum c, object o)
        {
            string data = ObjectConverter.Serialize(o);//object
            ServiceReply sr = new ServiceReply(CommandEnum.GetAllLogsCommand, data);
            return ObjectConverter.Serialize(sr);//object
        }
    }
}
