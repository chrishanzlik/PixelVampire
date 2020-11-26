using PixelVampire.Shared.Controls;
using PixelVampire.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace PixelVampire.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, 
                    x => x.Router, 
                    x => x.RoutedViewHost.Router).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.NotificationHost, 
                    x => x.NotificationHost.ViewModel).DisposeWith(d);
            });
        }
    }
}
