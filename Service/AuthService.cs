using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyTodo.Database;
using MyTodo.Entities;
using MyTodo.Models;

namespace MyTodo.Service
{
    public class AuthService(TodoDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<ActionResult> LoginAsync(UserDto request, HttpContext httpContext)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                return new UnauthorizedResult();
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return new UnauthorizedResult();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                });

            return new OkResult();
        }

        public async Task<User?> RegisterAsync(UserDto request)
        {
            if(await context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return null;
            }
            var user = new User();
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.Username = request.Username;
            user.PasswordHash = hashedPassword;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;

        }
        //public string CreateToken(User user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.Username),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        //    };
        //    var key = new SymmetricSecurityKey(
        //        Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Token")));

        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        //    var tokenDescriptor = new JwtSecurityToken(
        //        issuer: configuration.GetValue<string>("Jwt:Issuer"),
        //        audience:configuration.GetValue<string>("Token:Audience"),
        //        claims:claims,
        //        expires: DateTime.UtcNow.AddDays(1),
        //        signingCredentials:creds
        //        );

        //    return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        //}

    }

}
