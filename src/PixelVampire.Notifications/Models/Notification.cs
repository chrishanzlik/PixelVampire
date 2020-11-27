using System;

namespace PixelVampire.Notifications.Models
{
    /// <summary>
    /// Displays information to the current application user.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notification" /> class.
        /// </summary>
        /// <param name="message">Message to display.</param>
        /// <param name="title">Notifications title.</param>
        /// <param name="level">Notification level</param>
        /// <param name="displayDuration">How long the notfiication is valid.</param>
        public Notification(string message, string title, NotificationLevel level, TimeSpan? displayDuration)
        {
            Title = title;
            Message = message;
            Level = level;
            DisplayDuration = displayDuration;
        }

        /// <summary>
        /// Gets the notification title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the notification message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the notificatin level.
        /// </summary>
        public NotificationLevel Level { get; }

        /// <summary>
        /// Gets the time when the notification was created.
        /// </summary>
        public DateTime? TimeStamp { get; }

        /// <summary>
        /// Gets the amount of time after which the notification disappears.
        /// </summary>
        public TimeSpan? DisplayDuration { get; }
    }
}
