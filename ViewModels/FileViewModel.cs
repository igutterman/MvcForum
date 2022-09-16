using MvcForum.Models;



namespace MvcForum.ViewModels
{
    public class FileViewModel
    {

        public UploadFile File { get; set; }

        public string WebRootPath { get; set; }

        public bool IsOp { get; set; }

        public FileViewModel(UploadFile file, string webRootPath, bool isOp)
        {
            File = file;
            WebRootPath = webRootPath;
            IsOp = isOp;
        }
    }
}
