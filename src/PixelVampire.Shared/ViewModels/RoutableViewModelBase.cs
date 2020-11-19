using ReactiveUI;
using Splat;

namespace PixelVampire.Shared.ViewModels
{
    public abstract class RoutableViewModelBase : ViewModelBase, IRoutableViewModel
    {
        public RoutableViewModelBase(IScreen hostScreen = null)
        {
            Guard.Against.NullOrEmpty(UrlPathSegment, nameof(UrlPathSegment));

            HostScreen = hostScreen ?? Splat.Locator.Current.GetService<IScreen>();
        }

        public abstract string UrlPathSegment { get; }

        public IScreen HostScreen { get; }
    }
}
