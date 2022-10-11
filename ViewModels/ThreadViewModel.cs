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


        public ThreadViewModel(ForumThread thread, string webRootPath, bool indexView)
        {
            WebRootPath = webRootPath;
            Thread = thread;
            IndexView = indexView;

        }

    }
}
