using System;

namespace PixelVampire.Helpers
{
    public static class Guard
    {
        public static class Against
        {
            public static void Null(object obj, string parameterName)
            {
                if (obj == null)
                    throw new ArgumentNullException(parameterName);
            }

            public static void NullOrEmpty(string obj, string parameterName)
            {
                if (string.IsNullOrEmpty(obj))
                    throw new ArgumentException($"Parameter {parameterName} cannot be null or empty.", parameterName);
            }
        }
    }
}
