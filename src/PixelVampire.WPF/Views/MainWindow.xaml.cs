using ReactiveUI;
using System.Reactive.Disposables;

namespace PixelVampire.WPF.Views
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
