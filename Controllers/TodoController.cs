﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTodo.Entities;
using MyTodo.Models;
using MyTodo.Service;


namespace MyTodo.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/todo")]
    public class TodoController: ControllerBase
    {
        public readonly ITodoService _todoService;
        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }
        [HttpPost("/create")]
        public async Task<ActionResult<Todo>> CreateTodo(TodoDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Task))
            {
                return BadRequest("Task cannot be empty");
            }
            var res = await _todoService.CreateTodoAsync(request);
            return Ok(res);
        }

        [HttpGet("getAllTodos")]

        public async Task<ActionResult<ICollection<Todo>>> getAllTodos()
        {
            Guid UserId;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out UserId))
            {
                var todos = await _todoService.GetTodosAsync(UserId);

                return Ok(todos);
            }
            else
            {
                return BadRequest("Invalid User ID format");
            }
        }

    }
}
