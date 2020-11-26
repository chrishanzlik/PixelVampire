using PixelVampire.Notifications.Models;
using System;

namespace PixelVampire.Notifications
{
    /// <summary>
    /// Listens permanently to (application-wide) occuring notifications.
    /// </summary>
    public interface INotificationListener
    {
        /// <summary>
        /// Gets a stream of occuring notifications.
        /// </summary>
        IObservable<Notification> Notifications { get; }
    }
}
