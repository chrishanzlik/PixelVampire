using PixelVampire.Imaging.ViewModels;
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

namespace PixelVampire.Imaging.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für ImageSettingsView.xaml
    /// </summary>
    public partial class ImageSettingsView
    {
        public ImageSettingsView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {

            });
        }
    }
}
