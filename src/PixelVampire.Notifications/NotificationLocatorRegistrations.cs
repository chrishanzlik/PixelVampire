﻿using PixelVampire.Notifications.ViewModels;
using PixelVampire.Notifications.Views;
using PixelVampire.Shared.Locator;
using ReactiveUI;
using Splat;

namespace PixelVampire.Notifications
{
    public class NotificationLocatorRegistrations : ModuleRegistration
    {
        public override void Register(IMutableDependencyResolver resolver)
        {
            resolver.Register(() => new NotificationHostView(), typeof(IViewFor<NotificationHostViewModel>));
            resolver.Register(() => new NotificationView(), typeof(IViewFor<NotificationViewModel>));

            var notificator = new Notificator();
            resolver.RegisterConstant(notificator, typeof(INotificationPublisher));
            resolver.RegisterConstant(notificator, typeof(INotificationListener));
        }
    }
}
