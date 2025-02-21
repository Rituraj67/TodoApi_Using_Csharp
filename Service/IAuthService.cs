using Microsoft.AspNetCore.Mvc;
using MyTodo.Entities;
using MyTodo.Models;

namespace MyTodo.Service
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<ActionResult> LoginAsync(UserDto request, HttpContext httpContext);
    }
}
