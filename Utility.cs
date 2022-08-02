using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MvcForum
{
    //static class providing methods for file processing and read/write operations
    public static class Utility
    {

        //Takes path to full-size image and path to save thumbnail to,
        //creates and saves thumbnail, returns path to thumbnail
        public static void CreateThumbnail(string FullImagePath, string ThumbnailPath)
        {

            using (var image = SixLabors.ImageSharp.Image.Load(FullImagePath))
            {


                    double Factor = Math.Min(250.0 / image.Height, 250.0 / image.Width);

                    int NewHeight = (int)(image.Height * Factor);
                    int NewWidth = (int)(image.Width * Factor);

                    image.Mutate(x => x.Resize(NewWidth, NewHeight));

                    image.Save(ThumbnailPath);

            }

        }

        public static void DeleteFile(string FilePath)
        {
            throw new NotImplementedException();
        }



    }
}
