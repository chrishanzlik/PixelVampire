using PixelVampire.Imaging.Models;
using PixelVampire.Imaging.ViewModels.Abstractions;
using PixelVampire.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive.Disposables;

namespace PixelVampire.Imaging.ViewModels
{
    /// <summary>
    /// Viewmodel which is related to the settings of a given <see cref="ImageHandle"/>.
    /// </summary>
    public class ImageSettingsViewModel : ViewModelBase, IImageSettingsViewModel
    {
        public ImageSettingsViewModel()
        {
            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.Context)
                    .Subscribe()
                    .DisposeWith(d);
            });
        }

        [Reactive] public ImageHandle Context { get; set; }
    }
}
