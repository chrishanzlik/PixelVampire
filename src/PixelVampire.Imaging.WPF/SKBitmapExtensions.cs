using SkiaSharp;
using System.IO;
using System.Windows.Media.Imaging;

namespace PixelVampire.Imaging.WPF
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
    }
}
