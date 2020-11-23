using PixelVampire.Imaging.Models;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace PixelVampire.Imaging.ViewModels.Abstractions
{
    public interface IImageEditorViewModel
    {
        ReactiveCommand<string, ImageHandle> LoadImage { get; }

        ReactiveCommand<Unit, Unit> SelectNext { get; }

        ReactiveCommand<Unit, Unit> SelectPrevious { get; }

        ImageExplorerViewModel ImageExplorer { get; }

        ImagePreviewViewModel ImagePreview { get; }

        ImageHandle SelectedImage { get; set; }

        bool IsLoading { get; }

        ReadOnlyObservableCollection<ImageHandle> Images { get; }
    }
}
