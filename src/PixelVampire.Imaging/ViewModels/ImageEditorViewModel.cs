using DynamicData;
using PixelVampire.Imaging.Exceptions;
using PixelVampire.Imaging.Models;
using PixelVampire.Imaging.ViewModels.Abstractions;
using PixelVampire.Notifications;
using PixelVampire.ViewModels;
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
    /// <summary>
    /// Viewmodel which contains all image manipulation capabilities.
    /// </summary>
    public class ImageEditorViewModel : RoutableViewModelBase, IImageEditorViewModel
    {
        private readonly IImageService _imageService;
        private SourceCache<ImageHandle, string> _source = new SourceCache<ImageHandle, string>(x => x.LoadingSettings.FilePath);
        private ReadOnlyObservableCollection<ImageHandle> _images;
        private ReactiveCommand<ImageHandle, ImageHandle> _calculatePreview;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageEditorViewModel" /> class.
        /// </summary>
        /// <param name="imageService">Service for image loading and manipulation.</param>
        public ImageEditorViewModel(IImageService imageService = null)
        {
            _imageService = imageService ?? Locator.Current.GetService<IImageService>();

            IObservable<ImageHandle> selectionChanges = this.WhenAnyValue(x => x.SelectedImage).Publish().RefCount();

            ImageExplorer = new ImageExplorerViewModel(_source.AsObservableCache());
            ImagePreview = new ImagePreviewViewModel(selectionChanges);
            Settings = new ImageSettingsViewModel(selectionChanges);

            SelectFolder = new Interaction<string, string>();
            
            LoadImage = ReactiveCommand.CreateFromObservable<string, ImageHandle>(x => _imageService.LoadImage(x));
            ExportSelected = ReactiveCommand.CreateFromObservable(ExportSelectedImpl);
            ExportAll = ReactiveCommand.CreateFromObservable(ExportAllImpl);
            _calculatePreview = ReactiveCommand.CreateFromObservable<ImageHandle, ImageHandle>(x => _imageService.CalculatePreview(x));

            var connection = _source.Connect();

            this.WhenActivated(d =>
            {
                // Dispose handles removed from the source collection
                connection
                    .DisposeMany()
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Bind(out _images)
                    .Subscribe()
                    .DisposeWith(d);

                // Recaluclate image when quality changes
                connection.WhenPropertyChanged(x => x.ManipulationState.Quality, false)
                    .Throttle(TimeSpan.FromMilliseconds(50))
                    .Select(x => x.Sender)
                    .InvokeCommand(_calculatePreview)
                    .DisposeWith(d);

                // Notify about successful export.
                ExportSelected
                    .Do(name => this.Notify().PublishInfo($"Image export to {name}", "Export completed!", TimeSpan.FromSeconds(3)))
                    .Subscribe()
                    .DisposeWith(d);

                // Notify about successful export.
                ExportAll
                    .Do(path => this.Notify().PublishInfo($"All images exported to {path}", "Export completed!", TimeSpan.FromSeconds(3)))
                    .Subscribe()
                    .DisposeWith(d);

                // Add loaded images to source
                LoadImage
                    .ObserveOn(RxApp.TaskpoolScheduler)
                    .Where(x => x != null)
                    .SelectMany(x => _calculatePreview.Execute(x))
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
                Observable.CombineLatest(
                        LoadImage.IsExecuting,
                        ExportSelected.IsExecuting,
                        ExportAll.IsExecuting,
                        _calculatePreview.IsExecuting,
                        (a, b, c, d) => a || b || c || d)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .ToPropertyEx(this, x => x.IsLoading)
                    .DisposeWith(d);

                // React on close requests from explorer view
                this.WhenAnyObservable(x => x.ImageExplorer.DeletionRequests)
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

        /// <inheritdoc />
        public ReactiveCommand<string, ImageHandle> LoadImage { get; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, string> ExportSelected { get; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, string> ExportAll { get; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> SelectNext { get; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> SelectPrevious { get; }

        /// <inheritdoc />
        public IImageExplorerViewModel ImageExplorer { get; }

        /// <inheritdoc />
        public IImagePreviewViewModel ImagePreview { get; }

        /// <inheritdoc />
        public IImageSettingsViewModel Settings { get; }

        /// <inheritdoc />
        public override string UrlPathSegment => "image-editor";

        /// <inheritdoc />
        [Reactive]
        public ImageHandle SelectedImage { get; set; }

        /// <inheritdoc />
        [ObservableAsProperty]
        public bool IsLoading { get; }

        /// <inheritdoc />
        public ReadOnlyObservableCollection<ImageHandle> Images => _images;

        /// <inheritdoc />
        public Interaction<string, string> SelectFolder { get; }


        private IObservable<string> ExportSelectedImpl()
        {
            return SelectFolder.Handle("Please select a folder.")
                .Where(path => !string.IsNullOrEmpty(path))
                .SelectMany(path => _imageService.ExportImage(SelectedImage, path));
        }

        private IObservable<string> ExportAllImpl()
        {
            return SelectFolder.Handle("Please select a folder")
                .Where(path => !string.IsNullOrEmpty(path))
                .SelectMany(path => Images.ToObservable()
                    .SelectMany(handle => _imageService.ExportImage(handle, path)));
        }
    }
}
