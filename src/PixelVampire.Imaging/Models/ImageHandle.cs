using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SkiaSharp;
using System;

namespace PixelVampire.Imaging.Models
{
    /// <summary>
    /// The image handle is for interarcting and displaying with loaded files.
    /// </summary>
    public class ImageHandle : ReactiveObject, IDisposable
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

            OriginalImage = image;
            LoadingSettings = new ImageDefaults(path, image.Width, image.Height, image.Info.BytesSize64, format.ToAppFormat());
            ManipulationState = new ManipulationState();
        }

        /// <summary>
        /// Gets the image settings at loading time.
        /// </summary>
        public ImageDefaults LoadingSettings { get; }

        /// <summary>
        /// Gets the original loaded image.
        /// </summary>
        public SKBitmap OriginalImage { get; }

        /// <summary>
        /// Gets or sets the actual image preview (manipulated image).
        /// </summary>
        [Reactive] public SKBitmap Preview { get; set; }

        /// <summary>
        /// Gets or sets the manipulation state of this image.
        /// </summary>
        [Reactive] public ManipulationState ManipulationState { get; set; }

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
