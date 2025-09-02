using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuth.Models;
using UserAuth.Services;
namespace UserAuth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [Authorize]
    [HttpGet("me")]
    public IActionResult GetProfile()
    {
        var username = User.Identity?.Name;
        return Ok(new { Username = username });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public IActionResult AllUsers()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("change-role")]
    public IActionResult ChangeUserRole([FromBody] ChangeRoleRequest request)
    {
        if (request != null && !string.IsNullOrEmpty(request.UserChanging))
        {
            _userService.ChangeUserRole(request.UserChangingRole, request.UserChanging);
            return Ok("User role changed successfully.");
        }
        return BadRequest("Invalid request data.");
    }
    
    [Authorize]
    [HttpPut("change-password")]
    public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var updatedUser = _userService.GetUserByUsername(request.User.Username);
        if (updatedUser != null)
        {
            _userService.ChangePassword(updatedUser, request.NewPassword);
            return Ok("Password changed successfully.");
        }
        return BadRequest("User Not Found.");
    }
    
    [Authorize]
    [HttpPut("change-email")]
    public IActionResult ChangeEmail([FromBody] ChangeEmailRequest request)
    {
        var updatedUser = _userService.GetUserByUsername(request.User.Username);
        if (updatedUser != null)
        {
            _userService.ChangeEmail(request.User, request.NewEmail);
            return Ok("Email changed successfully.");
        }
        return BadRequest("User Not Found.");
    }
}