using PixelVampire.Notifications.ViewModels;
using ReactiveUI;

namespace PixelVampire.ViewModels.Abstractions
{
    public interface IMainWindowViewModel
    {
        RoutingState Router { get; }
        NotificationHostViewModel NotificationHost { get; }
    }
}
