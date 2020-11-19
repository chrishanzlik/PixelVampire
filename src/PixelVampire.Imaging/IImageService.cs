using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace PixelVampire.Imaging
{
    public interface IImageService
    {
        IObservable<ImageLoadResult> LoadImages(IEnumerable<string> paths, IScheduler scheduler = null);
    }
}
