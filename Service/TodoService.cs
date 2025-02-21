
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTodo.Database;
using MyTodo.Entities;
using MyTodo.Models;

namespace MyTodo.Service
{
    public class TodoService : ITodoService
    {
        public readonly IConfiguration _configuration;
        public readonly TodoDbContext _context;
        public TodoService(IConfiguration configuration, TodoDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<Todo> CreateTodoAsync(TodoDto request)
        {
            
            var todo = new Todo()
            {
                Task = request.Task,
                UserId = Guid.Parse(request.UserId) // Convert string to Guid
            };

            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();

            return todo;
        }

        public async Task<ICollection<Todo>> GetTodosAsync(Guid UserId)
        {
            var todos = await _context.Todos.Where(todo => todo.UserId == UserId).ToListAsync();
            return todos;
        }
    }
}
