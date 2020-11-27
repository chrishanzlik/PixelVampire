using PixelVampire.Imaging.ViewModels;
using PixelVampire.Imaging.WPF.Views;
using PixelVampire.Location;
using ReactiveUI;
using Splat;

namespace PixelVampire.Imaging.WPF
{
    /// <summary>
    /// Registration container for "Imaging"-Module
    /// </summary>
    public class ImagingLocatorRegistrations : ModuleRegistration
    {
        /// <inheritdoc />
        public override void Register(IMutableDependencyResolver resolver)
        {
            resolver.Register(() => new ImageEditorView(), typeof(IViewFor<ImageEditorViewModel>));
            resolver.Register(() => new ImageSettingsView(), typeof(IViewFor<ImageSettingsViewModel>));
            resolver.Register(() => new ImageEditorView(), typeof(IViewFor<ImageEditorViewModel>));
            resolver.Register(() => new ImageExplorerItemView(), typeof(IViewFor<ImageExplorerItemViewModel>));
            resolver.Register(() => new ImageExplorerView(), typeof(IViewFor<ImageExplorerViewModel>));
            resolver.Register(() => new ImagePreviewView(), typeof(IViewFor<ImagePreviewViewModel>));

            resolver.Register<IImageService>(() => new ImageService());
        }
    }
}