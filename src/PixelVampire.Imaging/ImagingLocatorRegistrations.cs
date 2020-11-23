using PixelVampire.Imaging.ViewModels;
using PixelVampire.Imaging.Views;
using PixelVampire.Shared.Locator;
using ReactiveUI;
using Splat;

namespace PixelVampire.Imaging
{
    public class ImagingLocatorRegistrations : ModuleRegistration
    {
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
