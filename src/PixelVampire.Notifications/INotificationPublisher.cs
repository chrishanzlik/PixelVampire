using PixelVampire.Notifications.Models;

namespace PixelVampire.Notifications
{
    public interface INotificationPublisher
    {
        void Publish(Notification message);
    }
}
