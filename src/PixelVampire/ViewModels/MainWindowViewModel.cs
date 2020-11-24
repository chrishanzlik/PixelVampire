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
    }
}
