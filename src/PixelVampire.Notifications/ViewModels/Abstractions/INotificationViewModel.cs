using ReactiveUI;
using System.Reactive;
using Notification = PixelVampire.Notifications.Models.Notification;

namespace PixelVampire.Notifications.ViewModels.Abstractions
{
    /// <summary>
    /// Contains state and information about a single notification.
    /// </summary>
    public interface INotificationViewModel
    {
        /// <summary>
        /// Gets the command which requests a close singal of this notification.
        /// </summary>
        ReactiveCommand<Unit, INotificationViewModel> Close { get; }

        /// <summary>
        /// Gets the notification object itself.
        /// </summary>
        Notification Notification { get; }

        /// <summary>
        /// Gets a value indicating whether the notification removes itself after a given amount of time.
        /// </summary>
        bool SelfDestructionEnabled { get; }

        /// <summary>
        /// Gets the destruction process of the notification. This property is only viable if <see cref="SelfDestructionEnabled"/> is true. 
        /// When <see cref="DestructionProcess"/> reaches 100 then the <see cref="Close"/> command will be invoked.
        /// </summary>
        int DestructionProcess { get; }
    }
}
