using System;

namespace PixelVampire.Notifications
{
    public class Notification
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationLevel Level { get; set; }
        public DateTime? TimeStamp { get; set; }
        public TimeSpan? DisplayDuration { get; set; }
    }
}
