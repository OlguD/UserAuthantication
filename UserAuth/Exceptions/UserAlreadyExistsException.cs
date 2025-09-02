namespace UserAuth.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException()
        : base("User already exists")
    {
    }
    
    public UserAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}