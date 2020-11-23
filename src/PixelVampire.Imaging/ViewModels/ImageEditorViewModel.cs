using DynamicData;
using PixelVampire.Imaging.Models;
using PixelVampire.Notifications;
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
        private SourceCache<ImageHandle, string> _source = new SourceCache<ImageHandle, string>(x => x.OriginalPath);
        private ReadOnlyObservableCollection<ImageHandle> _loadedImages;

        public ImageEditorViewModel(IImageService imageService = null)
        {
            imageService ??= Locator.Current.GetService<IImageService>();

            ImageExplorer = new ImageExplorerViewModel(_source.AsObservableCache());
            ImagePreview = new ImagePreviewViewModel(this.WhenAnyValue(x => x.SelectedImage));

            var sourceConnection = _source.Connect();

            LoadImage = ReactiveCommand.CreateFromObservable<string, ImageHandle>(x => imageService.LoadImage(x));

            this.WhenActivated(d =>
            {
                sourceConnection
                    .DisposeMany()
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Bind(out _loadedImages)
                    .Do(x =>
                    {
                        //TODO: called to early
                        if (SelectedImage == null)
                        {
                            ImageExplorer.SelectNextImage();
                        }
                    })
                    .Subscribe()
                    .DisposeWith(d);

                // Add loaded images to source
                LoadImage
                    .Where(x => x != null)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(handle => _source.AddOrUpdate(handle))
                    .DisposeWith(d);

                // Show image loading error
                LoadImage.ThrownExceptions
                    .Subscribe(_ => this.Notify()
                        .PublishError("Sorry. Could not load this file.", "Error", TimeSpan.FromSeconds(10)))
                    .DisposeWith(d);

                // Pipe loadings to property
                LoadImage.IsExecuting
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .ToPropertyEx(this, x => x.IsLoading)
                    .DisposeWith(d);

                // React on close requests from explorer view
                ImageExplorer.WhenAnyObservable(x => x.RequestRemove)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(img => {
                        _source.Remove(img);
                        if (SelectedImage == img) SelectedImage = null;
                    })
                    .DisposeWith(d);

                // Select explorer item
                ImageExplorer.WhenAnyValue(x => x.SelectedItem)
                    .Select(x => x != null ? _source.Items.FirstOrDefault(i => i.OriginalPath == x.ExplorerItem.FilePath) : null)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(x => SelectedImage = x)
                    .DisposeWith(d);
            });
        }

        public ReactiveCommand<string, ImageHandle> LoadImage { get; }

        public ImageExplorerViewModel ImageExplorer { get; }

        public ImagePreviewViewModel ImagePreview { get; }

        public override string UrlPathSegment => "image-editor";
        
        [Reactive]
        public ImageHandle SelectedImage { get; set; }

        [ObservableAsProperty]
        public bool IsLoading { get; }

        public ReadOnlyObservableCollection<ImageHandle> LoadedImages => _loadedImages;
    }
}
