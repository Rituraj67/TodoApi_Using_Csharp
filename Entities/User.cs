using System.ComponentModel.DataAnnotations.Schema;

namespace MyTodo.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
    }
}
