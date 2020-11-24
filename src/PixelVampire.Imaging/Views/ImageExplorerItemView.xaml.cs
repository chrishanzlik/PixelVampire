using PixelVampire.Imaging.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace PixelVampire.Imaging.Views
{
    /// <summary>
    /// Interaktionslogik für ImageExplorerItemView.xaml
    /// </summary>
    public partial class ImageExplorerItemView : ReactiveUserControl<ImageExplorerItemViewModel>
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
                    .Subscribe(x => {
                        FileNameText.Text = Path.GetFileName(x.FilePath);
                        FileNameText.ToolTip = x.FilePath;
                        ThumbnailImage.Source = x.Thumbnail.ToBitmapImage();
                    })
                    .DisposeWith(d);
            });
        }
    }
}
