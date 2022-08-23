//using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcForum.Models
{
    public class Post
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //public string? Name { get; set; }
        public string? Subject { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsOP { get; set; }

        public bool Sage { get; set; }
        public string? Text { get; set; }

        public string Board { get; set; }

        public int IdOnBoard { get; set; }

        public string UserIP { get; set; }

        public int ThreadId { get; set; }

        public bool HasImage { get; set; }

        public IList<UploadFile>? Files { get; set; }

        
        //public string? UserFileName { get; set; }

        //public string? FullImageName { get; set; }

        //public string? ThumbImageName { get; set; }

    }
}
