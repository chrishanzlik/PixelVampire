using PixelVampire.Helpers;
using ReactiveUI;
using Splat;

namespace PixelVampire.ViewModels.Abstractions
{
    public abstract class RoutableViewModelBase : ViewModelBase, IRoutableViewModel
    {
        public RoutableViewModelBase(string urlPathSegment, IScreen hostScreen = null)
        {
            Guard.Against.NullOrEmpty(urlPathSegment, nameof(urlPathSegment));

            UrlPathSegment = urlPathSegment;
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        }

        public string UrlPathSegment { get; }

        public IScreen HostScreen { get; }
    }
}
