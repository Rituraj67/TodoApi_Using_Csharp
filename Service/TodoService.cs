
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

        public async Task<Todo?> UpdateTodo(TodoDto request, Guid TodoId)
        {
            Todo? todo =await  _context.Todos.FindAsync(TodoId);
            if(todo is null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(request.Task))
            {
                todo.Task = request.Task;
            }
            todo.IsComplete = request.IsComplete;
            await _context.SaveChangesAsync();
            return todo;
        }

        
        public async Task<Todo?> DeleteTodoAsync(Guid id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo is null) return null;
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return todo;
        }
    }
}
