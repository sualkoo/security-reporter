using webapi.Utils;

namespace webapi.Models
{
    public class Comment
    {
        [StringValidation(ErrorMessage = "The author name field must not be empty or contain only whitespace.")]
        public string Author { get; set; }
        [StringValidation(ErrorMessage = "The text field must not be empty or contain only whitespace.")]
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
