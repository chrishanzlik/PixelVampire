using PixelVampire.Imaging.Models;
using SkiaSharp;
using System;

namespace PixelVampire.Imaging
{
    public static class ImagingExtensions
    {
        /// <summary>
        /// Converts an app <see cref="ImageFormat"/> into a SkiaSharp <see cref="SKEncodedImageFormat"/>.
        /// </summary>
        /// <param name="self">Object to be extended.</param>
        /// <returns>Converted SkiaSharp image format</returns>
        public static SKEncodedImageFormat ToSkiaFormat(this ImageFormat self)
        {
            switch(self)
            {
                case ImageFormat.Bmp:
                    return SKEncodedImageFormat.Bmp;
                case ImageFormat.Jpeg:
                    return SKEncodedImageFormat.Jpeg;
                case ImageFormat.Png:
                    return SKEncodedImageFormat.Png;
                default: throw new InvalidOperationException("Not a supported image format.");

            }
        }
    }
}
