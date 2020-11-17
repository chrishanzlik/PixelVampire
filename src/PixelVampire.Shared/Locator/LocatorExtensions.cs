using Splat;

namespace PixelVampire.Shared.Locator
{
    public static class LocatorExtensions
    {
        public static void RegisterModule(this IMutableDependencyResolver self, ModuleRegistration registration)
        {
            Guard.Against.Null(registration, nameof(registration));

            registration.Register(self);
        }
    }
}
