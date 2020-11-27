using ReactiveUI;
using System;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace PixelVampire.Imaging.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für ImageExplorerItemView.xaml
    /// </summary>
    public partial class ImageExplorerItemView
    {
        public ImageExplorerItemView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel,
                    x => x.RequestRemove,
                    x => x.RemoveButton).DisposeWith(d);

                this.WhenAnyValue(x => x.ViewModel.ExplorerItem)
                    .Where(x => x != null)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(x =>
                    {
                        FileNameText.Text = Path.GetFileName(x.FilePath);
                        FileNameText.ToolTip = x.FilePath;
                        ThumbnailImage.Source = x.Thumbnail?.ToBitmapImage();
                    })
                    .DisposeWith(d);
            });
        }
    }
}
