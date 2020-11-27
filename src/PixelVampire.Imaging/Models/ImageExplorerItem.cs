using PixelVampire.Shared;
using SkiaSharp;
using System;

namespace PixelVampire.Imaging.Models
{
    /// <summary>
    /// Represents the data which is stored inside a explorer tile.
    /// </summary>
    public class ImageExplorerItem : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageExplorerItem" /> class.
        /// </summary>
        /// <param name="filePath">File path of the loaded file.</param>
        /// <param name="thumbnail">Thumbnail image of the loaded file.</param>
        public ImageExplorerItem(string filePath, SKBitmap thumbnail)
        {
            Guard.Against.ArgumentNullOrEmpty(filePath, nameof(filePath));
            Guard.Against.ArgumentNull(thumbnail, nameof(thumbnail));

            FilePath = filePath;
            Thumbnail = thumbnail;
        }

        /// <summary>
        /// Gets the file path of the loaded file.
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Gets the thumbnail image of the loaded file.
        /// </summary>
        public SKBitmap Thumbnail { get; }

        /// <summary>
        /// Releases all <see cref="ImageExplorerItem"/> resources.
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
                Thumbnail?.Dispose();
            }
        }
    }
}
