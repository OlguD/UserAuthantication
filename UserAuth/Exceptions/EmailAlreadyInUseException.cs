namespace UserAuth.Exceptions;

public class EmailAlreadyInUseException : Exception
{
    public EmailAlreadyInUseException(string email)
        : base($"Email '{email}' is already in use.")
    {
    }

    public EmailAlreadyInUseException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}