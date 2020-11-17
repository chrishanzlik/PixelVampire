using PixelVampire.Notifications;
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
            Locator.CurrentMutable.Register(() => new ImageEditorView(), typeof(IViewFor<ImageEditorViewModel>));
            Locator.CurrentMutable.Register(() => new ImageSettingsView(), typeof(IViewFor<ImageSettingsViewModel>));
            Locator.CurrentMutable.Register(() => new ImageEditorView(), typeof(IViewFor<ImageEditorViewModel>));
            Locator.CurrentMutable.Register(() => new NotificationHostView(), typeof(IViewFor<NotificationHostViewModel>));
            Locator.CurrentMutable.Register(() => new NotificationView(), typeof(IViewFor<NotificationViewModel>));

            var notificator = new Notificator();
            Locator.CurrentMutable.RegisterConstant(notificator, typeof(INotificationPublisher));
            Locator.CurrentMutable.RegisterConstant(notificator, typeof(INotificationListener));
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
