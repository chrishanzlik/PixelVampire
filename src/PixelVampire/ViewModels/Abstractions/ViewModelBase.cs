using PixelVampire.Notifications;
using ReactiveUI;
using Splat;
using System;

namespace PixelVampire.ViewModels.Abstractions
{
    [Serializable]
    public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel, IEnableNotifications, IEnableLogger
    {
        public ViewModelBase()
        {
            Activator = new ViewModelActivator();
        }

        public ViewModelActivator Activator { get; }
    }
}
