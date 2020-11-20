using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace PixelVampire.Imaging
{
    public interface IImageService
    {
        IObservable<ImageHandle> LoadImage(string path);
    }
}
