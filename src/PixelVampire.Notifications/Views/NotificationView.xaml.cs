using MahApps.Metro.IconPacks;
using PixelVampire.Notifications.Models;
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
                TitleText.Text = ViewModel.Notification.Title;
                MessageText.Text = ViewModel.Notification.Message;
                IconWrapper.Content = GetIcon(ViewModel.Notification.Level);
                Duration.Visibility = ViewModel.SelfDestructionEnabled ? Visibility.Visible : Visibility.Collapsed;

                this.OneWayBind(ViewModel,
                    x => x.DestructionProcess,
                    x => x.Duration.Value).DisposeWith(d);

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
