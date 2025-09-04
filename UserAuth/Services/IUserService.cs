using UserAuth.Models;
namespace UserAuth.Services;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User> GetByUsernameAsync(string username);
    Task<User> AddAsync(User user);
    Task DeleteAsync(User adminUser, string username);
    User? Validate(string username, string password);

    Task<User> ChangeUserRole(string username, string targetUsername, string newRole);
    Task<User> ChangePasswordAsync(string username, string oldPassword, string newPassword);
    Task<User> ChangeEmailAsync(string username, string newEmail);
}