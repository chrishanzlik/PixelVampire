using PixelVampire.Imaging.Exceptions;
using PixelVampire.Imaging.Models;
using ReactiveUI;
using SkiaSharp;
using System;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace PixelVampire.Imaging
{
    /// <summary>
    /// Service class for image loading and manipulation
    /// </summary>
    public class ImageService : IImageService
    {
        /// <inheritdoc />
        public IObservable<ImageHandle> LoadImage(string path, IScheduler executionScheduler = null)
        {
            executionScheduler ??= RxApp.TaskpoolScheduler;

            return Observable.Create<ImageHandle>(observer =>
            {
                CompositeDisposable disposable = new CompositeDisposable();
                ImageHandle handle = default;
                SKCodec codec = default;

                IDisposable scheduling = executionScheduler.Schedule(() =>
                {
                    try
                    {
                        codec = SKCodec.Create(path) ?? throw new ImageLoadingException(path);
                        codec.DisposeWith(disposable);

                        var bitmap = SKBitmap.Decode(codec);
                        handle = new ImageHandle(path, bitmap, codec.EncodedFormat);
                        observer.OnNext(handle);
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                    }
                    observer.OnCompleted();

                }).DisposeWith(disposable);

                return disposable;
            });
        }

        /// <inheritdoc />
        public IObservable<ImageHandle> CalculatePreview(ImageHandle handle, IScheduler executionScheduler = null)
        {
            Guard.Against.ArgumentNull(handle, "handle");
            executionScheduler ??= RxApp.TaskpoolScheduler;

            return Observable.Create<ImageHandle>(observer =>
            {
                IDisposable scheduling = executionScheduler.Schedule(() =>
                {
                    handle.Preview?.Dispose();

                    var prev = new SKBitmap(handle.OriginalImage.Width, handle.OriginalImage.Height, handle.OriginalImage.ColorType, handle.OriginalImage.AlphaType, handle.OriginalImage.ColorSpace);
                    handle.OriginalImage.ExtractSubset(prev, new SKRectI(0, 0, handle.OriginalImage.Width, handle.OriginalImage.Height));

                    handle.Preview = prev;

                    // ...

                    observer.OnNext(handle);
                    observer.OnCompleted();
                });

                return scheduling;
            });
        }
    }
}
