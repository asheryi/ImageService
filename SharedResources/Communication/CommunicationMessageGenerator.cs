﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedResources.Commands;
using SharedResources;
using SharedResources.Communication;

namespace SharedResources.Communication
{
    public class CommunicationMessageGenerator : IMessageGenerator
    {
        public string Generate(CommandEnum c, object o)
        {
            string data = ObjectConverter.Serialize(o);//object
            CommunicationMessage sr = new CommunicationMessage(c, data);
            return ObjectConverter.Serialize(sr);//object
        }
    }
}