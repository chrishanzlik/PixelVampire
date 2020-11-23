using PixelVampire.Imaging.Models;
using PixelVampire.Shared.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace PixelVampire.Imaging.ViewModels
{
    public class ImagePreviewViewModel : ViewModelBase
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

        [ObservableAsProperty]
        public ImageHandle ImageContext { get; }
    }
}
