using DynamicData;
using PixelVampire.Notifications;
using PixelVampire.Shared.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
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

            var sourceConnection = _source.Connect();

            LoadImage = ReactiveCommand.CreateFromObservable<string, ImageHandle>(x => imageService.LoadImage(x));

            this.WhenActivated(d =>
            {
                // Add loaded images to source
                LoadImage
                    .Where(x => x != null)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(handle => _source.AddOrUpdate(handle))
                    .DisposeWith(d);

                // Display latest loaded image
                LoadImage
                    .Throttle(TimeSpan.FromMilliseconds(200))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(handle => SelectedImage = handle)
                    .DisposeWith(d);

                // Show image loading error
                LoadImage.ThrownExceptions
                    .Subscribe(_ => this.Notify()
                        .PublishError("Sorry. Could not load this file.", "Error", TimeSpan.FromSeconds(10)));

                // Pipe loadings to property
                LoadImage.IsExecuting
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .ToPropertyEx(this, x => x.IsLoading)
                    .DisposeWith(d);

                // Bind and transform source to ObservableCollection
                sourceConnection
                    .DisposeMany()
                    .Transform(x => new ImageExplorerItemViewModel(x))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Bind(out _images)
                    .Subscribe()
                    .DisposeWith(d);

                // Subscribe to ViewModel closes after new items were added
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
        public ReactiveCommand<Unit, Unit> GoNext { get; }
        public ReactiveCommand<Unit, Unit> GoPrev { get; }

        public ReadOnlyObservableCollection<ImageExplorerItemViewModel> Images => _images;
        public override string UrlPathSegment => "image-editor";
        
        [Reactive]
        public ImageHandle SelectedImage { get; set; }

        [ObservableAsProperty]
        public bool IsLoading { get; }
    }
}
