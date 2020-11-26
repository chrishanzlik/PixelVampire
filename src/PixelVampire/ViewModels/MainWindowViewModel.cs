using PixelVampire.Imaging.ViewModels;
using PixelVampire.Notifications.ViewModels;
using PixelVampire.Notifications.ViewModels.Abstractions;
using PixelVampire.Shared.ViewModels;
using PixelVampire.ViewModels.Abstractions;
using ReactiveUI;

namespace PixelVampire.ViewModels
{
    /// <summary>
    /// Applications topmost view model.
    /// </summary>
    public sealed class MainWindowViewModel : ViewModelBase, IMainWindowViewModel, IScreen
    {
        public MainWindowViewModel()
        {
            NotificationHost = new NotificationHostViewModel();
            Router = new RoutingState();
            Router.Navigate.Execute(new ImageEditorViewModel());
        }

        /// <inheritdoc />
        public RoutingState Router { get; }

        /// <inheritdoc />
        public INotificationHostViewModel NotificationHost { get; }
    }
}
