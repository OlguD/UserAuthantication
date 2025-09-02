using Xunit;
using UserAuth.Services;
using UserAuth.Models;
using UserAuth.Exceptions;
namespace UserAuth.Tests;

public class ExceptionTests : IDisposable
{
    private readonly IUserService _userService;
    private readonly User _user;
    private readonly User _admin;
    private readonly User _fakeUser;
    
    public ExceptionTests()
    {
        _userService = new UserService();
        _user = new User
        {
            Username = "test_username",
            Password = "test_password",
            Email = "test_email",
            Role = "User",
            Name = "test_name",
            Surname = "test_surname"
        };
        _admin = new User
        {
            Username = "test_admin_username",
            Password = "test_admin_password",
            Email = "test_admin_email",
            Role = "Admin",
            Name = "test_admin_name",
            Surname = "test_admin_surname"
        };

        _userService.Add(_user);
        _userService.Add(_admin);

        _fakeUser = (new User
        {
            Username = "fake_username",
            Password = "fake_password",
            Email = "fake_email",
            Role = "User",
            Name = "fake_name",
            Surname = "fake_surname"
        });
    }

    public void Dispose()
    {
        // Cleanup if needed
    }

    [Fact]
    public void ChangePassword_UserNotFound_ShouldThrow()
    {
        Assert.Throws<UserNotFoundException>(() =>
            _userService.ChangePassword(_fakeUser, "new_password"));
    }
    
    [Fact]
    public void ChangePassword_SamePassword_ShouldThrow()
    {
        Assert.Throws<InvalidOperationException>(() =>
            _userService.ChangePassword(_user, _user.Password));
    }
    
    [Fact]
    public void ChangeEmail_UserNotFound_ShouldThrow()
    {
        Assert.Throws<UserNotFoundException>(() =>
            _userService.ChangeEmail(_fakeUser, "new_email@example.com"));
    }

    [Fact]
    public void ChangeEmail_SameEmail_ShouldThrow()
    {
        Assert.Throws<InvalidOperationException>(() =>
            _userService.ChangeEmail(_user, _user.Email));
    }

    [Fact]
    public void ChangeUserRole_NotAdmin_ShouldThrow()
    {
        Assert.Throws<OperationNotAllowedException>(() =>
            _userService.ChangeUserRole(_user, _admin.Username));
    }

    [Fact]
    public void ChangeUserRole_UserNotFound_ShouldThrow()
    {
        Assert.Throws<UserNotFoundException>(() =>
            _userService.ChangeUserRole(_admin, "non_existing_username"));
    }
}