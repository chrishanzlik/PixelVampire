using PixelVampire.Imaging.WPF;
using PixelVampire.Location;
using PixelVampire.Notifications.WPF;
using PixelVampire.WPF.ViewModels;
using PixelVampire.WPF.Views;
using ReactiveUI;
using Splat;
using System.Windows;

namespace PixelVampire.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Locator.CurrentMutable.Register(() => new MainWindow(), typeof(IViewFor<MainWindowViewModel>));

            Locator.CurrentMutable.RegisterModule(new ImagingLocatorRegistrations());
            Locator.CurrentMutable.RegisterModule(new NotificationLocatorRegistrations());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow = new MainWindow()
            {
                ViewModel = new MainWindowViewModel()
            };

            MainWindow.Closed += (o, e) => Current.Shutdown();

            MainWindow.Show();
        }
    }
}
