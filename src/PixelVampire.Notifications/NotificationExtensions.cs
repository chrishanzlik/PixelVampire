using PixelVampire.Notifications.Models;
using Splat;
using System;

namespace PixelVampire.Notifications
{
    /// <summary>
    /// Utility for spawning notifications
    /// </summary>
    public static class NotificationExtensions
    {
        /// <summary>
        /// Requests the registered <see cref="INotificationPublisher"/>.
        /// </summary>
        /// <param name="_">Object to be extended.</param>
        /// <returns>An registered <see cref="INotificationPublisher"/>.</returns>
        public static INotificationPublisher Notify(this IEnableNotifications _)
        {
            return Locator.Current.GetService<INotificationPublisher>();
        }

        /// <summary>
        /// Publishes a new info notification.
        /// </summary>
        /// <param name="self">Object to be extended.</param>
        /// <param name="message">The displayed text message.</param>
        /// <param name="title">The displayed title.</param>
        /// <param name="displayDuration">Display duration. No auto-remove if null.</param>
        public static void PublishInfo(this INotificationPublisher self, string message, string title = null, TimeSpan? displayDuration = null)
        {
            self.Publish(new Notification(message, title, NotificationLevel.Info, displayDuration));
        }

        /// <summary>
        /// Publishes a new warning notification.
        /// </summary>
        /// <param name="self">Object to be extended.</param>
        /// <param name="message">The displayed text message.</param>
        /// <param name="title">The displayed title.</param>
        /// <param name="displayDuration">Display duration. No auto-remove if null.</param>
        public static void PublishWarning(this INotificationPublisher self, string message, string title = null, TimeSpan? displayDuration = null)
        {
            self.Publish(new Notification(message, title, NotificationLevel.Warning, displayDuration));
        }

        /// <summary>
        /// Publishes a new error notification.
        /// </summary>
        /// <param name="self">Object to be extended.</param>
        /// <param name="message">The displayed text message.</param>
        /// <param name="title">The displayed title.</param>
        /// <param name="displayDuration">Display duration. No auto-remove if null.</param>
        public static void PublishError(this INotificationPublisher self, string message, string title = null, TimeSpan? displayDuration = null)
        {
            self.Publish(new Notification(message, title, NotificationLevel.Error, displayDuration));
        }
    }
}
