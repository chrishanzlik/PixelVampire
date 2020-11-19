using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace PixelVampire.Notifications
{
    public class Notificator : INotificationListener, INotificationPublisher
    {
        private readonly Subject<Notification> _notificationSubject;

        IObservable<Notification> INotificationListener.Notifications => _notificationSubject
            .AsObservable()
            .Publish()
            .RefCount()
            .Where(x => x != null);

        public Notificator()
        {
            _notificationSubject = new Subject<Notification>();
        }

        public void Publish(Notification notification)
        {
            _notificationSubject.OnNext(notification);
        }
    }
}
