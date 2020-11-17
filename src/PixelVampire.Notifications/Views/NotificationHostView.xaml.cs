using PixelVampire.Notifications.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PixelVampire.Notifications.Views
{
    /// <summary>
    /// Interaktionslogik für NotificationHostView.xaml
    /// </summary>
    public partial class NotificationHostView : ReactiveUserControl<NotificationHostViewModel>
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
