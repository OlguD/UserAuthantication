using Microsoft.AspNetCore.Mvc;
using UserAuth.Services;
using UserAuth.Models;
using UserAuth.Helpers;

namespace UserAuth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly JwtHelper _jwtHelper;

    public AuthController(IUserService userService, JwtHelper jwtHelper)
    {
        _userService = userService;
        _jwtHelper = jwtHelper;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        if (_userService.GetUserByUsername(user.Username) != null)
        {
            return BadRequest("Username already exists");
        }

        _userService.Add(user);
        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        var user = _userService.Validate(loginRequest.Username, loginRequest.Password);
        if (user == null) return Unauthorized();

        var token = _jwtHelper.GenerateToken(user.Username);
        return Ok(new { token });
    }
}