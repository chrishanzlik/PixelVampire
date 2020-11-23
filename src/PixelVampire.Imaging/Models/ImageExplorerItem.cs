using PixelVampire.Shared;
using SkiaSharp;
using System;

namespace PixelVampire.Imaging.Models
{
    public class ImageExplorerItem : IDisposable
    {
        public ImageExplorerItem(string filePath, SKBitmap thumbnail)
        {
            Guard.Against.NullOrEmpty(filePath, nameof(filePath));
            Guard.Against.Null(thumbnail, nameof(thumbnail));

            FilePath = filePath;
            Thumbnail = thumbnail;
        }

        public string FilePath { get; }
        public SKBitmap Thumbnail { get; }

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
