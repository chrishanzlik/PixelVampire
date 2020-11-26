using System;

namespace PixelVampire.Imaging.Exceptions
{
    /// <summary>
    /// Exception which occurs during image loading.
    /// </summary>
    public class ImageLoadingException : Exception
    {
        /// <summary>
        /// Gets the file path on users disk.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLoadingException" /> class.
        /// </summary>
        /// <param name="path">Path of the file to be loaded.</param>
        public ImageLoadingException(string path)
        {
            FilePath = path;
        }
    }
}
