using ReactiveUI;
using Splat;
using System;

namespace PixelVampire.Shared.ViewModels
{
    /// <summary>
    /// Most basic viewmodel to inhert from.
    /// </summary>
    [Serializable]
    public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel, IEnableNotifications, IEnableLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase" /> class.
        /// </summary>
        public ViewModelBase()
        {
            Activator = new ViewModelActivator();
        }

        /// <inheritdoc />
        public ViewModelActivator Activator { get; }
    }
}
