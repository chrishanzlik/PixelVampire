﻿using DynamicData;
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
        private SourceCache<ImageHandle, string> _source = new SourceCache<ImageHandle, string>(x => x.LoadingSettings.FilePath);
        private ReadOnlyObservableCollection<ImageHandle> _images;
        private ReactiveCommand<ImageHandle, ImageHandle> _calculatePreview;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageEditorViewModel" /> class.
        /// </summary>
        /// <param name="imageService">Service for image loading and manipulation.</param>
        public ImageEditorViewModel(IImageService imageService = null)
        {
            imageService ??= Locator.Current.GetService<IImageService>();

            ImageExplorer = new ImageExplorerViewModel(_source.AsObservableCache());
            ImagePreview = new ImagePreviewViewModel(this.WhenAnyValue(x => x.SelectedImage));
            LoadImage = ReactiveCommand.CreateFromObservable<string, ImageHandle>(x => imageService.LoadImage(x));
            _calculatePreview = ReactiveCommand.CreateFromObservable<ImageHandle, ImageHandle>(x => imageService.CalculatePreview(x));

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

                connection.WhenPropertyChanged(x => x.ManipulationState.Quality, false)
                    .Throttle(TimeSpan.FromMilliseconds(50))
                    .Select(x => x.Sender)
                    .InvokeCommand(_calculatePreview)
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
                Observable.CombineLatest(LoadImage.IsExecuting, _calculatePreview.IsExecuting, (a, b) => a || b)
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
        public ReactiveCommand<Unit, Unit> SelectNext { get; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> SelectPrevious { get; }

        /// <inheritdoc />
        public IImageExplorerViewModel ImageExplorer { get; }

        /// <inheritdoc />
        public IImagePreviewViewModel ImagePreview { get; }

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
    }
}
