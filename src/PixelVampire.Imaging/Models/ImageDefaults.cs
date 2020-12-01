namespace PixelVampire.Imaging.Models
{
    /// <summary>
    /// Represent settings of an image at loading time.
    /// </summary>
    public class ImageDefaults
    {
        public ImageDefaults(string filePath, int width, int height, long size, ImageFormat format)
        {
            Guard.Against.ArgumentNullOrEmpty(filePath, "filePath");
            Guard.Ensure.ArgumentGreaterThan(0, width, "width");
            Guard.Ensure.ArgumentGreaterThan(0, height, "height");
            Guard.Ensure.ArgumentGreaterThan(0, size, "size");

            FilePath = filePath;
            Width = width;
            Height = height;
            Size = size;
            Format = format;
        }

        /// <summary>
        /// Gets the original file path of the referenced image.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Gets the original width of the referenced image.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Gets the original height of the referenced image.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the original size in bytes of the referenced image.
        /// </summary>
        public long Size { get; }

        /// <summary>
        /// Gets the original file format of the referenced image.
        /// </summary>
        public ImageFormat Format { get; }
    }
}
