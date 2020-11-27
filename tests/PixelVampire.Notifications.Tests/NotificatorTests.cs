using Microsoft.Reactive.Testing;
using ReactiveUI.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PixelVampire.Notifications.Tests
{
    public class NotificatorTests
    {
        [Fact]
        public void Notificator_PushesNotificationsDirectlyToStream() =>
            new TestScheduler().With(scheduler =>
            {
                bool success = false;

                var sut = new Notificator();
                (sut as INotificationListener).Notifications
                    .Where(x => x.Message == "123")
                    .Subscribe(_ => success = true);

                sut.PublishInfo("123");

                scheduler.AdvanceBy(1);

                Assert.True(success);
            });

        [Fact]
        public void Notificator_DoesNotPublishNullValues() =>
            new TestScheduler().With(scheduler =>
            {
                int invokes = 0;

                var sut = new Notificator();
                (sut as INotificationListener).Notifications.Subscribe(_ => invokes++);
                sut.Publish(null);

                scheduler.AdvanceBy(1);

                Assert.True(invokes == 0);
            });
    }
}
