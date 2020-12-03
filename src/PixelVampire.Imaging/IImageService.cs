using PixelVampire.Imaging.Models;
using System;
using System.Reactive;
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

        /// <summary>
        /// Calcualtes the internal <see cref="ImageHandle.Preview"/> property depending on the given <see cref="ImageHandle.ManipulationState"/>.
        /// </summary>
        /// <param name="handle">Image to be processed.</param>
        /// <param name="executionScheduler">Which <see cref="IScheduler"/> should be used for work. Defaults to TaskPoolScheduler if null.</param>
        /// <returns>The updated handle.</returns>
        IObservable<ImageHandle> CalculatePreview(ImageHandle handle, IScheduler executionScheduler = null);

        /// <summary>
        /// Exports a given <see cref="ImageHandle"/> to the specified output path.
        /// </summary>
        /// <param name="handle">Image to be exported.</param>
        /// <param name="outputPath">Location where the image should be stored to.</param>
        /// <param name="executionScheduler">Which <see cref="IScheduler"/> should be used for work. Defaults to TaskPoolScheduler if null.</param>
        /// <returns>Completion signal.</returns>
        IObservable<Unit> ExportImage(ImageHandle handle, string outputPath, IScheduler executionScheduler = null);
    }
}
