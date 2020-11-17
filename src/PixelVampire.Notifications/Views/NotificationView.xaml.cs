using MahApps.Metro.IconPacks;
using PixelVampire.Notifications.ViewModels;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Windows;

namespace PixelVampire.Notifications.Views
{
    /// <summary>
    /// Interaktionslogik für NotificationView.xaml
    /// </summary>
    public partial class NotificationView : ReactiveUserControl<NotificationViewModel>
    {
        public NotificationView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel,
                    x => x.Notification.Title,
                    x => x.TitleText.Text).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.Notification.Message,
                    x => x.MessageText.Text).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.DestructionProcess,
                    x => x.Duration.Value).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.Notification.Level,
                    x => x.IconWrapper.Content,
                    x => GetIcon(x)).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.SelfDestructionEnabled,
                    x => x.Duration.Visibility,
                    x => x ? Visibility.Visible : Visibility.Collapsed);

                this.BindCommand(ViewModel,
                    x => x.Close,
                    x => x.CloseButton).DisposeWith(d);
            });
        }

        private PackIconBase GetIcon(NotificationLevel level)
        {
            switch(level)
            {
                case NotificationLevel.Info: return new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.InfoCircleSolid };
                case NotificationLevel.Warning: return new PackIconOcticons() { Kind = PackIconOcticonsKind.Alert };
                case NotificationLevel.Error: return new PackIconMicrons() { Kind = PackIconMicronsKind.Fail };
                default: throw new NotImplementedException("This notification level is not implemented yet.");
            }
        }
    }
}
