using ShaeredResources.Comunication;

namespace SharedResources
{
    // Could contain more properties in the future , other than a string .
    public class ContentEventArgs
    {
        public ClientID ClientID { get; set; }
        public string Content { get; private set; }

        public ContentEventArgs(string content)
        {
            Content = content;
        }
        public T GetContent<T>()
        {
            return ObjectConverter.Deserialize<T>(Content);
        }
    }
}
