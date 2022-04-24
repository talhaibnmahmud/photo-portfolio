using PhotoTest.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace PhotoTest.Models
{
    public class Comment
    {
        [Key]
        public Int64 Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string? CommentMessage { get; set; }
        [Editable(false)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set;} = DateTime.UtcNow;
        public Post? Post { get; set; }
        public PhotoTestUser? User { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Comment) return false;

            var item = obj as Comment;
            return Id == item?.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
