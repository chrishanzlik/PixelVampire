using System;

namespace PixelVampire.Notifications.Models
{
    /// <summary>
    /// Displays information to the current application user.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Gets or sets the notification title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the notification message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the notificatin level.
        /// </summary>
        public NotificationLevel Level { get; set; }

        /// <summary>
        /// Gets or sets the time when the notification was created.
        /// </summary>
        public DateTime? TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the amount of time after which the notification disappears.
        /// </summary>
        public TimeSpan? DisplayDuration { get; set; }
    }
}
