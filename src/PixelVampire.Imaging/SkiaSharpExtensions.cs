using PixelVampire.Imaging.Models;
using SkiaSharp;
using System;
using System.IO;

namespace PixelVampire.Imaging
{
    /// <summary>
    /// Extension methods related to SkiaSharp's bitmap class
    /// </summary>
    public static class SkiaSharpExtensions
    {
        /// <summary>
        /// Generates a resized bitmap within a given rectangle. (Without causing distortion)
        /// </summary>
        /// <param name="self">Object to be extended.</param>
        /// <param name="width">New max width of the bitmap.</param>
        /// <param name="height">New max height of the bitmap.</param>
        /// <param name="filterQuality">Bitmaps filter quality.</param>
        /// <returns>A new resized bitmap instance.</returns>
        public static SKBitmap ResizeFixedRatio(this SKBitmap self, int width, int height, SKFilterQuality filterQuality)
        {
            double scale = Math.Min((double)width / self.Width, (double)height / self.Height);
            int newWidth = (int)Math.Ceiling(self.Width * scale);
            int newHeight = (int)Math.Ceiling(self.Height * scale);
            return self.Resize(new SKSizeI(newWidth, newHeight), filterQuality);
        }

        /// <summary>
        /// Generates a cubic thumbnail bitmap from a given <see cref="SKBitmap"/>.
        /// </summary>
        /// <param name="self">Object to be extended.</param>
        /// <param name="sideLength">Side length of the bitmap. (X and Y axis)</param>
        /// <returns>A new bitmap instance.</returns>
        public static SKBitmap ToThumbnail(this SKBitmap self, int sideLength)
        {
            var srcHeight = self.Height;
            var srcWidth = self.Width;
            var srcShortSide = srcWidth > srcHeight ? srcHeight : srcWidth;

            int top = 0, left = 0, right = 0, bottom = 0;

            using var thumb = new SKBitmap(srcShortSide, srcShortSide);

            if (srcHeight > srcWidth)
            {
                var offset = (srcHeight - srcWidth) / 2;
                top = offset;
                right = srcShortSide;
                bottom = srcShortSide + offset;
            }
            else
            {
                var offset = (srcWidth - srcHeight) / 2;
                left = offset;
                right = offset + srcShortSide;
                bottom = srcShortSide;
            }

            self.ExtractSubset(thumb, new SKRectI(left, top, right, bottom));

            return thumb.Resize(new SKSizeI(sideLength, sideLength), SKFilterQuality.Medium);
        }

        /// <summary>
        /// Converts a SkiaSharp image format to a local one.
        /// </summary>
        /// <param name="self">Object to be extended.</param>
        /// <returns>A local style image format.</returns>
        public static ImageFormat ToAppFormat(this SKEncodedImageFormat self)
        {
            switch(self)
            {
                case SKEncodedImageFormat.Jpeg:
                    return ImageFormat.Jpeg;
                case SKEncodedImageFormat.Png:
                case SKEncodedImageFormat.Gif:
                    return ImageFormat.Png;
                case SKEncodedImageFormat.Bmp:
                    return ImageFormat.Bmp;
                default: throw new InvalidOperationException("Not a supported image format.");
            }
        }
    }
}
