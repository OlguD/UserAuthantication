namespace UserAuth.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException(string password)
        : base($"Invalid password: '{password}'")
    {
    }

    public InvalidPasswordException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}