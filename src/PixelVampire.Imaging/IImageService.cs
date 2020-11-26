using PixelVampire.Imaging.Models;
using System;
using System.Reactive.Concurrency;

namespace PixelVampire.Imaging
{
    /// <summary>
    /// Service class for image loading and manipulation
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Generates a new image handle for an image file stored on users disk.
        /// </summary>
        /// <param name="path">Path where the image can be found.</param>
        /// <param name="executionScheduler">Which <see cref="IScheduler"/> should be used for image loading. Defaults to TaskPoolScheduler if null.</param>
        /// <returns>The loaded image wrapped inside a <see cref="ImageHandle"/>.</returns>
        IObservable<ImageHandle> LoadImage(string path, IScheduler executionScheduler = null);
    }
}
