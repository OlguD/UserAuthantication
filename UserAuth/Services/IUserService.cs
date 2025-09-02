using UserAuth.Models;
namespace UserAuth.Services;

public interface IUserService
{
    public IEnumerable<User> GetAllUsers();
    public User? GetUserByUsername(string username);
    public User? Add(User user);
    public User? Validate(string username, string password);

    public void ChangeUserRole(User userWhoIsChangingRole, string userWhoseRoleIsChanging);
    public User? ChangePassword(User user, string newPassword);
    public User? ChangeEmail(User user, string newEmail);
}