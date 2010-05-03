using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace PublicDomain
{
    /// <summary>
    /// Common conversion tasks such as parsing string values into various types.
    /// </summary>
    internal static class ConversionUtilities
    {
        /// <summary>
        /// Determines whether [is string an integer] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an integer] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnInteger(string str)
        {
            return IsStringAnInteger64(str);
        }

        /// <summary>
        /// Determines whether [is string an integer64] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an integer64] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnInteger64(string str)
        {
            Int64 trash;
            return Int64.TryParse(str, out trash);
        }

        /// <summary>
        /// Parses the long. Default 0
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static long ParseLong(string str)
        {
            return ParseLong(str, 0);
        }

        /// <summary>
        /// Parses the long.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static long ParseLong(string str, long defaultValue)
        {
            long result;
            if (!long.TryParse(str, out result))
            {
                result = defaultValue;
            }
            return result;

        }
    }
}
