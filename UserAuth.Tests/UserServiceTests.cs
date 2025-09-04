using UserAuth.Services;
using UserAuth.Models;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserAuth.Data;
using System;

namespace UserAuth.Tests;

public class UserServiceTests
{
    [Fact]
    public async Task Register_NewUser_ShouldBeAdded()
    {
        // Create in-memory database for testing
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserAuthDb_" + Guid.NewGuid().ToString())
            .Options;
        
        var context = new AppDbContext(options);
        var service = new UserService(context);
        
        var user = new User {    
            Username = "olgu",
            Password = "1234",
            Email = "olgu@mail.com",
            Name = "Olgu",
            Surname = "Degirmenci"  
        };

        await service.AddAsync(user);
        var result = await service.GetByUsernameAsync("olgu");
        
        Assert.NotNull(result);
        Assert.Equal("olgu", result.Username);
    }

    [Fact]
    public async Task Validate_CorrectCredentials_ShouldReturnUser()
    {
        // Create in-memory database for testing
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserAuthDb_" + Guid.NewGuid().ToString())
            .Options;
        
        var context = new AppDbContext(options);
        var service = new UserService(context);
        
        await service.AddAsync(new User {    
            Username = "olgu",
            Password = "1234",
            Email = "olgu@mail.com",
            Name = "Olgu",
            Surname = "Degirmenci"
        });
        
        var user = service.Validate("olgu", "1234");

        Assert.NotNull(user);
        Assert.Equal("olgu", user.Username);
    }

    [Fact]
    public async Task Validate_WrongCredentials_ShouldReturnNull()
    {
        // Create in-memory database for testing
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserAuthDb_" + Guid.NewGuid().ToString())
            .Options;
        
        var context = new AppDbContext(options);
        var service = new UserService(context);
        
        await service.AddAsync(new User {    
            Username = "olgu",
            Password = "1234",
            Email = "olgu@mail.com",
            Name = "Olgu",
            Surname = "Degirmenci"
        });
        
        var user = service.Validate("olgu", "wrong_password");
        
        Assert.Null(user);
    }
    
    [Fact]
    public async Task ChangePassword_Success_ShouldUpdatePassword()
    {
        // Create in-memory database for testing
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserAuthDb_" + Guid.NewGuid().ToString())
            .Options;
        
        var context = new AppDbContext(options);
        var service = new UserService(context);
        
        var user = new User {    
            Username = "olgu",
            Password = "oldPassword",
            Email = "olgu@mail.com",
            Name = "Olgu",
            Surname = "Degirmenci"
        };
        
        await service.AddAsync(user);
        
        var updatedUser = await service.ChangePasswordAsync("olgu", "oldPassword", "newPassword");
        
        Assert.Equal("newPassword", updatedUser.Password);
        Assert.Equal("olgu", updatedUser.Username);
    }
    
    [Fact]
    public async Task ChangeEmail_Success_ShouldUpdateEmail()
    {
        // Create in-memory database for testing
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserAuthDb_" + Guid.NewGuid().ToString())
            .Options;
        
        var context = new AppDbContext(options);
        var service = new UserService(context);
        
        var user = new User {    
            Username = "olgu",
            Password = "1234",
            Email = "olgu@mail.com",
            Name = "Olgu",
            Surname = "Degirmenci"
        };
        
        await service.AddAsync(user);
        
        var updatedUser = await service.ChangeEmailAsync("olgu", "new@mail.com");
        
        Assert.Equal("new@mail.com", updatedUser.Email);
        Assert.Equal("olgu", updatedUser.Username);
    }
}