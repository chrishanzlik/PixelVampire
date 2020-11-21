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

        public static SKBitmap ResizeFixedRatio(this SKBitmap self, int width, int height, SKFilterQuality filterQuality)
        {
            double scale = Math.Min((double)width / self.Width, (double)height / self.Height);
            int newWidth = (int)Math.Ceiling(self.Width * scale);
            int newHeight = (int)Math.Ceiling(self.Height * scale);
            return self.Resize(new SKSizeI(newWidth, newHeight), filterQuality);
        }
    }
}
