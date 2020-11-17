using PixelVampire.Notifications;
using PixelVampire.ViewModels.Abstractions;
using ReactiveUI;
using Splat;
using System;
using System.Diagnostics;
using System.Reactive.Linq;

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
