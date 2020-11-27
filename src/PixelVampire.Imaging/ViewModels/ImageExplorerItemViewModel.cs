using PixelVampire.Imaging.Models;
using PixelVampire.Imaging.ViewModels.Abstractions;
using PixelVampire.ViewModels;
using ReactiveUI;
using System.Reactive;

namespace PixelVampire.Imaging.ViewModels
{
    /// <summary>
    /// Represents a single image inside a <see cref="ImageExplorerViewModel"/>.
    /// </summary>
    public class ImageExplorerItemViewModel : ViewModelBase, IImageExplorerItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageExplorerItemViewModel" /> class.
        /// </summary>
        /// <param name="explorerItem">Object which should be displayed inside the image explorer.</param>
        public ImageExplorerItemViewModel(ImageExplorerItem explorerItem)
        {
            Guard.Against.ArgumentNull(explorerItem, nameof(explorerItem));

            ExplorerItem = explorerItem;

            RequestRemove = ReactiveCommand.Create(
                () => this as IImageExplorerItemViewModel,
                outputScheduler: RxApp.TaskpoolScheduler);
        }
        
        /// <inheritdoc />
        public ReactiveCommand<Unit, IImageExplorerItemViewModel> RequestRemove { get; }

        /// <inheritdoc />
        public ImageExplorerItem ExplorerItem { get; }
    }
}
