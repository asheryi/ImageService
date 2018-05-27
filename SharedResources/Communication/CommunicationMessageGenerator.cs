using SharedResources.Commands;

namespace SharedResources.Communication
{
    // generator of messages according to the specific protocol :
    // transferring string of serialized command + object to transfer.
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
