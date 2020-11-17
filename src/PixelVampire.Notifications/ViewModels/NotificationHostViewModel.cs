using DynamicData;
using DynamicData.Binding;
using PixelVampire.Shared.ViewModels;
using ReactiveUI;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace PixelVampire.Notifications.ViewModels
{
    public class NotificationHostViewModel : ViewModelBase
    {
        private readonly INotificationListener notificationListener;
        private ReadOnlyObservableCollection<NotificationViewModel> notifications;

        public NotificationHostViewModel(INotificationListener notificationListener = null)
        {
            this.notificationListener = notificationListener ?? Locator.Current.GetService<INotificationListener>();

            var notificationSource = new SourceList<Notification>();
            var sourceConnection = notificationSource.Connect();

            this.WhenActivated(d =>
            {
                this.notificationListener.Notifications
                    .Subscribe(x => notificationSource.Add(x))
                    .DisposeWith(d);

                sourceConnection
                    .Transform(x => new NotificationViewModel(x))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Bind(out notifications)
                    .Subscribe()
                    .DisposeWith(d);

                Notifications.ToObservableChangeSet()
                    .Select(_ => Notifications.Select(x => x.Close).Merge())
                    .Switch()
                    .Subscribe(x => notificationSource.Remove(x.Notification))
                    .DisposeWith(d);
            });
        }

        public ReadOnlyObservableCollection<NotificationViewModel> Notifications => notifications;

    }
}
