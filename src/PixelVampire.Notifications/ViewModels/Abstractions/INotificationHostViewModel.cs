using System.Collections.Generic;

namespace PixelVampire.Notifications.ViewModels.Abstractions
{
    /// <summary>
    /// Host of application notifications.
    /// </summary>
    public interface INotificationHostViewModel
    {
        /// <summary>
        /// Gets all present notifications wrapped insede a view model
        /// </summary>
        IReadOnlyCollection<INotificationViewModel> Notifications { get; }
    }
}
