
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTodo.Entities;
using MyTodo.Models;
using MyTodo.Service;

namespace MyTodo.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Credentials cannot be empty");
            }

            var user = await authService.RegisterAsync(request);

            if(user is null)
            {
                return BadRequest("Username Already Exists");
            }
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Credentials cannot be empty");
            }
            //var token = await authService.LoginAsync(request);
            return await authService.LoginAsync(request, HttpContext);




            //var cookieOptions = new CookieOptions
            //{
            //    HttpOnly=  true,
            //    Secure= true,
            //    SameSite= SameSiteMode.Strict,
            //    Expires = DateTime.UtcNow.AddDays(1)
            //};

            //HttpContext.Response.Cookies.Append("authToken", token, cookieOptions);
            //return Ok("User Logged In");
        }
        [HttpGet("/test")]
        [Authorize]
        public ActionResult Test()
        {
            var username = User.Identity.Name; // Retrieves username
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Retrieves user ID

            return Ok(new { message = "Access granted!", username, userId });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
