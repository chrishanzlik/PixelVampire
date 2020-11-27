using PixelVampire.Notifications.ViewModels.Abstractions;
using ReactiveUI;

namespace PixelVampire.WPF.ViewModels.Abstractions
{
    /// <summary>
    /// The place where we host our application.
    /// </summary>
    public interface IMainWindowViewModel
    {
        /// <summary>
        /// Gets the routing state of the application.
        /// </summary>
        RoutingState Router { get; }

        /// <summary>
        /// Gets an object which is listening for general application notifications.
        /// </summary>
        INotificationHostViewModel NotificationHost { get; }
    }
}
