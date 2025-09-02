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
    public IActionResult ChangeUserRole([FromBody] User userChangingRole, [FromBody] string userChanging)
    {
        if (userChangingRole != null && userChanging != null)
        {
            _userService.ChangeUserRole(userChangingRole, userChanging);
            return Ok("User role changed successfully.");
        }
        return BadRequest("Invalid request data.");
    }
    
    [Authorize]
    [HttpPut("change-password")]
    public IActionResult ChangePassword([FromBody] User user, [FromBody] string newPassword)
    {
        var updatedUser = _userService.GetUserByUsername(user.Username);
        if (updatedUser != null)
        {
            _userService.ChangePassword(updatedUser, newPassword);
            return Ok("Password changed successfully.");
        }
        return BadRequest("User Not Found.");
    }
    
    [Authorize]
    [HttpPut("change-email")]
    public IActionResult ChangeEmail([FromBody] User user, [FromBody] string newEmail)
    {
        var updatedUser = _userService.GetUserByUsername(user.Username);
        if (updatedUser != null)
        {
            _userService.ChangeEmail(user, newEmail);
            return Ok("Email changed successfully.");
        }
        return BadRequest("User Not Found.");
    }
}