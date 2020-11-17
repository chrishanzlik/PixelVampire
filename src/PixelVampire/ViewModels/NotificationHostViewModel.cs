using PixelVampire.Notifications;
using PixelVampire.ViewModels.Abstractions;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelVampire.ViewModels
{
    public class NotificationHostViewModel : ViewModelBase
    {
        private readonly INotificationListener notificationListener;

        public NotificationHostViewModel(INotificationListener notificationListener = null)
        {
            Notifications = new ObservableCollection<NotificationViewModel>();
            this.notificationListener = notificationListener ?? Locator.Current.GetService<INotificationListener>();

            this.WhenActivated(d =>
            {
                this.notificationListener.Notifications
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Select(x => new NotificationViewModel(x))
                    .Subscribe(x => Notifications.Add(x))
                    .DisposeWith(d);
            });
        }

        public ObservableCollection<NotificationViewModel> Notifications { get; }
    }
}
