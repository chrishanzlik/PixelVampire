using DynamicData;
using PixelVampire.ViewModels.Abstractions;

namespace PixelVampire.ViewModels
{
    public class ImageEditorViewModel : RoutableViewModelBase, IImageEditorViewModel
    {
        public ImageEditorViewModel() : base("image-editor")
        {
            FilePaths = new SourceList<string>();
        }

        public ISourceList<string> FilePaths { get; }
    }
}
