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
                var duration = notification.DisplayDuration.Value;
                var stepWidth = 1;
                var stepOverTime = duration * stepWidth / 100;

                IObservable<int> lifeTime = Observable.Generate(
                    stepWidth,
                    x => x <= 100,
                    x => x + stepWidth,
                    x => x,
                    x => stepOverTime);

                this.WhenActivated(d =>
                {
                    lifeTime
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .ToPropertyEx(this, x => x.DestructionProcess)
                        .DisposeWith(d);
                    lifeTime
                        .Where(x => x >= 100)
                        .Select(_ => Unit.Default)
                        .InvokeCommand(Close)
                        .DisposeWith(d);
                });
            }
        }

        public Notification Notification { get; }
        public ReactiveCommand<Unit, NotificationViewModel> Close { get; }

        [ObservableAsProperty]
        public int DestructionProcess { get; }

        public bool SelfDestructionEnabled { get; }
    }
}
