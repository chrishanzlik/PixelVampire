using ReactiveUI;
using Splat;

namespace PixelVampire.Shared.ViewModels
{
    /// <summary>
    /// Base viewmodel with capabilities of routing.
    /// </summary>
    public abstract class RoutableViewModelBase : ViewModelBase, IRoutableViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutableViewModelBase" /> class.
        /// </summary>
        /// <param name="hostScreen">The screen in which the view is shown.</param>
        public RoutableViewModelBase(IScreen hostScreen = null)
        {
            Guard.Against.NullOrEmpty(UrlPathSegment, nameof(UrlPathSegment));

            HostScreen = hostScreen ?? Splat.Locator.Current.GetService<IScreen>();
        }

        /// <inheritdoc />
        public abstract string UrlPathSegment { get; }

        /// <inheritdoc />
        public IScreen HostScreen { get; }
    }
}
