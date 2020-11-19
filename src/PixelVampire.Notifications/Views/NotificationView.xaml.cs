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
                    x => x ? Visibility.Visible : Visibility.Collapsed).DisposeWith(d);

                this.BindCommand(ViewModel,
                    x => x.Close,
                    x => x.CloseButton).DisposeWith(d);
            });
        }

        private static PackIconBase GetIcon(NotificationLevel level) =>
            level switch
            {
                NotificationLevel.Info => new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.InfoCircleSolid },
                NotificationLevel.Warning => new PackIconOcticons() { Kind = PackIconOcticonsKind.Alert },
                NotificationLevel.Error => new PackIconMicrons() { Kind = PackIconMicronsKind.Fail },
                _ => throw new NotImplementedException("This notification level is not implemented yet."),
            };
        
    }
}
