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

            if (post.Files != null)
            {
                TruncateFileNames(post.Files);
            }

            WebRootPath = webRootPath;
        }

        public void TruncateFileNames(IList<UploadFile> Files)
        {
            foreach (UploadFile file in Files)
            {
                if (Path.GetFileNameWithoutExtension(file.UserFileName).Length > 25)
                {
                    file.UserFileName = Path.GetFileNameWithoutExtension(file.UserFileName).Substring(0, 25) + "..." + file.Extension;
                }
            }
        }
    }


}
