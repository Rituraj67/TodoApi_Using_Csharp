using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyTodo.Models
{
    public class TodoDto//: IValidatableObject
    {
        public string Task { get; set; } = string.Empty;
        public bool IsComplete { get; set; } = false;
        public string UserId { get; set; } = string.Empty;
    }
}

//[FirstLetterUpperCase]
//public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
//{
//    if (!string.IsNullOrEmpty(Task))
//    {
//        var firstLetter = Task.ToString()[0].ToString();

//        if (firstLetter != firstLetter.ToUpper())
//        {
//            yield return new ValidationResult("First Letter should be uppercase");
//        }
//    }
//}
