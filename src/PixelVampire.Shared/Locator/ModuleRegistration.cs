using Splat;
using System;

namespace PixelVampire.Shared.Locator
{
    public abstract class ModuleRegistration
    {
        public abstract void Register(IMutableDependencyResolver resolver);
    }
}
