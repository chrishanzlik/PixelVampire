using ReactiveUI;
using System.Reactive;
using Notification = PixelVampire.Notifications.Models.Notification;

namespace PixelVampire.Notifications.ViewModels.Abstractions
{
    public interface INotificationViewModel
    {
        ReactiveCommand<Unit, NotificationViewModel> Close { get; }
        Notification Notification { get; }
        bool SelfDestructionEnabled { get; }
        int DestructionProcess { get; }
    }
}
