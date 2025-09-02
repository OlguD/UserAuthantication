using UserAuth.Models;
namespace UserAuth.Services;

public interface IUserService
{
    public IEnumerable<User> GetAllUsers();
    public User? GetUserByUsername(string username);
    public User? Add(User user);
    public User? Validate(string username, string password);
}