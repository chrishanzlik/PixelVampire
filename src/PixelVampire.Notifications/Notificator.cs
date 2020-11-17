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
        private readonly Subject<Notification> notificationSubject;

        IObservable<Notification> INotificationListener.Notifications => notificationSubject
            .AsObservable()
            .Publish()
            .RefCount()
            .Where(x => x != null);

        public Notificator()
        {
            notificationSubject = new Subject<Notification>();
        }

        public void Publish(Notification notification)
        {
            notificationSubject.OnNext(notification);
        }
    }
}
