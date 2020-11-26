using System.Collections.ObjectModel;

namespace PixelVampire.Notifications.ViewModels.Abstractions
{
    public interface INotificationHostViewModel
    {
        ReadOnlyObservableCollection<NotificationViewModel> Notifications { get; }
    }
}
