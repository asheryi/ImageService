namespace SharedResources.Commands
{
    public enum CommandEnum : int
    {
        NewFileCommand = 0,
        CloseHandlerCommand = 1, // to close specific handler.
        GetAllLogsCommand = 2,  // efficient (all logs).
        GetConfigCommand = 3,
        SendLog = 4 // Send one log .
    }
}
