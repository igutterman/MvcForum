﻿using MvcForum.Models;

namespace MvcForum.ViewModels
{
    public class ThreadViewModel
    {

        public ForumThread Thread { get; set; }
        public string WebRootPath { get; set; }

        public ThreadViewModel(string webRootPath)
        {
            WebRootPath = webRootPath;
            Thread = new ForumThread(); 
        }

    }
}
