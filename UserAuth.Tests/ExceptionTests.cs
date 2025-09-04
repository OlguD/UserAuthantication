using Xunit;
using UserAuth.Services;
using UserAuth.Models;
using UserAuth.Exceptions;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;

namespace UserAuth.Tests;

public class ExceptionTests : IDisposable
{
    private readonly IUserService _userService;
    private readonly User _user;
    private readonly User _admin;
    private readonly User _fakeUser;
    private readonly AppDbContext _context;
    
    public ExceptionTests()
    {
        // Create in-memory database for testing
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserAuthDb_" + Guid.NewGuid().ToString())
            .Options;
        
        _context = new AppDbContext(options);
        _userService = new UserService(_context);
        
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

        _userService.AddAsync(_user).Wait();
        _userService.AddAsync(_admin).Wait();

        _fakeUser = new User
        {
            Username = "fake_username",
            Password = "fake_password",
            Email = "fake_email",
            Role = "User",
            Name = "fake_name",
            Surname = "fake_surname"
        };
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task ChangePassword_UserNotFound_ShouldThrow()
    {
        await Assert.ThrowsAsync<UserNotFoundException>(() =>
            _userService.ChangePasswordAsync("fake_username", "old_password", "new_password"));
    }
    
    [Fact]
    public async Task ChangePassword_InvalidPassword_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidPasswordException>(() =>
            _userService.ChangePasswordAsync(_user.Username, "wrong_password", "new_password"));
    }
    
    [Fact]
    public async Task ChangePassword_SamePassword_ShouldThrow()
    {
        await Assert.ThrowsAsync<InvalidPasswordException>(() =>
            _userService.ChangePasswordAsync(_user.Username, _user.Password, _user.Password));
    }
    
    [Fact]
    public async Task ChangeEmail_UserNotFound_ShouldThrow()
    {
        await Assert.ThrowsAsync<UserNotFoundException>(() =>
            _userService.ChangeEmailAsync("fake_username", "new_email@example.com"));
    }

    [Fact]
    public async Task ChangeEmail_SameEmail_ShouldThrow()
    {
        await Assert.ThrowsAsync<EmailAlreadyInUseException>(() =>
            _userService.ChangeEmailAsync(_user.Username, _user.Email));
    }

    [Fact]
    public async Task ChangeUserRole_NotAdmin_ShouldThrow()
    {
        await Assert.ThrowsAsync<OperationNotAllowedException>(() =>
            _userService.ChangeUserRole(_user.Username, _admin.Username, "User"));
    }

    [Fact]
    public async Task ChangeUserRole_UserNotFound_ShouldThrow()
    {
        await Assert.ThrowsAsync<UserNotFoundException>(() =>
            _userService.ChangeUserRole(_admin.Username, "non_existing_username", "User"));
    }
}