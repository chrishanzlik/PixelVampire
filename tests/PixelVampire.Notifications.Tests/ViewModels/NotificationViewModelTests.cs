using Microsoft.Reactive.Testing;
using PixelVampire.Notifications.Models;
using PixelVampire.Notifications.ViewModels;
using ReactiveUI.Testing;
using System;
using Xunit;

namespace PixelVampire.Notifications.Tests.ViewModels
{
    public class NotificationViewModelTests
    {
        [Fact]
        public void NotificationViewModel_ThrowsArgumentNull_WhenNotifictionParamIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new NotificationViewModel(null));
        }

        [Fact]
        public void NotificationViewModel_SetSelfDestructionToTrue_WhenDisplayDurationIsSet()
        {
            var n = new Notification("", "", NotificationLevel.Info, TimeSpan.FromSeconds(1));
            var sut = new NotificationViewModel(n);

            Assert.True(sut.SelfDestructionEnabled);
        }

        [Fact]
        public void NotificationViewModel_SetSelfDestructionToFalse_WhenDisplayDurationIsNull()
        {
            var n = new Notification("", "", NotificationLevel.Info, null);
            var sut = new NotificationViewModel(n);

            Assert.False(sut.SelfDestructionEnabled);
        }

        [Fact]
        public void NotificationViewModel_InvokesCloseCommand_AfterTimeout() =>
            new TestScheduler().With(scheduler =>
            {
                bool invoked = false;

                var n = new Notification("", "", NotificationLevel.Info, TimeSpan.FromSeconds(1));
                var sut = new NotificationViewModel(n);
                sut.Activator.Activate();
                sut.Close.Subscribe(_ => invoked = true);

                scheduler.AdvanceBy(1);
                Assert.False(invoked);
                scheduler.AdvanceBy(TimeSpan.FromSeconds(1.5).Ticks);
                Assert.True(invoked);
            });

        [Fact]
        public void NotificationViewModel_NeverSelfInvokesCloseCommand_WhenSelfDestructionIsFalse() =>
            new TestScheduler().With(scheduler =>
            {
                bool invoked = false;

                var n = new Notification("", "", NotificationLevel.Info, null);
                var sut = new NotificationViewModel(n);

                Assert.False(sut.SelfDestructionEnabled);

                sut.Activator.Activate();
                sut.Close.Subscribe(_ => invoked = true);

                scheduler.Start();
                Assert.False(invoked);
            });

        [Fact]
        public void NotificationViewModel_DestructionProcessAdvancesOverTime_WhenSelfDestructionEnabled() =>
            new TestScheduler().With(scheduler =>
            {
                var n = new Notification("", "", NotificationLevel.Info, TimeSpan.FromSeconds(10));
                var sut = new NotificationViewModel(n);
                sut.Activator.Activate();

                Assert.Equal(0, sut.DestructionProcess);

                scheduler.AdvanceBy(TimeSpan.FromSeconds(5).Ticks);
                Assert.True(sut.DestructionProcess > 30 && sut.DestructionProcess < 70);

                scheduler.AdvanceBy(TimeSpan.FromSeconds(6).Ticks);
                Assert.Equal(100, sut.DestructionProcess);
            });
    }
}
