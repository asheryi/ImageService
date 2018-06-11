using ImageService.Comunication;
using ImageService.Model;
using SharedResources.Commands;
using SharedResources.Communication;
namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModel m_Model;

        public NewFileCommand(IImageServiceModel Model)
        {
            m_Model = Model;            // Storing the Model
        }

        public string Execute(string[] args, out bool result)
        {

            string fullPath = args[0];
            // The String Will Return the New Path if result = true, 
            // and will return the error message if false .

            string sucecced=m_Model.AddFile(fullPath,out result);
            string[] paths = sucecced.Split(';');
            SingletonServer singletonServer = SingletonServer.Instance;
            CommunicationMessageGenerator CommunicationMessageGenerator = new CommunicationMessageGenerator();
            singletonServer.SendToAll(CommunicationMessageGenerator.Generate(CommandEnum.NewFileCommand, paths[1]));
            return paths[0];
        }
    }
}
