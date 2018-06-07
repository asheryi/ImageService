using SharedResources.Commands;

namespace SharedResources.Communication
{
    public interface IMessageGenerator
    {
        /// <summary>
        /// Generate aaccording to specific protocol the message to send through
        /// the communication
        /// </summary>
        /// <param name="c">the command</param>
        /// <param name="o">the object to send</param>
        /// <returns></returns>
        string Generate(CommandEnum c, object o);        
    }
}
