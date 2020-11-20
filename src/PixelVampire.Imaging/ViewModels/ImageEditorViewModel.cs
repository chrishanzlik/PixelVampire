using DynamicData;
using DynamicData.Binding;
using PixelVampire.Shared.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace PixelVampire.Imaging.ViewModels
{
    public class ImageEditorViewModel : RoutableViewModelBase
    {
        private ReadOnlyObservableCollection<ImageExplorerItemViewModel> _images;

        public ImageEditorViewModel(IImageService imageService = null)
        {
            imageService ??= Locator.Current.GetService<IImageService>();

            var source = new SourceCache<ImageHandle, string>(x => x.OriginalPath);

            LoadImage = ReactiveCommand.CreateFromObservable<string, ImageHandle>(imageService.LoadImage);

            this.WhenActivated(d =>
            {
                LoadImage
                    .Where(x => x != null)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(handle => source.AddOrUpdate(handle))
                    .DisposeWith(d);

                source
                    .Connect()
                    .DisposeMany()
                    .Transform(x => new ImageExplorerItemViewModel(x))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Bind(out _images)
                    .Subscribe()
                    .DisposeWith(d);

                Images
                    .ToObservableChangeSet()
                    .Select(_ => Images.Select(x => x.Remove).Merge())
                    .Switch()
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(x => {
                        source.Remove(x.ImageHandle);
                        if (SelectedImage == x.ImageHandle) SelectedImage = null;
                    })
                    .DisposeWith(d);
            });
        }

        public ReactiveCommand<string, ImageHandle> LoadImage { get; }
        public override string UrlPathSegment => "image-editor";
        public ReadOnlyObservableCollection<ImageExplorerItemViewModel> Images => _images;
        
        [Reactive]
        public ImageHandle SelectedImage { get; set; }
    }
}
