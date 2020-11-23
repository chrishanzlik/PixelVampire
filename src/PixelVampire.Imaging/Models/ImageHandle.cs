using SkiaSharp;
using System;

namespace PixelVampire.Imaging.Models
{
    public class ImageHandle : IDisposable
    {
        internal ImageHandle()
        {

        }

        public string OriginalPath { get; set; }
        public string OriginalName { get; set; }
        public SKBitmap OriginalImage { get; set; }
        public SKBitmap Preview { get; set; }
        public SKEncodedImageFormat Format { get; set; }

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
