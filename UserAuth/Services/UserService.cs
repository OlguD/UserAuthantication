using UserAuth.Models;
using UserAuth.Exceptions;
namespace UserAuth.Services;

public class UserService : IUserService
{
    private readonly List<User> _users = new();
    private int _id = 1;

    public IEnumerable<User> GetAllUsers()
    {
        return _users;
    }

    public void ChangeUserRole(User userWhoIsChangingRole, string userWhoseRoleIsChanging)
    {
        if (userWhoIsChangingRole.Role == "Admin")
        {
            try
            {
                var user = GetUserByUsername(userWhoseRoleIsChanging);
                if (user != null)
                {
                    user.Role = "Admin";
                }
                else
                {
                    throw new UserNotFoundException(userWhoseRoleIsChanging);
                }

            }
            catch (UserNotFoundException)
            {
                throw new UserNotFoundException(userWhoseRoleIsChanging);
            }
        }
        else
        {
            throw new OperationNotAllowedException();
        }
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

    public User ChangePassword(User user, string newPassword)
    {
        if (_users.Contains(user))
        {
            if (newPassword == user.Password)
            {
                throw new InvalidOperationException("Passwords must be different.");
            }
            user.Password = newPassword;
            user.UpdatedAt = DateTime.Now;
            return user;       
        }

        throw new UserNotFoundException(user.Username);
    }
    
    public User ChangeEmail(User user, string newEmail)
    {
        if (_users.Contains(user))
        {
            if (newEmail == user.Email)
            {
                throw new InvalidOperationException("Email must be different.");
            }
            user.Email = newEmail;
            user.UpdatedAt = DateTime.Now;
            return user;       
        }
        throw new UserNotFoundException(user.Username);
    }
}