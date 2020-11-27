using PixelVampire.Notifications.ViewModels.Abstractions;
using PixelVampire.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Notification = PixelVampire.Notifications.Models.Notification;

namespace PixelVampire.Notifications.ViewModels
{
    /// <summary>
    /// Contains state and information about a single notification.
    /// </summary>
    public class NotificationViewModel : ViewModelBase, INotificationViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationViewModel" /> class.
        /// </summary>
        /// <param name="notification">The displayed notification.</param>
        /// <param name="taskPoolScheduler">Custom task pool scheduler.</param>
        public NotificationViewModel(Notification notification)
        {
            Guard.Against.ArgumentNull(notification, nameof(notification));

            Notification = notification;
            Close = ReactiveCommand.Create(() => this as INotificationViewModel);
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
                    x => decayOverTime,
                    RxApp.TaskpoolScheduler);

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

        /// <inheritdoc />
        public ReactiveCommand<Unit, INotificationViewModel> Close { get; }

        /// <inheritdoc />
        public Notification Notification { get; }
        
        /// <inheritdoc />
        public bool SelfDestructionEnabled { get; }

        /// <inheritdoc />
        [ObservableAsProperty]
        public int DestructionProcess { get; }

    }
}
