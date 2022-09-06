using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcForum.Models

{
    public class UploadFile
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Post Post { get; set; }

        public string UserFileName { get; set; }

        public string Extension { get; set; }

        public string FullFileName { get; set; }

        public string? ThumbFileName { get; set; }



    }
}
