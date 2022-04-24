using PhotoTest.Areas.Identity.Data;
using PhotoTest.Models;

namespace PhotoTest.ViewModels
{
    public class PostViewModel
    {
        public List<Comment> Comments { get; set; }
        public List<Favorite> Favorites { get; set; }
        public string Text { get; set; }
        public bool LikedPost { get; set; } = false;
        public PhotoTestUser? User { get; set; }
        public Post Post { get; set; }

        public PostViewModel()
        {
            Comments = new List<Comment>();
            Favorites = new List<Favorite>();
            Text = "";
        }
        public PostViewModel(Post post)
        {
            Comments = new List<Comment>();
            Favorites = new List<Favorite>();
            Text = "";
            Post = post;
        }
    }
}
