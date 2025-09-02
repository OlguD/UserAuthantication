namespace UserAuth.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string username)
        : base($"User '{username}' not found.")
    {
    }

    public UserNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}