namespace UserAuth.Exceptions;

public class OperationNotAllowedException : Exception
{
    public OperationNotAllowedException()
        : base("Operation not allowed")
    {
    }
    
    public OperationNotAllowedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}