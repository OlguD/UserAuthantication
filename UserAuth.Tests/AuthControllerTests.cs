using UserAuth.Controllers;
using UserAuth.Services;
using UserAuth.Models;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace UserAuth.Tests;

public class AuthControllerTests
{
    [Fact]
    public async Task Register_ValidUser_ReturnsOk()
    {
        // Setup mock service
        var mockService = new Mock<IUserService>();
        
        // Setup the GetByUsernameAsync to return null (user doesn't exist yet)
        mockService.Setup(service => service.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null);
            
        // Setup the AddAsync method
        mockService.Setup(service => service.AddAsync(It.IsAny<User>()))
            .ReturnsAsync((User user) => user);
            
        var controller = new AuthController(mockService.Object, null);

        var user = new User
        {
            Username = "test_username",
            Password = "test_password",
            Email = "test@email.com",
            Name = "Test",
            Surname = "User"
        };
        
        // Act
        var result = await controller.Register(user);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("User registered successfully", okResult.Value);
    }
    
    [Fact]
    public async Task Register_ExistingUsername_ReturnsBadRequest()
    {
        // Setup mock service
        var mockService = new Mock<IUserService>();
        
        // Setup the GetByUsernameAsync to return a user (username exists)
        mockService.Setup(service => service.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User { 
                Username = "test_username",
                Password = "test_password",
                Email = "test@email.com",
                Name = "Test",
                Surname = "User"
            });
            
        var controller = new AuthController(mockService.Object, null);

        var user = new User
        {
            Username = "test_username",
            Password = "test_password",
            Email = "test@email.com",
            Name = "Test",
            Surname = "User"
        };
        
        // Act
        var result = await controller.Register(user);
        
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Username already exists", badRequestResult.Value);
    }
}