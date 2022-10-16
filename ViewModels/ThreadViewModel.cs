using MvcForum.Models;

namespace MvcForum.ViewModels
{
    public class ThreadViewModel
    {

        public ForumThread Thread { get; set; }
        public string WebRootPath { get; set; }

        //True if model is displayed in index view, false if thread view.
        //Used to set whether _PostPartial displays link to thread (only in index view)
        //Passed down from here to PostViewModel
        public bool IndexView { get; set; }

        //How many posts from the thread were omitted by the controller from display in index view
        //Index view should only display first post and last three (or fewer) replies
        public int OmittedPosts { get; set; }


        public ThreadViewModel(ForumThread thread, string webRootPath, bool indexView)
        {
            WebRootPath = webRootPath;
            Thread = thread;
            IndexView = indexView;
            OmittedPosts = 0;

        }

        public void TruncateForIndex()
        {
            if (Thread.Posts.Count > 4)
            {
                OmittedPosts = Thread.Posts.Count - 4;
                Thread.Posts.RemoveRange(1, OmittedPosts);

            }
        }

    }
}
