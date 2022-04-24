using PhotoTest.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace PhotoTest.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        public Post? Post { get; set; }
        public PhotoTestUser? User { get; set; }
    }
}
