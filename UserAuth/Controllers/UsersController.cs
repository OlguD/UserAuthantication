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
    public async Task<IActionResult> GetProfile()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized();
        }
        var user = await _userService.GetByUsernameAsync(username);
        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> AllUsers()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("change-role")]
    public async Task<IActionResult> ChangeUserRole([FromBody] ChangeRoleRequest request)
    {
        if (request == null)
            return BadRequest("Invalid request data.");

        var updatedUser = await _userService.ChangeUserRole(
            request.UserChangingRole,
            request.UserChanging,
            request.Role);

        return Ok(updatedUser);
    }
    
    [Authorize]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var updatedUser = await _userService.ChangePasswordAsync(
            request.Username,
            request.OldPassword,
            request.NewPassword);
        
        return Ok(updatedUser);
    }
    
    [Authorize]
    [HttpPut("change-email")]
    public IActionResult ChangeEmail([FromBody] ChangeEmailRequest request)
    {
        var updatedUser = _userService.ChangeEmailAsync(
            request.Username,
            request.NewEmail);
        
        return Ok(updatedUser);
    }
}