public class ClienteException : Exception
{
    public ClienteException(string message) : base(message) { }
    public ClienteException(string message, Exception ex) : base(message, ex) { }
}
