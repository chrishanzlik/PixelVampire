using PixelVampire.Notifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PixelVampire.Notifications.Tests
{
    public class NotificationExtensionTests
    {
        [Theory]
        [InlineData("")]
        [InlineData((string)null)]
        public void NotificationExtensions_ThrowsArgumentException_WhenMessageIsNullOrEmpty(string message)
        {
            Assert.Throws<ArgumentException>(() => new TestObject().Notify().PublishInfo(message));
        }

        [Fact]
        public void NotificationExtensions_PublishInfo_SetsInfoLevelAtNotification()
        {
            bool success = false;

            var notificator = new Notificator();

            (notificator as INotificationListener).Notifications
                .Take(1)
                .Where(x => x.Level == NotificationLevel.Info)
                .Subscribe(_ => success = true);

            notificator.PublishInfo("Test");

            Assert.True(success);
        }

        [Fact]
        public void NotificationExtensions_PublishWarning_SetsWarningLevelAtNotification()
        {
            bool success = false;

            var notificator = new Notificator();

            (notificator as INotificationListener).Notifications
                .Take(1)
                .Where(x => x.Level == NotificationLevel.Warning)
                .Subscribe(_ => success = true);

            notificator.PublishWarning("Test");

            Assert.True(success);
        }

        [Fact]
        public void NotificationExtensions_PublishError_SetsErrorLevelAtNotification()
        {
            bool success = false;

            var notificator = new Notificator();

            (notificator as INotificationListener).Notifications
                .Take(1)
                .Where(x => x.Level == NotificationLevel.Error)
                .Subscribe(_ => success = true);

            notificator.PublishError("Test");

            Assert.True(success);
        }

        private class TestObject : IEnableNotifications { }
    }
}
