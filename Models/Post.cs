//using System;
using System.ComponentModel.DataAnnotations;

namespace MvcForum.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsOP { get; set; }
        public string Text { get; set; }
        public string Board { get; set; }

        public int ThreadId { get; set; }

    }
}
