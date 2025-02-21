using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyTodo.Entities
{
    public class Todo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public required string Task { get; set; }

        public bool IsComplete { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public Guid UserId { get; set; }

        // Navigation Property
        public User User { get; set; } = null!;

    }
}
