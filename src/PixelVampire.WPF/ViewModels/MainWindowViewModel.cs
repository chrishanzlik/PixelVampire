using PixelVampire.Imaging.ViewModels;
using PixelVampire.Notifications.ViewModels;
using PixelVampire.Notifications.ViewModels.Abstractions;
using PixelVampire.ViewModels;
using PixelVampire.WPF.ViewModels.Abstractions;
using ReactiveUI;

namespace PixelVampire.WPF.ViewModels
{
    /// <summary>
    /// Applications topmost view model.
    /// </summary>
    public sealed class MainWindowViewModel : ViewModelBase, IMainWindowViewModel, IScreen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>
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
