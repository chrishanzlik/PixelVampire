using DynamicData;
using PixelVampire.Notifications;
using PixelVampire.Shared.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace PixelVampire.Imaging.ViewModels
{
    public class ImageEditorViewModel : RoutableViewModelBase
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
                this.Notify().PublishInfo(path, "File loaded", TimeSpan.FromSeconds(2));
        }
    }
}
