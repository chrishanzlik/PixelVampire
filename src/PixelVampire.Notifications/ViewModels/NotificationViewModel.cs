using PixelVampire.Notifications;
using PixelVampire.Shared;
using PixelVampire.Shared.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace PixelVampire.Notifications.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {
        public NotificationViewModel(Notification notification)
        {
            Guard.Against.Null(notification, nameof(notification));

            Notification = notification;
            Close = ReactiveCommand.Create(() => this);
            SelfDestructionEnabled = notification.DisplayDuration.HasValue;

            if (notification.DisplayDuration.HasValue)
            {
                var displayDuration = notification.DisplayDuration.Value;
                var decayAmount = 1;
                var decayOverTime = displayDuration * decayAmount / 100;

                IObservable<int> decayTicks = Observable.Generate(
                    decayAmount,
                    x => x <= 100,
                    x => x + decayAmount,
                    x => x,
                    x => decayOverTime);

                this.WhenActivated(d =>
                {
                    decayTicks
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .ToPropertyEx(this, x => x.DestructionProcess)
                        .DisposeWith(d);

                    decayTicks
                        .Where(x => x >= 100)
                        .Select(_ => Unit.Default)
                        .InvokeCommand(Close)
                        .DisposeWith(d);
                });
            }
        }

        public ReactiveCommand<Unit, NotificationViewModel> Close { get; }
        public Notification Notification { get; }
        public bool SelfDestructionEnabled { get; }

        [ObservableAsProperty]
        public int DestructionProcess { get; }

    }
}
