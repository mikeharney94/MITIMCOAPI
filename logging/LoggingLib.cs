public static class LoggingLib
{
    /*
        Allows us to handle errors
    */
    public static void ThrowException(string message, string? type)
    {
        Console.WriteLine("Error exception: "+message);
        
        if (!Global.IsTesting)
        {
            // Send email with error...
        }
        switch (type)
        {
            case "Argument":
                throw new ArgumentException(message);
            default:
                throw new Exception(message);
        }
    }
}