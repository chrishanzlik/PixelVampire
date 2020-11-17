using PixelVampire.Notifications;
using PixelVampire.Shared.ViewModels;
using ReactiveUI;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace PixelVampire.Notifications.ViewModels
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
