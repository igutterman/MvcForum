using MvcForum.Models;

namespace MvcForum.ViewModels
{
    public class PostViewModel
    {

        public Post Post { get; set; }
        public string WebRootPath { get; set; }

        public PostViewModel(Post post, string webRootPath)
        {
            Post = post;
            WebRootPath = webRootPath;
        }


    }
}
