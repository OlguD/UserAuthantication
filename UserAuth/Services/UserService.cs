using SQLitePCL;
using UserAuth.Models;
using UserAuth.Exceptions;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;
namespace UserAuth.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> ChangeUserRole(string username, string targetUsername, string newRole)
    {
        var userChanging = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (userChanging == null)
        {
            throw new UserNotFoundException(username);
        }

        if (userChanging.Role != "Admin")
        {
            throw new OperationNotAllowedException();
        }

        var target = await _context.Users.FirstOrDefaultAsync(u => u.Username == targetUsername);
        if (target == null)
        {
            throw new UserNotFoundException(targetUsername);
        }

        target.Role = newRole;
        await _context.SaveChangesAsync();
        return target;
    }
    
    public async Task<User> GetByUsernameAsync(string username)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            throw new UserNotFoundException(username);
        }
        return user;
    }
    
    public async Task<User> AddAsync(User user)
    {
        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
        {
            throw new UserAlreadyExistsException();
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    public User? Validate(string username, string password)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password).Result;
    }

    public async Task<User> ChangePasswordAsync(string username, string oldPassword, string newPassword)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            throw new UserNotFoundException(username);
        }

        if (user.Password != oldPassword)
        {
            throw new InvalidPasswordException();
        }

        if (user.Password == newPassword)
        {
            throw new InvalidPasswordException();
        }
        user.Password = newPassword;
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> ChangeEmailAsync(string username, string newEmail)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            throw new UserNotFoundException(username);
        }
        
        if (await _context.Users.AnyAsync(u => u.Email == newEmail && u.Id != user.Id))
        {
            throw new EmailAlreadyInUseException(newEmail);
        }

        // Check if email is the same as current email
        if (user.Email == newEmail)
        {
            throw new EmailAlreadyInUseException(newEmail);
        }

        user.Email = newEmail;
        await _context.SaveChangesAsync();
        return user;
    }
    
    public async Task DeleteAsync(User adminUser, string username)
    {
        if (adminUser.Role == "Admin")
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new OperationNotAllowedException();
        }
    }
}