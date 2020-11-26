﻿using Splat;

namespace PixelVampire.Shared.Locator
{
    /// <summary>
    /// Extensions for Splats Locator.
    /// </summary>
    public static class LocatorExtensions
    {
        /// <summary>
        /// Registeres a <see cref="ModuleRegistration"/> to the <see cref="IMutableDependencyResolver"/>.
        /// </summary>
        /// <param name="self">Object to be extended.</param>
        /// <param name="registration">Registration which should be applied.</param>
        public static void RegisterModule(this IMutableDependencyResolver self, ModuleRegistration registration)
        {
            Guard.Against.Null(registration, nameof(registration));

            registration.Register(self);
        }
    }
}
