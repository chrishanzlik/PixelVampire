using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PixelVampire.Imaging
{
    public class ImageService : IImageService
    {
        public IObservable<ImageHandle> LoadImage(string path)
        {
            return Observable.Create<ImageHandle>(observer =>
            {
                ImageHandle handle = default;

                try
                {
                    using var codec = SKCodec.Create(path);
                    if (codec == null)
                    {
                        throw new Exception("Codec is null..."); //TODO... better approach?
                    }
                    var bitmap = SKBitmap.Decode(codec);
                    handle = new ImageHandle
                    {
                        OriginalImage = bitmap,
                        Preview = bitmap,
                        Format = codec.EncodedFormat,
                        OriginalPath = path,
                        OriginalName = Path.GetFileName(path),
                        Thumbnail = CreateThumbnail(bitmap, 50, 50)
                    };
                    observer.OnNext(handle);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }

                observer.OnCompleted();

                return Disposable.Empty;
            });
        }

        private SKBitmap CreateThumbnail(SKBitmap source, int width, int height)
        {
            //using var ms = new MemoryStream();
            var skBitmap = source.Resize(new SKSizeI(width, height), SKFilterQuality.Medium);
            //skBitmap.Encode(ms, SKEncodedImageFormat.Png, 80);
            //var bitmap = new Bitmap(ms, false)

            return skBitmap;
        }
    }
}
