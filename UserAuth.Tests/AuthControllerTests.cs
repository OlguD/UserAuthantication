using UserAuth.Controllers;
using UserAuth.Services;
using UserAuth.Models;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace UserAuth.Tests;

public class AuthControllerTests
{
    [Fact]
    public void Register_ValidUser_ReturnsOk()
    {
        var mockService = new Mock<IUserService>();
        var controller = new AuthController(mockService.Object, null);

        var user = (new User
        {
            Username = "test_username",
            Password = "test_password",
            Email = "test@email.com",
            Name = "Test",
            Surname = "User"
        });
        
        var result = controller.Register(user);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("User registered successfully", okResult.Value);
    }
    
    
    
}