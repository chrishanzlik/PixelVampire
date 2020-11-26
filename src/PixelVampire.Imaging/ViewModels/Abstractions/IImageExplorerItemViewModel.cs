using PixelVampire.Imaging.Models;
using ReactiveUI;
using System.Reactive;

namespace PixelVampire.Imaging.ViewModels.Abstractions
{
    public interface IImageExplorerItemViewModel
    {
        ImageExplorerItem ExplorerItem { get; }

        ReactiveCommand<Unit, ImageExplorerItemViewModel> RequestRemove { get; }
    }
}
