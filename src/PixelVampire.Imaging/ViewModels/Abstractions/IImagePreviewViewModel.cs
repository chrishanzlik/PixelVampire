using PixelVampire.Imaging.Models;

namespace PixelVampire.Imaging.ViewModels.Abstractions
{
    public interface IImagePreviewViewModel
    {
        ImageHandle ImageContext { get; }
    }
}
