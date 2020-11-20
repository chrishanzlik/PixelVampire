using SkiaSharp;
using System;
using System.IO;
using System.Reactive.Linq;

namespace PixelVampire.Imaging
{
    public class ImageService : IImageService
    {
        public IObservable<ImageHandle> LoadImage(string path)
        {
            return Observable.Create<ImageHandle>(observer =>
            {
                ImageHandle handle = default;
                SKCodec codec = default;

                try
                {
                    codec = SKCodec.Create(path);
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
                        Thumbnail = CreateThumbnail(bitmap, 50)
                    };
                    observer.OnNext(handle);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }

                observer.OnCompleted();

                return codec;
            });
        }

        private static SKBitmap CreateThumbnail(SKBitmap source, int sideLength)
        {
            var srcHeight = source.Height;
            var srcWidth = source.Width;
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

            source.ExtractSubset(thumb, new SKRectI(left, top, right, bottom));

            return thumb.Resize(new SKSizeI(sideLength, sideLength), SKFilterQuality.Medium); ;
        }
    }
}
