using DynamicData;
using PixelVampire.Imaging.Exceptions;
using PixelVampire.Imaging.Models;
using PixelVampire.Imaging.ViewModels.Abstractions;
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
    public class ImageEditorViewModel : RoutableViewModelBase, IImageEditorViewModel
    {
        private SourceCache<ImageHandle, string> _source = new SourceCache<ImageHandle, string>(x => x.OriginalPath);
        private ReadOnlyObservableCollection<ImageHandle> _images;

        public ImageEditorViewModel(IImageService imageService = null)
        {
            imageService ??= Locator.Current.GetService<IImageService>();

            ImageExplorer = new ImageExplorerViewModel(_source.AsObservableCache());
            ImagePreview = new ImagePreviewViewModel(this.WhenAnyValue(x => x.SelectedImage));
            LoadImage = ReactiveCommand.CreateFromObservable<string, ImageHandle>(x => imageService.LoadImage(x));
            SelectNext = ReactiveCommand.Create(ImageExplorer.SelectNext);
            SelectPrevious = ReactiveCommand.Create(ImageExplorer.SelectPrevious);

            this.WhenActivated(d =>
            {
                _source
                    .Connect()
                    .DisposeMany()
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Bind(out _images)
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
                    .OfType<ImageLoadingException>()
                    .Subscribe(ex => this.Notify()
                        .PublishError($"Sorry. \"{ex.FilePath}\" does not have a supported file format.", "Error", TimeSpan.FromSeconds(5)))
                    .DisposeWith(d);

                // Pipe loadings to property
                LoadImage.IsExecuting
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .ToPropertyEx(this, x => x.IsLoading)
                    .DisposeWith(d);

                // React on close requests from explorer view
                this.WhenAnyObservable(x => x.ImageExplorer.Deletions)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(x => _source.Remove(x))
                    .DisposeWith(d);

                // Select explorer item
                this.WhenAnyObservable(x => x.ImageExplorer.Selections)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(x => SelectedImage = x)
                    .DisposeWith(d);
            });
        }

        public ReactiveCommand<string, ImageHandle> LoadImage { get; }

        public ReactiveCommand<Unit, Unit> SelectNext { get; }

        public ReactiveCommand<Unit, Unit> SelectPrevious { get; }

        public ImageExplorerViewModel ImageExplorer { get; }

        public ImagePreviewViewModel ImagePreview { get; }

        public override string UrlPathSegment => "image-editor";
        
        [Reactive]
        public ImageHandle SelectedImage { get; set; }

        [ObservableAsProperty]
        public bool IsLoading { get; }

        public ReadOnlyObservableCollection<ImageHandle> Images => _images;
    }
}
