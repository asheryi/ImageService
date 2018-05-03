using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public class SingletonCommunication
    {
        private static SingletonCommunication singelC;

        private SingletonCommunication() {

        }

        public static SingletonCommunication Instance
        {
            get
            {
                if (singelC == null)
                {
                    singelC = new SingletonCommunication();
                }
                return singelC;
            }
        }
    }
}
