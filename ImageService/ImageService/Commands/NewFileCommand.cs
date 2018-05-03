using ImageService.Model;

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

            return m_Model.AddFile(fullPath,out result);
        }
    }
}
