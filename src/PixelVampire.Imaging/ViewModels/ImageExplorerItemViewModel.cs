using PixelVampire.Imaging.Models;
using PixelVampire.Shared;
using PixelVampire.Shared.ViewModels;
using ReactiveUI;
using System.Reactive;

namespace PixelVampire.Imaging.ViewModels
{
    public class ImageExplorerItemViewModel : ViewModelBase
    {
        public ImageExplorerItemViewModel(ImageExplorerItem explorerItem)
        {
            Guard.Against.Null(explorerItem, nameof(explorerItem));

            ExplorerItem = explorerItem;

            RequestRemove = ReactiveCommand.Create(() => this, outputScheduler: RxApp.TaskpoolScheduler);
        }
        
        public ReactiveCommand<Unit, ImageExplorerItemViewModel> RequestRemove { get; }
        public ImageExplorerItem ExplorerItem { get; }
    }
}
