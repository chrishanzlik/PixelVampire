using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelVampire.Imaging
{
    public class ImageService : IImageService
    {
        public IObservable<ImageLoadResult> LoadImages(IEnumerable<string> paths, IScheduler scheduler = null)
        {
            var result = new ImageLoadResult();

            return Observable.Create<ImageLoadResult>(observer =>
            {
                foreach(var path in paths)
                {
                    try
                    {
                        using var codec = SKCodec.Create(path);
                        if (codec == null)
                        {
                            // Not an image
                            continue;
                        }

                        SKBitmap bitmap = SKBitmap.Decode(codec);

                        var handle = new ImageHandle
                        {
                            OriginalImage = bitmap,
                            Format = codec.EncodedFormat,
                            OriginalPath = path,
                            OriginalName = Path.GetFileName(path),
                            Thumbnail = CreateThumbnail(bitmap, 200, 200)
                        };

                        result.Images.Add(handle);
                    }
                    catch(Exception ex)
                    {
                        observer.OnError(ex);
                    }

                }

                result.LoadedCount = result.Images.Count;
                result.RejectedCount = paths.Count() - result.LoadedCount;

                observer.OnNext(result);
                observer.OnCompleted();

                return () => { };
            });
        }

        private SKBitmap CreateThumbnail(SKBitmap source, int width, int height)
        {
            return source.Resize(new SKSizeI(width, height), SKFilterQuality.Medium);
        }

        private bool ValidateFilePath(string path)
        {
            return false;
        }
    }
}
