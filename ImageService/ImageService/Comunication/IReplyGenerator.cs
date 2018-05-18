using SharedResources.Commands;

namespace ImageService.Comunication
{
    public interface IReplyGenerator
    {
        string Generate(CommandEnum c, object o);        
    }
}
