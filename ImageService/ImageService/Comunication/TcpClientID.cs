using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Comunication
{
    [Serializable]
    public  class TcpClientID:ClientID
    {
       
        public TcpClientID(string[] clientEndPoint):base(clientEndPoint) {
            
        }
        public bool compare(ClientID clientID)
        {
           
            if (clientID.getArgs()[0] == this.getArgs()[0])
            return true;
            return false;
        }
       
      
       
    }
}
