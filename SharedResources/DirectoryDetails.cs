namespace SharedResources
{
    // Stores the direcory details
    public class DirectoryDetails
    {
       public DirectoryDetails(string directoryName)
        {
            this.directoryName = directoryName;
        }
        private string directoryName;
        public string DirectoryName {
            get
            { return directoryName; }
            set
            {
                if(directoryName != value)
                {
                    directoryName = value;
                }
            }
        }
    }
}
