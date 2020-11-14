using PixelVampire.Services;
using PixelVampire.Services.Abstractions;
using PixelVampire.ViewModels;
using PixelVampire.Views;
using ReactiveUI;
using Splat;
using System.Windows;

namespace PixelVampire
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Locator.CurrentMutable.Register(() => new MainWindow(), typeof(IViewFor<MainWindowViewModel>));

            Locator.CurrentMutable.Register<IImageService>(() => new SkiaImageService());
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
