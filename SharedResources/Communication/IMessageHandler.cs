using SharedResources.Commands;
using System;

namespace SharedResources.Communication
{
    public interface IMessageHandler
    {
        /// <summary>
        /// Refister the func given to the command c
        /// </summary>
        /// <param name="c">specific command to register to</param>
        /// <param name="func">func to register</param>
        /// <returns></returns>
        bool RegisterFuncToEvent(CommandEnum c, EventHandler<ContentEventArgs> func);

        /// <summary>
        /// according to the raw_data , according to the specific protocol handles
        /// it .
        /// </summary>
        /// <param name="raw_data"></param>
        /// <returns></returns>
        bool Handle(string raw_data);



    }
}
