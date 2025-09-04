namespace UserAuth.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException()
        : base($"Invalid password operation")
    {
    }

    public InvalidPasswordException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}