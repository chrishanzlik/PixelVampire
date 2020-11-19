using DynamicData;
using PixelVampire.Notifications;
using PixelVampire.Shared.ViewModels;
using ReactiveUI;
using SkiaSharp.Views.WPF;
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

            LoadImages = ReactiveCommand.CreateFromObservable<IEnumerable<string>, ImageLoadResult>(x => imageService.LoadImages(x));

            this.WhenActivated(d =>
            {
                source.Connect().Transform(x => new ImageExplorerItemViewModel(x)).Bind(out _images).Subscribe().DisposeWith(d);

                // duplicates are not disposed?
                LoadImages.Where(x => x != null).Subscribe(result => source.AddOrUpdate(result.Images)).DisposeWith(d);
            });


            LoadImages.Subscribe(res =>
            {
                this.Notify().PublishInfo(
                    $"{res.LoadedCount} images loaded. {res.RejectedCount} images rejected.",
                    "Image loading process completed",
                    TimeSpan.FromSeconds(2));
            });
        }

        public ReactiveCommand<IEnumerable<string>, ImageLoadResult> LoadImages { get; }
        public override string UrlPathSegment => "image-editor";
        public ReadOnlyObservableCollection<ImageExplorerItemViewModel> Images => _images;
    }
}
