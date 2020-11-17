using PixelVampire.Notifications;
using PixelVampire.Shared;
using PixelVampire.Shared.ViewModels;

namespace PixelVampire.Notifications.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {
        public NotificationViewModel(Notification notification)
        {
            Guard.Against.Null(notification, nameof(notification));

            Notification = notification;
        }

        public Notification Notification { get; }
    }
}
