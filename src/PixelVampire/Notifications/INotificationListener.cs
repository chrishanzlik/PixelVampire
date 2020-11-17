using System;

namespace PixelVampire.Notifications
{
    public interface INotificationListener
    {
        IObservable<Notification> Notifications { get; }
    }
}
