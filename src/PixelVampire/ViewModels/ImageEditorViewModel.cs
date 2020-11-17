using DynamicData;
using PixelVampire.Notifications;
using PixelVampire.ViewModels.Abstractions;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace PixelVampire.ViewModels
{
    public class ImageEditorViewModel : RoutableViewModelBase, IImageEditorViewModel
    {
        public ImageEditorViewModel()
            : base("image-editor")
        {
            FilePaths = new SourceList<string>();

            this.WhenActivated(d =>
            {
                Disposable.Create(() => { }).DisposeWith(d);
            });
        }

        public ISourceList<string> FilePaths { get; }

        public void LoadFiles(IEnumerable<string> filePaths)
        {
            foreach (var path in filePaths)
                this.Notify().PublishInfo(path);
        }
    }
}
