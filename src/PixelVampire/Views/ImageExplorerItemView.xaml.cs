using PixelVampire.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace PixelVampire.Views
{
    /// <summary>
    /// Interaktionslogik für ImageExplorerItemView.xaml
    /// </summary>
    public partial class ImageExplorerItemView : ReactiveUserControl<ImageExlporerItemViewModel>
    {
        public ImageExplorerItemView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {

            });
        }
    }
}
