using UserAuth.Services;
using UserAuth.Models;
using Xunit;
using Xunit.Abstractions;


namespace UserAuth.Tests;

public class UserServiceTests
{
    [Fact]
    public void Register_NewUser_ShouldBeAdded()
    {
        var service = new UserService();
        var user = new User {    
            Username = "olgu",
            Password = "1234",
            Email = "olgu@mail.com",
            Name = "Olgu",
            Surname = "Degirmenci"
            
        };

        service.Add(user);
        var result = service.GetUserByUsername("olgu");
        
        Assert.NotNull(result);
        Assert.Equal("olgu", result.Username);
    }

    [Fact]
    public void Validate_CorrectCredentials_ShouldReturnUser()
    {
        var service = new UserService();
        service.Add(new User {    
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
    public void Validate_WrongCredentials_ShouldReturnNull()
    {
        var service = new UserService();
        service.Add(new User {    
            Username = "olgu",
            Password = "1234",
            Email = "olgu@mail.com",
            Name = "Olgu",
            Surname = "Degirmenci"
            
        });
        
        var user = service.Validate("olgu", "wrong_password");
        
        Assert.Null(user);
    }
}