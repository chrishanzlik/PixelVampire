using SkiaSharp;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace PixelVampire.Imaging
{
    /// <summary>
    /// Extension methods related to SkiaSharp's bitmap class
    /// </summary>
    public static class SKBitmapExtensions
    {
        /// <summary>
        /// Creates a new <see cref="BitmapImage"/> from a <see cref="SKBitmap"/> class.
        /// </summary>
        /// <param name="bitmap">Object to be extended.</param>
        /// <returns>New created <see cref="BitmapImage"/>.</returns>
        public static BitmapImage ToBitmapImage(this SKBitmap bitmap)
        {
            using var ms = new MemoryStream();
            bitmap.Encode(ms, SKEncodedImageFormat.Png, 100);

            var image = new BitmapImage();
            ms.Position = 0;
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = ms;
            image.EndInit();
            image.Freeze();

            return image;
        }

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
    }
}
