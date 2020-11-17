using ReactiveUI;
using Splat;
using System;

namespace PixelVampire.Shared.ViewModels
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
