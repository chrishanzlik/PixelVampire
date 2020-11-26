using PixelVampire.Notifications.Models;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace PixelVampire.Notifications
{
    /// <summary>
    /// Class which is managing notifications during its lifetime.
    /// </summary>
    public class Notificator : INotificationListener, INotificationPublisher
    {
        private readonly Subject<Notification> _notificationSubject;

        public Notificator()
        {
            _notificationSubject = new Subject<Notification>();
        }

        /// <inheritdoc />
        IObservable<Notification> INotificationListener.Notifications => _notificationSubject
            .AsObservable()
            .Publish()
            .RefCount()
            .Where(x => x != null);

        /// <inheritdoc />
        public void Publish(Notification notification)
        {
            _notificationSubject.OnNext(notification);
        }
    }
}
