using SharedResources;
using SharedResources.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    interface IResponseHandler
    {
        
        bool RegisterFuncToEvent(CommandEnum c, EventHandler<ContentEventArgs> func);

        bool Handle(string raw_data);



    }
}
