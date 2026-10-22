using System.ComponentModel.DataAnnotations;
using test_project.Data;

namespace test_project.Models
{
    public class SavedText
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }
    }
}
