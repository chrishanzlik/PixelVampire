using ReactiveUI;
using System.Reactive.Disposables;

namespace PixelVampire.Notifications.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für NotificationHostView.xaml
    /// </summary>
    public partial class NotificationHostView
    {
        public NotificationHostView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel,
                    x => x.Notifications,
                    x => x.NotificationContainer.ItemsSource).DisposeWith(d);
            });
        }
    }
}
