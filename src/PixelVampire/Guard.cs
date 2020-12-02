using System;

namespace PixelVampire
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
            public static void ArgumentNull(object obj, string parameterName)
            {
                if (obj == null)
                    throw new ArgumentNullException(parameterName);
            }

            /// <summary>
            /// Check if the given string is null or empty.
            /// </summary>
            /// <param name="obj">String to check.</param>
            /// <param name="parameterName">Parameter name of the string.</param>
            public static void ArgumentNullOrEmpty(string obj, string parameterName)
            {
                if (string.IsNullOrEmpty(obj))
                    throw new ArgumentException($"Parameter {parameterName} cannot be null or empty.", parameterName);
            }
        }

        /// <summary>
        /// Guard "Against" clauses.
        /// </summary>
        public static class Ensure
        {
            /// <summary>
            /// Ensures that the actual value is greater than the expected value.
            /// </summary>
            /// <param name="expected">Expected compare reference</param>
            /// <param name="actual">Actual compare value</param>
            /// <param name="parameterName">Name of the parameter.</param>
            public static void ArgumentGreaterThan(decimal expected, decimal actual, string parameterName)
            {
                if (actual <= expected)
                    throw new ArgumentOutOfRangeException(parameterName, $"Parameter \"{parameterName}\" must be greater than {expected}, but is actually {actual}.");
            }
        }
    }
}
