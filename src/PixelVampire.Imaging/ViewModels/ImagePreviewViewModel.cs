﻿using PixelVampire.Imaging.Models;
using PixelVampire.Imaging.ViewModels.Abstractions;
using PixelVampire.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SkiaSharp;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace PixelVampire.Imaging.ViewModels
{
    /// <summary>
    /// Viewmodel for visual image actions.
    /// </summary>
    public class ImagePreviewViewModel : ViewModelBase, IImagePreviewViewModel
    {
        public ImagePreviewViewModel(IObservable<ImageHandle> imageChanges)
        {
            Guard.Against.ArgumentNull(imageChanges, nameof(imageChanges));

            this.WhenActivated(d =>
            {
                imageChanges
                    .Where(x => x != null)
                    .Select(handle => handle.WhenAnyValue(x => x.Preview))
                    .Switch()
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .ToPropertyEx(this, x => x.Image)
                    .DisposeWith(d);
            });
        }

        /// <inheritdoc />
        [ObservableAsProperty]
        public SKBitmap Image { get; }
    }
}
