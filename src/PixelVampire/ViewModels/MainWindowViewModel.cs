using PixelVampire.Imaging.ViewModels;
using PixelVampire.Notifications;
using PixelVampire.Notifications.ViewModels;
using PixelVampire.Shared.ViewModels;
using ReactiveUI;
using System;
using System.Reactive;

namespace PixelVampire.ViewModels
{
    public sealed class MainWindowViewModel : ViewModelBase, IScreen
    {
        public MainWindowViewModel()
        {
            NotificationHost = new NotificationHostViewModel();
            Router = new RoutingState();
            Router.Navigate.Execute(new ImageEditorViewModel());
        }

        public RoutingState Router { get; }
        public NotificationHostViewModel NotificationHost { get; }


        //Testing:
        public ReactiveCommand<Unit, Unit> SpawnTestNotification1 => ReactiveCommand.Create(() => {
            this.Notify().PublishInfo("Info detail goes here...", "Test info notification", TimeSpan.FromSeconds(3));
        });
        public ReactiveCommand<Unit, Unit> SpawnTestNotification2 => ReactiveCommand.Create(() => {
            this.Notify().PublishError("Error detail goes here...", "Test error notification", TimeSpan.FromSeconds(3));
        });
        public ReactiveCommand<Unit, Unit> SpawnTestNotification3 => ReactiveCommand.Create(() => {
            this.Notify().PublishWarning("Warning detail goes here...", "Test warning notification", TimeSpan.FromSeconds(3));
        });
    }
}
