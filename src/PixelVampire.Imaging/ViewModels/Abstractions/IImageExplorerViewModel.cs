using PixelVampire.Imaging.Models;
using System;

namespace PixelVampire.Imaging.ViewModels.Abstractions
{
    public interface IImageExplorerViewModel
    {
        IObservable<ImageHandle> Selections { get; }

        IObservable<ImageHandle> Deletions { get; }

        void NextImage();

        void PreviousImage();
    }
}
