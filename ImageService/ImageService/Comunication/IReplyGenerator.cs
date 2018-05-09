using SharedResources.Commands;

namespace ImageService.Comunication
{
    interface IReplyGenerator
    {
        string Generate(CommandEnum c, object o);        
    }
}
