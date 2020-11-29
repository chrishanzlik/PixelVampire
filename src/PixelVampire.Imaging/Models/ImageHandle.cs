using SkiaSharp;
using System;
using System.IO;

namespace PixelVampire.Imaging.Models
{
    /// <summary>
    /// The image handle is for interarcting and displaying with loaded files.
    /// </summary>
    public class ImageHandle : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageHandle" /> class.
        /// </summary>
        /// <param name="path">Path of the loaded image</param>
        /// <param name="image">Loaded image data</param>
        /// <param name="format">Format of the image</param>
        public ImageHandle(string path, SKBitmap image, SKEncodedImageFormat format)
        {
            Guard.Against.ArgumentNullOrEmpty(path, "path");
            Guard.Against.ArgumentNull(image, "image");

            OriginalPath = path;
            OriginalName = Path.GetFileName(path);
            OriginalImage = image;
            Format = format;

            //TODO: temp
            Preview = OriginalImage;
        }

        /// <summary>
        /// Gets the original image path on users local disk.
        /// </summary>
        public string OriginalPath { get; }

        /// <summary>
        /// Gets the original image name.
        /// </summary>
        public string OriginalName { get; }

        /// <summary>
        /// Gets the original image data.
        /// </summary>
        public SKBitmap OriginalImage { get; }

        /// <summary>
        /// Gets the image format.
        /// </summary>
        public SKEncodedImageFormat Format { get; }

        /// <summary>
        /// Gets or sets the actual image preview (manipulated image).
        /// </summary>
        public SKBitmap Preview { get; set; }

        /// <summary>
        /// Gets or sets the manipulation state of this image.
        /// </summary>
        public ManipulationState ManipulationState { get; set; }

        /// <summary>
        /// Releases all <see cref="ImageHandle"/> resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                OriginalImage?.Dispose();
                Preview?.Dispose();
            }
        }
    }
}
