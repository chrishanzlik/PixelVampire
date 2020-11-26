using PixelVampire.Notifications.Models;

namespace PixelVampire.Notifications
{
    /// <summary>
    /// Through this interface application-wide notifications can be spawned.
    /// </summary>
    public interface INotificationPublisher
    {
        /// <summary>
        /// Spawns a new notification
        /// </summary>
        /// <param name="message">The notification object.</param>
        void Publish(Notification message);
    }
}
