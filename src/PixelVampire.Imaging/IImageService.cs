using PixelVampire.Imaging.Models;
using System;
using System.Reactive.Concurrency;

namespace PixelVampire.Imaging
{
    public interface IImageService
    {
        IObservable<ImageHandle> LoadImage(string path, IScheduler executionScheduler = null);
    }
}
