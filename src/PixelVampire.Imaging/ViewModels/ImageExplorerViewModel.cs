using DynamicData;
using PixelVampire.Imaging.Models;
using PixelVampire.Imaging.ViewModels.Abstractions;
using PixelVampire.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace PixelVampire.Imaging.ViewModels
{
    /// <summary>
    /// Viewmodel for browsing and displaying a list of loaded images.
    /// </summary>
    public class ImageExplorerViewModel : ViewModelBase, IImageExplorerViewModel
    {
        private ReadOnlyObservableCollection<IImageExplorerItemViewModel> _children;
        private ReactiveCommand<IImageExplorerItemViewModel, ImageHandle> _requestRemove;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageExplorerViewModel" /> class.
        /// </summary>
        /// <param name="imageCache">Cache which notifies about loaded or removed images.</param>
        public ImageExplorerViewModel(IObservableCache<ImageHandle, string> imageCache)
        {
            var connection = imageCache?.Connect() ?? throw new ArgumentNullException(nameof(imageCache));

            SelectNext = ReactiveCommand.Create(SelectNextImpl);
            SelectPrevious = ReactiveCommand.Create(SelectPreviousImpl);
            _requestRemove = ReactiveCommand.Create<IImageExplorerItemViewModel, ImageHandle>(
                vm => imageCache.Items.FirstOrDefault(x => x.LoadingSettings.FilePath == vm.ExplorerItem.FilePath),
                outputScheduler: RxApp.TaskpoolScheduler);

            DeletionRequests = _requestRemove.AsObservable();
            Selections = this.WhenAnyValue(x => x.SelectedItem)
                .DistinctUntilChanged()
                .Select(vm => imageCache.Items.FirstOrDefault(x => x.LoadingSettings.FilePath == vm?.ExplorerItem.FilePath));

            this.WhenActivated(d =>
            {
                connection
                    .ObserveOn(RxApp.TaskpoolScheduler)
                    .Filter(x => x?.Preview != null)
                    .Transform(x => new ImageExplorerItem(x.LoadingSettings.FilePath, x.OriginalImage.ToThumbnail(50)))
                    .DisposeMany()
                    .Transform(x => new ImageExplorerItemViewModel(x) as IImageExplorerItemViewModel)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Bind(out _children)
                    .Subscribe(_ => SelectedItem ??= Children.FirstOrDefault())
                    .DisposeWith(d);

                connection
                    .Where(x => x != null)
                    .Throttle(TimeSpan.FromMilliseconds(200))
                    .Select(_ => Children.Select(x => x.RequestRemove).Merge())
                    .Switch()
                    .InvokeCommand(_requestRemove)
                    .DisposeWith(d);
            });
        }



        /// <inheritdoc />
        public ReadOnlyObservableCollection<IImageExplorerItemViewModel> Children => _children;

        /// <inheritdoc />
        [Reactive]
        public IImageExplorerItemViewModel SelectedItem { get; set; }

        /// <inheritdoc />
        public IObservable<ImageHandle> Selections { get; }

        /// <inheritdoc />
        public IObservable<ImageHandle> DeletionRequests { get; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, IImageExplorerItemViewModel> SelectNext { get; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, IImageExplorerItemViewModel> SelectPrevious { get; }


        private IImageExplorerItemViewModel SelectNextImpl()
        {
            if (Children.Count <= 1) return SelectedItem;

            int actualIndex = Children.IndexOf(SelectedItem);
            int nextIndex = actualIndex < Children.Count - 1 ? actualIndex + 1 : 0;
            
            return SelectedItem = Children.ElementAt(nextIndex);
        }

        public IImageExplorerItemViewModel SelectPreviousImpl()
        {
            if (Children.Count <= 1) return SelectedItem;

            int actualIndex = Children.IndexOf(SelectedItem);
            int nextIndex = actualIndex > 0 ? actualIndex - 1 : Children.Count - 1;

            return SelectedItem = Children.ElementAt(nextIndex);
        }
    }
}
