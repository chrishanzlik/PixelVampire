using PixelVampire.Imaging.Models;
using ReactiveUI;
using System.Reactive;

namespace PixelVampire.Imaging.ViewModels.Abstractions
{
    /// <summary>
    /// Represents a single image inside a <see cref="IImageExplorerViewModel"/>.
    /// </summary>
    public interface IImageExplorerItemViewModel
    {
        /// <summary>
        /// Gets the item context.
        /// </summary>
        ImageExplorerItem ExplorerItem { get; }

        /// <summary>
        /// Requests the removal of this associated image.
        /// </summary>
        ReactiveCommand<Unit, IImageExplorerItemViewModel> RequestRemove { get; }
    }
}
