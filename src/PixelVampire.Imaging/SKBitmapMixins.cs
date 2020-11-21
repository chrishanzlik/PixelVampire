using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;

namespace PixelVampire.Imaging
{
    public static class SKBitmapMixins
    {
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

        public static SKBitmap ResizeWithLockedRatio(this SKBitmap self, double width, double height, SKFilterQuality filterQuality)
        {
            var srcHeight = (double)self.Height;
            var srcWidth = (double)self.Width;
            int targetHeight, targetWidth;

            //TODO: check for overflows

            if (srcHeight > srcWidth)
            {
                var ratio = height / srcHeight;
                targetHeight = (int)Math.Ceiling(height);
                targetWidth = (int)Math.Ceiling(ratio * srcWidth);
            }
            else
            {
                var ratio = width / srcWidth;
                targetWidth = (int)Math.Ceiling(width);
                targetHeight = (int)Math.Ceiling(ratio * srcHeight);
            }

            return self.Resize(new SKSizeI(targetWidth, targetHeight), filterQuality);
        }


    }
}
