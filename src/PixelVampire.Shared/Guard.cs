using System;

namespace PixelVampire.Shared
{
    /// <summary>
    /// Helper class to unify guard clauses.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Guard "Against" clauses.
        /// </summary>
        public static class Against
        {
            /// <summary>
            /// Check if the given object is null.
            /// </summary>
            /// <param name="obj">Object to check.</param>
            /// <param name="parameterName">Parameter name of the object.</param>
            public static void Null(object obj, string parameterName)
            {
                if (obj == null)
                    throw new ArgumentNullException(parameterName);
            }

            /// <summary>
            /// Check if the given string is null or empty.
            /// </summary>
            /// <param name="obj">String to check.</param>
            /// <param name="parameterName">Parameter name of the string.</param>
            public static void NullOrEmpty(string obj, string parameterName)
            {
                if (string.IsNullOrEmpty(obj))
                    throw new ArgumentException($"Parameter {parameterName} cannot be null or empty.", parameterName);
            }
        }
    }
}
