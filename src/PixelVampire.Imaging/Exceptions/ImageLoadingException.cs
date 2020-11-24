using System;

namespace PixelVampire.Imaging.Exceptions
{
    public class ImageLoadingException : Exception
    {
        public string FilePath { get; }

        public ImageLoadingException(string path)
        {
            FilePath = path;
        }
    }
}
