using PixelVampire.Shared;
using Splat;
using System;

namespace PixelVampire.Notifications
{
    public static class NotificationMixins
    {
        public static INotificationPublisher Notify(this IEnableNotifications self)
        {
            return Locator.Current.GetService<INotificationPublisher>();
        }

        public static void PublishInfo(this INotificationPublisher self, string message, string title = null)
        {
            var n = GenerateNotification(NotificationLevel.Info, message, title);
            self.Publish(n);
        }

        public static void PublishWarning(this INotificationPublisher self, string message, string title = null)
        {
            var n = GenerateNotification(NotificationLevel.Warning, message, title);
            self.Publish(n);
        }

        public static void PublishError(this INotificationPublisher self, string message, string title = null)
        {
            var n = GenerateNotification(NotificationLevel.Error, message, title);
            self.Publish(n);
        }

        private static Notification GenerateNotification(NotificationLevel level, string message, string title)
        {
            return new Notification
            {
                Level = level,
                Message = message,
                Title = title,
                TimeStamp = DateTime.Now
            };
        }
    }
}
