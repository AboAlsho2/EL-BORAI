using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELBORAI.API.Controllers;

[ApiController]
[Route("api/test-auth")]
public class TestAuthController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("Anyone can access this.");
    }

    [Authorize]
    [HttpGet("protected")]
    public IActionResult Protected()
    {
        return Ok("You are authenticated.");
    }

    [Authorize(Roles = "CUSTOMER")]
    [HttpGet("customer")]
    public IActionResult Customer()
    {
        return Ok("You are a CUSTOMER.");
    }

    [Authorize(Roles = "MERCHANT")]
    [HttpGet("merchant")]
    public IActionResult Merchant()
    {
        return Ok("You are a MERCHANT.");
    }

    [Authorize(Roles = "ADMIN")]
    [HttpGet("admin")]
    public IActionResult Admin()
    {
        return Ok("You are an ADMIN.");
    }

    [Authorize]
    [HttpGet("claims")]
    public IActionResult Claims()
    {
        return Ok(User.Claims.Select(c => new
        {
            c.Type,
            c.Value
        }));
    }

    [Authorize]
    [HttpGet("role-check")]
    public IActionResult RoleCheck()
    {
        return Ok(new
        {
            IsCustomer = User.IsInRole("CUSTOMER"),
            IsMerchant = User.IsInRole("MERCHANT"),
            IsAdmin = User.IsInRole("ADMIN")
        });
    }
}