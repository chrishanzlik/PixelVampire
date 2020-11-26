using PixelVampire.Imaging.Models;

namespace PixelVampire.Imaging.ViewModels.Abstractions
{
    /// <summary>
    /// Viewmodel for visual image actions.
    /// </summary>
    public interface IImagePreviewViewModel
    {
        /// <summary>
        /// Gets the actual loaded image.
        /// </summary>
        ImageHandle ImageContext { get; }
    }
}
