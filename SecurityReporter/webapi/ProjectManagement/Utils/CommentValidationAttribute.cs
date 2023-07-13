using System.ComponentModel.DataAnnotations;
using webapi.Models;

namespace webapi.Utils
{
    public class CommentValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is List<Comment> commentList)
            {
                if (commentList.Count == 0)
                {
                    return false; // Empty list is considered invalid
                }

                if (commentList.Any(comment => comment == null)) 
                { 
                    return false;
                }
            }

            return true; // Non-list values are considered valid
        }
    }
}
