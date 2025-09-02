using UserAuth.Models;
namespace UserAuth.Services;

public class UserService : IUserService
{
    private readonly List<User> _users = new();
    private int _id = 1;

    public IEnumerable<User> GetAllUsers()
    {
        return _users;
    }
    
    public User? GetUserByUsername(string username)
    {
        return _users.FirstOrDefault(u => u.Username == username);
    }
    
    public User Add(User user)
    {
        user.Id = _id++;
        _users.Add(user);
        return user;
    }
    
    public User? Validate(string username, string password)
    {
        return _users.FirstOrDefault(u =>
            u.Username.Trim().Equals(username.Trim(), StringComparison.OrdinalIgnoreCase) &&
            u.Password == password
        );
    }
}