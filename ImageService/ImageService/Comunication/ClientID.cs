using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ImageService.Comunication
{
    
    public  class  ClientID
    {
        string[] clientArgs;
        public ClientID(string[] clientArgs)
        {
            this.clientArgs = new string[clientArgs.Length];
            clientArgs.CopyTo(this.clientArgs, 0);
           
        }
      public string [] getArgs()
        {
            return clientArgs;
        }

    }
}
