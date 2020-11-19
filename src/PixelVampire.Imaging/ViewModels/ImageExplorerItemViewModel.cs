using PixelVampire.Shared.ViewModels;

namespace PixelVampire.Imaging.ViewModels
{
    public class ImageExplorerItemViewModel : ViewModelBase
    {
        public ImageExplorerItemViewModel(ImageHandle imageHandle)
        {
            ImageHandle = imageHandle;
        }

        public ImageHandle ImageHandle { get; }
    }
}
