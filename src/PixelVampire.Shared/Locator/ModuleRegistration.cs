using Splat;

namespace PixelVampire.Shared.Locator
{
    /// <summary>
    /// Provides ability to register dependencies in a module friendly format.
    /// </summary>
    public abstract class ModuleRegistration
    {
        /// <summary>
        /// Within this method new module dependencies can be registered.
        /// </summary>
        /// <param name="resolver">The objects where the registration goes to.</param>
        public abstract void Register(IMutableDependencyResolver resolver);
    }
}
