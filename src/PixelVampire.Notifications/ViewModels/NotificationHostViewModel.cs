using DynamicData;
using PixelVampire.Notifications.Models;
using PixelVampire.Notifications.ViewModels.Abstractions;
using PixelVampire.Shared.ViewModels;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace PixelVampire.Notifications.ViewModels
{
    /// <summary>
    /// Host of application notifications.
    /// </summary>
    public class NotificationHostViewModel : ViewModelBase, INotificationHostViewModel
    {
        private readonly INotificationListener _notificationListener;
        private ReadOnlyObservableCollection<NotificationViewModel> _notifications;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationHostViewModel" /> class.
        /// </summary>
        /// <param name="notificationListener">Object which is listening for occuring notifications.</param>
        public NotificationHostViewModel(INotificationListener notificationListener = null)
        {
            _notificationListener = notificationListener ?? Locator.Current.GetService<INotificationListener>();

            var notificationSource = new SourceList<Notification>();
            var notificationChanges = notificationSource.Connect();

            this.WhenActivated(d =>
            {
                _notificationListener.Notifications
                    .Subscribe(x => notificationSource.Add(x))
                    .DisposeWith(d);

                // Add new received notifications as viewmodels to our internal collection.
                notificationChanges
                    .Transform(x => new NotificationViewModel(x))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Bind(out _notifications)
                    .Subscribe()
                    .DisposeWith(d);

                // Listen for notification close requests.
                notificationChanges
                    .Throttle(TimeSpan.FromMilliseconds(100))
                    .Select(_ => Notifications.Select(x => x.Close).Merge())
                    .Switch()
                    .Subscribe(x => notificationSource.Remove(x.Notification))
                    .DisposeWith(d);
            });
        }

        /// <inheritdoc />
        public IReadOnlyCollection<INotificationViewModel> Notifications => _notifications;

    }
}
