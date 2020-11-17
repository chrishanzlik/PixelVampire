using PixelVampire.Helpers;
using PixelVampire.Notifications;
using PixelVampire.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelVampire.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {
        public NotificationViewModel(Notification notification)
        {
            Guard.Against.Null(notification, nameof(notification));

            Notification = notification;
        }

        public Notification Notification { get; }
    }
}
