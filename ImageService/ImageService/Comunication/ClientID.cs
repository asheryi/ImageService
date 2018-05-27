namespace ImageService.Comunication
{
    //NOTE: If in the future we wont use TCPClient we should not change the controller.
    public  class  ClientID
    {
        string[] clientArgs;
        /// <summary>
        /// ClientID constructor.
        /// </summary>
        /// <param name="clientArgs">clientID args</param>
        public ClientID(string[] clientArgs)
        {
            this.clientArgs = new string[clientArgs.Length];
            clientArgs.CopyTo(this.clientArgs, 0);
           
        }
        /// <summary>
        /// Gets the Args
        /// </summary>
        /// <returns></returns>
      public string [] getArgs()
        {
            return clientArgs;
        }

    }
}
