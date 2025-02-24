
using MyTodo.Entities;
using MyTodo.Models;

namespace MyTodo.Service
{
    public interface ITodoService
    {
        public Task<Todo> CreateTodoAsync(TodoDto request);
        public Task<ICollection<Todo>> GetTodosAsync(Guid UserId);
        public Task<Todo?> UpdateTodo(TodoDto request, Guid TodoId);
        public Task<Todo?> DeleteTodoAsync(Guid id);
        
    }
}
