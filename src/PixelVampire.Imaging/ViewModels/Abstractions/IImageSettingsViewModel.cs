using PixelVampire.Imaging.Models;

namespace PixelVampire.Imaging.ViewModels.Abstractions
{
    /// <summary>
    /// Viewmodel which is related to the settings of a given <see cref="ImageHandle"/>.
    /// </summary>
    public interface IImageSettingsViewModel
    {
        ImageHandle Context { get; }
    }
}
