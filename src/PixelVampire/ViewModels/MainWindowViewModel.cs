using PixelVampire.Imaging.ViewModels;
using PixelVampire.Notifications.ViewModels;
using PixelVampire.Shared.ViewModels;
using PixelVampire.ViewModels.Abstractions;
using ReactiveUI;

namespace PixelVampire.ViewModels
{
    public sealed class MainWindowViewModel : ViewModelBase, IMainWindowViewModel, IScreen
    {
        public MainWindowViewModel()
        {
            NotificationHost = new NotificationHostViewModel();
            Router = new RoutingState();
            Router.Navigate.Execute(new ImageEditorViewModel());
        }

        public RoutingState Router { get; }
        public NotificationHostViewModel NotificationHost { get; }
    }
}
