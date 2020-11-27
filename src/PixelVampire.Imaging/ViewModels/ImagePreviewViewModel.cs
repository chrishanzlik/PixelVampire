using PixelVampire.Imaging.Models;
using PixelVampire.Imaging.ViewModels.Abstractions;
using PixelVampire.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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
            if (imageChanges == null)
                throw new ArgumentNullException(nameof(imageChanges));

            this.WhenActivated(d =>
            {
                imageChanges
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .ToPropertyEx(this, x => x.ImageContext)
                    .DisposeWith(d);
            });
        }

        /// <inheritdoc />
        [ObservableAsProperty]
        public ImageHandle ImageContext { get; }
    }
}
