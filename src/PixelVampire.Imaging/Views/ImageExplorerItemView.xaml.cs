using PixelVampire.Imaging.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
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

                this.OneWayBind(ViewModel,
                    x => x.ExplorerItem.FilePath,
                    x => x.FileNameText.Text,
                    x => Path.GetFileName(x)).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.ExplorerItem.FilePath,
                    x => x.FileNameText.ToolTip).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.ExplorerItem.Thumbnail,
                    x => x.ThumbnailImage.Source,
                    x => x.ToBitmapImage()).DisposeWith(d);
            });
        }
    }
}
