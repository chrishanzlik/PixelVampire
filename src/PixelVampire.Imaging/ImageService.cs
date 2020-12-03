using PixelVampire.Imaging.Exceptions;
using PixelVampire.Imaging.Models;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Reactive;
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
            Guard.Against.ArgumentNullOrEmpty(path, nameof(path));
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
                        var format = codec.EncodedFormat == SKEncodedImageFormat.Gif ? SKEncodedImageFormat.Png : codec.EncodedFormat;
                        handle = new ImageHandle(path, bitmap, format);
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
            Guard.Against.ArgumentNull(handle, nameof(handle));
            executionScheduler ??= RxApp.TaskpoolScheduler;

            return Observable.Create<ImageHandle>(observer =>
            {
                IDisposable scheduling = executionScheduler.Schedule(() =>
                {
                    var old = handle.Preview;

                    using var data = handle.OriginalImage.Encode(handle.LoadingSettings.Format.ToSkiaFormat(), handle.ManipulationState.Quality);

                    handle.Preview = SKBitmap.Decode(data);

                    old?.Dispose();

                    observer.OnNext(handle);
                    observer.OnCompleted();
                });

                return scheduling;
            });
        }

        /// <inheritdoc />
        public IObservable<string> ExportImage(ImageHandle handle, string outputPath, IScheduler executionScheduler = null)
        {
            Guard.Against.ArgumentNull(handle, nameof(handle));
            Guard.Against.ArgumentNullOrEmpty(outputPath, nameof(outputPath));
            executionScheduler ??= RxApp.TaskpoolScheduler;

            return Observable.Create<string>(observer =>
            {
                return executionScheduler.Schedule(() =>
                {
                    try
                    {
                        var originalFileName = Path.GetFileNameWithoutExtension(handle.LoadingSettings.FilePath);
                        var originalExtension = Path.GetExtension(handle.LoadingSettings.FilePath); //TODO: Compare with output format
                        var newFileName = $"{originalFileName}.min{originalExtension}";
                        var fullOutputPath = Path.Combine(outputPath, newFileName);

                        using var fs = File.Create(fullOutputPath);
                        handle.OriginalImage.Encode(fs, handle.LoadingSettings.Format.ToSkiaFormat(), handle.ManipulationState.Quality);

                        observer.OnNext(fullOutputPath);
                    }
                    catch(Exception ex)
                    {
                        observer.OnError(ex);
                    }
                    observer.OnCompleted();
                });
            });
        }
    }
}
