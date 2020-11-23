using DynamicData;
using DynamicData.Binding;
using PixelVampire.Imaging.Models;
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
    public class ImageExplorerViewModel : ViewModelBase
    {
        private ReadOnlyObservableCollection<ImageExplorerItemViewModel> _children;

        public ImageExplorerViewModel(IObservableCache<ImageHandle, string> imageCache)
        {
            var connection = imageCache.Connect();

            RequestRemove = ReactiveCommand.Create<ImageExplorerItemViewModel, ImageHandle>(
                vm => imageCache.Items.FirstOrDefault(x => x.OriginalPath == vm.ExplorerItem.FilePath),
                outputScheduler: RxApp.TaskpoolScheduler);


            this.WhenActivated(d =>
            {
                connection
                    .Filter(x => x != null)
                    .Transform(x => new ImageExplorerItem(x.OriginalPath, x.OriginalImage.ToThumbnail(50)))
                    .DisposeMany()
                    .Transform(x => new ImageExplorerItemViewModel(x)).ObserveOn(RxApp.MainThreadScheduler)
                    .Bind(out _children)
                    .Subscribe()
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

        public ReactiveCommand<ImageExplorerItemViewModel, ImageHandle> RequestRemove { get; }

        public ReadOnlyObservableCollection<ImageExplorerItemViewModel> Children => _children;

        [Reactive]
        public ImageExplorerItemViewModel SelectedItem { get; set; }

        public void SelectNextImage()
        {
            if (Children.Count < 1) return;

            var actualIndex = Children.IndexOf(SelectedItem);

            var nextIndex = actualIndex == Children.Count ? 0 : actualIndex + 1;

            SelectedItem = Children.ElementAtOrDefault(nextIndex);
        }

        public void SelectPreviousImage()
        {
            if (Children.Count < 1) return;

            var actualIndex = Children.IndexOf(SelectedItem);

            var nextIndex = actualIndex == 0 ? Children.Count : actualIndex - 1;

            SelectedItem = Children.ElementAtOrDefault(nextIndex);
        }
    }
}
