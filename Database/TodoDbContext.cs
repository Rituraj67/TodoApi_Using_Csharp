using Microsoft.EntityFrameworkCore;
using MyTodo.Entities;


namespace MyTodo.Database
{
    public class TodoDbContext(DbContextOptions<TodoDbContext> options):DbContext(options)
    {
        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
