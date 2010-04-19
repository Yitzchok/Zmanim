using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace PublicDomain
{
    /// <summary>
    /// Common conversion tasks such as parsing string values into various types.
    /// </summary>
    public static class ConversionUtilities
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
        /// Determines whether [is string an integer16] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an integer16] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnInteger16(string str)
        {
            Int16 trash;
            return Int16.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string an integer32] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an integer32] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnInteger32(string str)
        {
            Int32 trash;
            return Int32.TryParse(str, out trash);
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
        /// Determines whether [is string an unsigned integer any] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an unsigned integer any] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnUnsignedIntegerAny(string str)
        {
            return IsStringAnUnsignedInteger64(str);
        }

        /// <summary>
        /// Determines whether [is string an unsigned integer16] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an unsigned integer16] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnUnsignedInteger16(string str)
        {
            UInt16 trash;
            return UInt16.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string an unsigned integer32] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an unsigned integer32] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnUnsignedInteger32(string str)
        {
            UInt32 trash;
            return UInt32.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string an unsigned integer64] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an unsigned integer64] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnUnsignedInteger64(string str)
        {
            UInt64 trash;
            return UInt64.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string A double] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string A double] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringADouble(string str)
        {
            double trash;
            return double.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string A decimal] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string A decimal] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringADecimal(string str)
        {
            decimal trash;
            return decimal.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string A float] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string A float] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAFloat(string str)
        {
            float trash;
            return float.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string A char] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string A char] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAChar(string str)
        {
            char trash;
            return char.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string A boolean] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string A boolean] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringABoolean(string str)
        {
            bool trash;
            return bool.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string A byte] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string A byte] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAByte(string str)
        {
            byte trash;
            return byte.TryParse(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string A time span] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string A time span] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringATimeSpan(string str)
        {
            TimeSpan trash;
            return DateTimeUtlities.TryParseTimeSpan(str, out trash);
        }

        /// <summary>
        /// Determines whether [is string A date time] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string A date time] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringADateTime(string str)
        {
            DateTime trash;
            return DateTime.TryParse(str, out trash);
        }

        /// <summary>
        /// Parses the int. Default 0
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static int ParseInt(string str)
        {
            return ParseInt(str, 0);
        }

        /// <summary>
        /// Parses the int.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int ParseInt(string str, int defaultValue)
        {
            int result;
            if (!int.TryParse(str, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// Parses the hex.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static int ParseHex(string str)
        {
            return ParseHex(str, 0);
        }

        /// <summary>
        /// Parses the hex.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int ParseHex(string str, int defaultValue)
        {
            int result;

            if (str != null && str.Length > 0)
            {
                if (str[0] == 'x' || str[0] == 'X')
                {
                    str = str.Substring(1);
                }
                else if (str.Length > 1 && str[0] == '0' && (str[1] == 'x' || str[1] == 'X'))
                {
                    str = str.Substring(2);
                }
            }

            try
            {
                result = int.Parse(str, NumberStyles.HexNumber);
            }
            catch (Exception)
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Parses the short. Default 0
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static short ParseShort(string str)
        {
            return ParseShort(str, 0);
        }

        /// <summary>
        /// Parses the short.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static short ParseShort(string str, short defaultValue)
        {
            short result;
            if (!short.TryParse(str, out result))
            {
                result = defaultValue;
            }
            return result;
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

        /// <summary>
        /// Parses the float. Default 0
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static float ParseFloat(string str)
        {
            return ParseFloat(str, 0);
        }

        /// <summary>
        /// Parses the float.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static float ParseFloat(string str, float defaultValue)
        {
            float result;
            if (!float.TryParse(str, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// Parses the double. Default 0
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static double ParseDouble(string str)
        {
            return ParseDouble(str, 0);
        }

        /// <summary>
        /// Parses the double.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static double ParseDouble(string str, double defaultValue)
        {
            double result;
            if (!double.TryParse(str, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// Parses the decimal. Default 0
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static decimal ParseDecimal(string str)
        {
            return ParseDecimal(str, 0);
        }

        /// <summary>
        /// Parses the decimal.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static decimal ParseDecimal(string str, decimal defaultValue)
        {
            decimal result;
            if (!decimal.TryParse(str, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// Parses the U int. Default 0
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static uint ParseUInt(string str)
        {
            return ParseUInt(str, 0);
        }

        /// <summary>
        /// Parses the U int.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static uint ParseUInt(string str, uint defaultValue)
        {
            uint result;
            if (!uint.TryParse(str, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// Parses the U short. Default 0
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static ushort ParseUShort(string str)
        {
            return ParseUShort(str, 0);
        }

        /// <summary>
        /// Parses the U short.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defualtValue">The defualt value.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static ushort ParseUShort(string str, ushort defualtValue)
        {
            ushort result;
            ushort.TryParse(str, out result);
            return result;
        }

        /// <summary>
        /// Parses the U long. Default 0
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static ulong ParseULong(string str)
        {
            return ParseULong(str, 0);
        }

        /// <summary>
        /// Parses the U long.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static ulong ParseULong(string str, ulong defaultValue)
        {
            ulong result;
            if (!ulong.TryParse(str, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// Parses the bool. Default false
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static bool ParseBool(string str)
        {
            return ParseBool(str, false);
        }

        /// <summary>
        /// Parses the bool.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public static bool ParseBool(string str, bool defaultValue)
        {
            bool result;
            if (!bool.TryParse(str, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// Parses the byte. Default 0
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static byte ParseByte(string str)
        {
            return ParseByte(str, 0);
        }

        /// <summary>
        /// Parses the byte.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static byte ParseByte(string str, byte defaultValue)
        {
            byte result;
            if (!byte.TryParse(str, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// Parses the char. Default 0
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static char ParseChar(string str)
        {
            return ParseChar(str, (char)0);
        }

        /// <summary>
        /// Parses the char.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static char ParseChar(string str, char defaultValue)
        {
            char result;
            if (!char.TryParse(str, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// Parses the URI. Default null
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static Uri ParseUri(string str)
        {
            return ParseUri(str, null);
        }

        /// <summary>
        /// Parses the URI.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static Uri ParseUri(string str, Uri defaultValue)
        {
            Uri result;
            if (!Uri.TryCreate(str, UriKind.RelativeOrAbsolute, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// Tries the parse URI.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static bool TryParseUri(string str, out Uri uri)
        {
            uri = null;
            try
            {
                uri = ParseUri(str);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// Parses the version. Default null
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static Version ParseVersion(string str)
        {
            return ParseVersion(str, null);
        }

        /// <summary>
        /// Parses the version.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="defaultVersion">The default version.</param>
        /// <returns></returns>
        public static Version ParseVersion(string str, Version defaultVersion)
        {
            Version result = null;
            try
            {
                result = new Version(str);
            }
            catch (ArgumentException)
            {
                result = defaultVersion;
            }
            return result;
        }

        /// <summary>
        /// Tries the parse version.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public static bool TryParseVersion(string str, out Version version)
        {
            version = null;
            try
            {
                version = ParseVersion(str);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// Toes the binary.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <returns></returns>
        public static string ToBinary(int num)
        {
            return Convert.ToString(num, 2);
        }

        /// <summary>
        /// Toes the hexadecimal.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <returns></returns>
        public static string ToHexadecimal(int num)
        {
            return ToHexadecimal(num, false);
        }

        /// <summary>
        /// Toes the hexadecimal.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <param name="prependZeroX">if set to <c>true</c> [prepend zero X].</param>
        /// <returns></returns>
        public static string ToHexadecimal(int num, bool prependZeroX)
        {
            string result = prependZeroX ? "0x" : "";
            result += Convert.ToString(num, 16);
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="val">if set to <c>true</c> [val].</param>
        /// <returns></returns>
        public static bool BooleanLaxTryParse(string str, out bool val)
        {
            val = false;

            if (!string.IsNullOrEmpty(str))
            {
                str = str.Trim().ToLower();
                if (str == "off" || str == "false" || str == "0")
                {
                    val = false;
                    return true;
                }
                else if (str == "on" || str == "true" || str == "1")
                {
                    val = true;
                    return true;
                }
            }

            return false;
        }
    }
}
