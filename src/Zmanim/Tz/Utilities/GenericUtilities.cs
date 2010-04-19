using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// Various utilities that work on generics
    /// </summary>
    public static class GenericUtilities
    {
        /// <summary>
        /// Equalses the comparison.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static int EqualsComparison<T>(T x, T y)
        {
            return x != null && x.Equals(y) ? 0 : 1;
        }
    }
}
