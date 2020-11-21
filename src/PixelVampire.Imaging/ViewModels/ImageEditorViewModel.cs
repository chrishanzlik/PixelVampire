using DynamicData;
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
        private SourceCache<ImageHandle, string> _source = new SourceCache<ImageHandle, string>(x => x.OriginalPath);

        public ImageEditorViewModel(IImageService imageService = null)
        {
            imageService ??= Locator.Current.GetService<IImageService>();

            IObservable<bool> loadings = default;
            var sourceConnection = _source.Connect();

            LoadImage = ReactiveCommand.CreateFromObservable<string, ImageHandle>(x => imageService.LoadImage(x), loadings);

            loadings = LoadImage.IsExecuting;

            this.WhenActivated(d =>
            {
                LoadImage
                    .Where(x => x != null)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(handle => _source.AddOrUpdate(handle))
                    .DisposeWith(d);

                LoadImage
                    .Throttle(TimeSpan.FromMilliseconds(200))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(handle => SelectedImage = handle)
                    .DisposeWith(d);

                loadings
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .ToPropertyEx(this, x => x.IsLoading)
                    .DisposeWith(d);

                sourceConnection
                    .DisposeMany()
                    .Transform(x => new ImageExplorerItemViewModel(x))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Bind(out _images)
                    .Subscribe()
                    .DisposeWith(d);

                sourceConnection
                    .Select(_ => Images.Select(x => x.Remove).Merge())
                    .Switch()
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(x => {
                        _source.Remove(x.ImageHandle);
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

        [ObservableAsProperty]
        public bool IsLoading { get; }
    }
}
