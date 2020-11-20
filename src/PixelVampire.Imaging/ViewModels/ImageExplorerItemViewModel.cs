using PixelVampire.Shared;
using PixelVampire.Shared.ViewModels;
using ReactiveUI;
using System.Reactive;

namespace PixelVampire.Imaging.ViewModels
{
    public class ImageExplorerItemViewModel : ViewModelBase
    {
        public ImageExplorerItemViewModel(ImageHandle imageHandle)
        {
            Guard.Against.Null(imageHandle, nameof(imageHandle));

            ImageHandle = imageHandle;

            Remove = ReactiveCommand.Create(() => this);
        }
        
        public ReactiveCommand<Unit, ImageExplorerItemViewModel> Remove { get; }
        public ImageHandle ImageHandle { get; }
    }
}
