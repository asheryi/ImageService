using SharedResources.Commands;
using System;

namespace SharedResources.Communication
{
    public interface IMessageHandler
    {
        
        bool RegisterFuncToEvent(CommandEnum c, EventHandler<ContentEventArgs> func);

        bool Handle(string raw_data);



    }
}
