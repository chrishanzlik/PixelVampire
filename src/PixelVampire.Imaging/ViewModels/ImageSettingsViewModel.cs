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
    /// Viewmodel which is related to the settings of a given <see cref="ImageHandle"/>.
    /// </summary>
    public class ImageSettingsViewModel : ViewModelBase, IImageSettingsViewModel
    {
        public ImageSettingsViewModel(IObservable<ImageHandle> imageChanges) // yes?
        {
            Guard.Against.ArgumentNull(imageChanges, nameof(imageChanges));

            this.WhenActivated(d =>
            {
                imageChanges
                    .Where(x => x != null)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Log(this)
                    .ToPropertyEx(this, x => x.Context)
                    .DisposeWith(d);
            });
        }

        [ObservableAsProperty]
        public ImageHandle Context { get; }

        //just bind to quality? but this hides dependency
        //public IObservable<ImageHandle> AffectRenderChanges { get; }
    }
}
