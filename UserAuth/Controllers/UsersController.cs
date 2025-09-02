using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuth.Models;
using UserAuth.Services;
namespace UserAuth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public IActionResult GetProfile()
    {
        var username = User.Identity?.Name;
        return Ok(new { Username = username });
    }
}