using System;

namespace ShaeredResources.Comunication
{
    [Serializable]
    public  class TcpClientID:ClientID
    {
       
        public TcpClientID(string[] clientEndPoint):base(clientEndPoint) {
            
        }
        /// <summary>
        /// Compares two clientIds
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns>the comparision result</returns>
        public bool compare(ClientID clientID)
        {
           
            if (clientID.getArgs()[0] == this.getArgs()[0])
            return true;
            return false;
        }
       
      
       
    }
}
