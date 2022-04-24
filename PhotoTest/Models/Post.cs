using PhotoTest.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoTest.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.MultilineText)]
        public string? Status { get; set; }
        public string? ImagePath { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public Int64 FavoriteCount { get; set; } = 0;
        [Editable(false)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set;} = DateTime.Now;
        public PhotoTestUser? User { get; set; }

        public List<Comment>? Comments { get; set; }
        public List<Favorite>? Favorites { get; set; }
    }
}
