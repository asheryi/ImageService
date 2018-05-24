using SharedResources.Commands;

namespace SharedResources.Communication
{
    public interface IMessageGenerator
    {
        string Generate(CommandEnum c, object o);        
    }
}
