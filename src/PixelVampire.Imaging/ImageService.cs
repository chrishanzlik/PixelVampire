using PixelVampire.Imaging.Models;
using PixelVampire.Notifications;
using PixelVampire.Shared;
using ReactiveUI;
using SkiaSharp;
using System;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace PixelVampire.Imaging
{
    public class ImageService : IImageService, IEnableNotifications
    {
        public IObservable<ImageHandle> LoadImage(string path, IScheduler executionScheduler = null)
        {
            executionScheduler ??= RxApp.TaskpoolScheduler;

            return Observable.Create<ImageHandle>(observer =>
            {
                CompositeDisposable disposable = new CompositeDisposable();
                ImageHandle handle = default;
                SKCodec codec = default;

                executionScheduler.Schedule(() =>
                {
                    try
                    {
                        codec = SKCodec.Create(path);
                        codec.DisposeWith(disposable);
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
                            OriginalName = Path.GetFileName(path)
                        };
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
    }
}
