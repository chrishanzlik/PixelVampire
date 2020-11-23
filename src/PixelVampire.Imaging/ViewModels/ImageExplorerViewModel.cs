using DynamicData;
using PixelVampire.Imaging.Models;
using PixelVampire.Imaging.ViewModels.Abstractions;
using PixelVampire.Shared.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace PixelVampire.Imaging.ViewModels
{
    public class ImageExplorerViewModel : ViewModelBase, IImageExplorerViewModel
    {
        private ReadOnlyObservableCollection<ImageExplorerItemViewModel> _children;
        private ReactiveCommand<ImageExplorerItemViewModel, ImageHandle> RequestRemove { get; }

        public ImageExplorerViewModel(IObservableCache<ImageHandle, string> imageCache)
        {
            var connection = imageCache?.Connect() ?? throw new ArgumentNullException(nameof(imageCache));

            RequestRemove = ReactiveCommand.Create<ImageExplorerItemViewModel, ImageHandle>(
                vm => imageCache.Items.FirstOrDefault(x => x.OriginalPath == vm.ExplorerItem.FilePath),
                outputScheduler: RxApp.TaskpoolScheduler);

            Deletions = RequestRemove.AsObservable();
            Selections = this.WhenAnyValue(x => x.SelectedItem)
                .Select(vm => imageCache.Items.FirstOrDefault(x => x.OriginalPath == vm?.ExplorerItem.FilePath))
                .DistinctUntilChanged();

            this.WhenActivated(d =>
            {
                connection
                    .Filter(x => x != null)
                    .Transform(x => new ImageExplorerItem(x.OriginalPath, x.OriginalImage.ToThumbnail(50)))
                    .DisposeMany()
                    .Transform(x => new ImageExplorerItemViewModel(x)).ObserveOn(RxApp.MainThreadScheduler)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Bind(out _children)
                    .Subscribe(_ => SelectedItem ??= Children.FirstOrDefault())
                    .DisposeWith(d);

                connection
                    .Where(x => x != null)
                    .Throttle(TimeSpan.FromMilliseconds(200))
                    .Select(_ => Children.Select(x => x.RequestRemove).Merge())
                    .Switch()
                    .InvokeCommand(RequestRemove)
                    .DisposeWith(d);
            });
        }

        public ReadOnlyObservableCollection<ImageExplorerItemViewModel> Children => _children;
        
        [Reactive]
        public ImageExplorerItemViewModel SelectedItem { get; set; }

        public IObservable<ImageHandle> Selections { get; }

        public IObservable<ImageHandle> Deletions { get; }

        public void NextImage()
        {
            if (Children.Count <= 1) return;

            int actualIndex = Children.IndexOf(SelectedItem);
            int nextIndex = actualIndex < Children.Count - 1 ? actualIndex + 1 : 0;
            
            SelectedItem = Children.ElementAt(nextIndex);
        }

        public void PreviousImage()
        {
            if (Children.Count <= 1) return;

            int actualIndex = Children.IndexOf(SelectedItem);
            int nextIndex = actualIndex > 0 ? actualIndex - 1 : Children.Count - 1;

            SelectedItem = Children.ElementAt(nextIndex);
        }
    }
}
