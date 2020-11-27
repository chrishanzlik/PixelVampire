using Microsoft.Reactive.Testing;
using PixelVampire.Notifications.ViewModels;
using ReactiveUI.Testing;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Xunit;

namespace PixelVampire.Notifications.Tests.ViewModels
{
    public class NotificationHostViewModelTests
    {
        [Fact]
        public void NotificationHostViewModel_AddsSpawnedNotificationToCollection_WhenNotNull() =>
            new TestScheduler().With(scheduler =>
            {
                var notificator = new Notificator();

                var sut = new NotificationHostViewModel(notificator);
                sut.Activator.Activate();

                notificator.PublishInfo("Hi");
                scheduler.AdvanceBy(2);

                Assert.Equal(1, sut.Notifications.Count);
            });

        [Fact]
        public void NotificationHostViewModel_RemovesNotificationFromList_AfterCloseRequested() =>
            new TestScheduler().With(scheduler =>
            {
                var notificator = new Notificator();

                var sut = new NotificationHostViewModel(notificator);
                sut.Activator.Activate();
                
                notificator.PublishInfo("Hi");

                // Pass throttle
                scheduler.AdvanceBy(TimeSpan.FromMilliseconds(100).Ticks + 1);

                var notification = sut.Notifications.First();
                notification.Close.Execute(Unit.Default).Subscribe();

                scheduler.AdvanceBy(4);

                Assert.Equal(0, sut.Notifications.Count);
            });
    }
}
