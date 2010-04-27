using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Collections;
using System.IO;

namespace PublicDomain
{
    /// <summary>
    /// String manipulation and generation methods, as well as string array manipulation.
    /// </summary>
    internal static class StringUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        public static char[] DefaultQuoteSensitiveChars = new char[] { '\"' };

        private static Random s_random;

        /// <summary>
        /// 
        /// </summary>
        static StringUtilities()
        {
            s_random = new Random(unchecked((int)DateTime.UtcNow.Ticks));
        }

        /// <summary>
        /// Determines whether [is string null or empty with trim] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string null or empty with trim] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringNullOrEmptyWithTrim(string str)
        {
            if (str == null)
            {
                return true;
            }

            str = str.Trim();
            return str.Length == 0;
        }

        /// <summary>
        /// Joins.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="chars">The chars.</param>
        /// <returns></returns>
        public static string Join(string separator, params char[] chars)
        {
            string result = null;

            if (chars != null)
            {
                int l = chars.Length;
                for (int i = 0; i < l; i++)
                {
                    if (i > 0)
                    {
                        result += separator;
                    }
                    result += chars[i];
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the bytes from string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static byte[] GetBytesFromString(string str)
        {
            // Strings in .NET are always UTF16
            return Encoding.Unicode.GetBytes(str);
        }

        /// <summary>
        /// Gets the string from bytes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string GetStringFromBytes(byte[] data)
        {
            return Encoding.Unicode.GetString(data);
        }

        /// <summary>
        /// Returns a string of length <paramref name="size"/> filled
        /// with random ASCII characters in the range A-Z, a-z. If <paramref name="lowerCase"/>
        /// is <c>true</c>, then the range is only a-z.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="lowerCase">if set to <c>true</c> [lower case].</param>
        /// <returns></returns>
        public static string RandomString(int size, bool lowerCase)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size", "Size must be positive");
            }
            StringBuilder builder = new StringBuilder(size);
            int low = 65; // 'A'
            int high = 91; // 'Z' + 1
            if (lowerCase)
            {
                low = 97; // 'a';
                high = 123; // 'z' + 1
            }
            for (int i = 0; i < size; i++)
            {
                char ch = Convert.ToChar(s_random.Next(low, high));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Returns a string of length <paramref name="length"/> with
        /// 0's padded to the left, if necessary.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string PadIntegerLeft(int val, int length)
        {
            return PadIntegerLeft(val, length, '0');
        }

        /// <summary>
        /// Pads the integer left.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="length">The length.</param>
        /// <param name="pad">The pad.</param>
        /// <returns></returns>
        public static string PadIntegerLeft(int val, int length, char pad)
        {
            string result = val.ToString();
            while (result.Length < length)
            {
                result = pad + result;
            }
            return result;
        }

        /// <summary>
        /// Returns a string of length <paramref name="length"/> with
        /// 0's padded to the right, if necessary.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string PadIntegerRight(int val, int length)
        {
            return PadIntegerRight(val, length, '0');
        }

        /// <summary>
        /// Pads the integer right.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="length">The length.</param>
        /// <param name="pad">The pad.</param>
        /// <returns></returns>
        public static string PadIntegerRight(int val, int length, char pad)
        {
            string result = val.ToString();
            while (result.Length < length)
            {
                result += pad;
            }
            return result;
        }

        /// <summary>
        /// Replace the first occurrence of <paramref name="find"/> (case sensitive) with
        /// <paramref name="replace"/>.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="find">The find.</param>
        /// <param name="replace">The replace.</param>
        /// <returns></returns>
        public static string ReplaceFirst(string str, string find, string replace)
        {
            return ReplaceFirst(str, find, replace, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// Replace the first occurrence of <paramref name="find"/> with
        /// <paramref name="replace"/>.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="find">The find.</param>
        /// <param name="replace">The replace.</param>
        /// <param name="findComparison">The find comparison.</param>
        /// <returns></returns>
        public static string ReplaceFirst(string str, string find, string replace, StringComparison findComparison)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            else if (string.IsNullOrEmpty(find))
            {
                throw new ArgumentNullException("find");
            }
            int firstIndex = str.IndexOf(find, findComparison);
            if (firstIndex != -1)
            {
                if (firstIndex == 0)
                {
                    str = replace + str.Substring(find.Length);
                }
                else if (firstIndex == (str.Length - find.Length))
                {
                    str = str.Substring(0, firstIndex) + replace;
                }
                else
                {
                    str = str.Substring(0, firstIndex) + replace + str.Substring(firstIndex + find.Length);
                }
            }
            return str;
        }

        /// <summary>
        /// Splits <paramref name="str"/> based on finding the first location of <paramref name="ch"/>. The first element
        /// is the left portion, and the second element
        /// is the right portion. The character at index <paramref name="index"/>
        /// is not included in either portion.
        /// The return result is never null, and the elements
        /// are never null, so one of the elements may be an empty string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="ch">The character to find.</param>
        /// <returns></returns>
        public static string[] SplitAroundIndexOf(string str, char ch)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            return SplitAround(str, str.IndexOf(ch));
        }

        /// <summary>
        /// Splits <paramref name="str"/> based on finding the first location of any of the characters from
        /// <paramref name="anyOf"/>. The first element
        /// is the left portion, and the second element
        /// is the right portion. The character at index <paramref name="index"/>
        /// is not included in either portion.
        /// The return result is never null, and the elements
        /// are never null, so one of the elements may be an empty string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="anyOf">Any of.</param>
        /// <returns></returns>
        public static string[] SplitAroundIndexOfAny(string str, params char[] anyOf)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            return SplitAround(str, str.IndexOfAny(anyOf));
        }

        /// <summary>
        /// Splits <paramref name="str"/> based on finding the last location of <paramref name="ch"/>. The first element
        /// is the left portion, and the second element
        /// is the right portion. The character at index <paramref name="index"/>
        /// is not included in either portion.
        /// The return result is never null, and the elements
        /// are never null, so one of the elements may be an empty string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="ch">The character to find.</param>
        /// <returns></returns>
        public static string[] SplitAroundLastIndexOf(string str, char ch)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            return SplitAround(str, str.LastIndexOf(ch));
        }

        /// <summary>
        /// Splits <paramref name="str"/> based on finding the last location of any of the charactesr from
        /// <paramref name="anyOf"/>. The first element
        /// is the left portion, and the second element
        /// is the right portion. The character at index <paramref name="index"/>
        /// is not included in either portion.
        /// The return result is never null, and the elements
        /// are never null, so one of the elements may be an empty string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="anyOf">Any of.</param>
        /// <returns></returns>
        public static string[] SplitAroundLastIndexOfAny(string str, params char[] anyOf)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            return SplitAround(str, str.LastIndexOfAny(anyOf));
        }

        /// <summary>
        /// Splits <paramref name="str"/> based on the index. The first element
        /// is the left portion, and the second element
        /// is the right portion. The character at index <paramref name="index"/>
        /// is not included in either portion.
        /// The return result is never null, and the elements
        /// are never null, so one of the elements may be an empty string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static string[] SplitAround(string str, int index)
        {
            string one, two;
            if (index == -1)
            {
                one = "";
                two = str;
            }
            else
            {
                if (index == 0)
                {
                    one = "";
                    two = str.Substring(1);
                }
                else if (index == str.Length - 1)
                {
                    one = str.Substring(0, str.Length - 1);
                    two = "";
                }
                else
                {
                    one = str.Substring(0, index);
                    two = str.Substring(index + 1);
                }
            }

            return new string[] { one, two };
        }

        /// <summary>
        /// Splits the specified pieces.
        /// </summary>
        /// <param name="pieces">The pieces.</param>
        /// <param name="splitChar">The split char.</param>
        /// <param name="indices">The indices.</param>
        /// <returns></returns>
        public static string[] Split(string[] pieces, char splitChar, params int[] indices)
        {
            if (pieces == null)
            {
                throw new ArgumentNullException("pieces");
            }

            if (indices != null && indices.Length == 0)
            {
                indices = new int[pieces.Length];
                for (int k = 0; k < indices.Length; k++)
                {
                    indices[k] = k;
                }
            }

            // First, we need to sort the indices
            Array.Sort(indices);

            int offset = 0;
            if (indices != null)
            {
                foreach (int index in indices)
                {
                    if (index + offset < pieces.Length)
                    {
                        string[] subPieces = pieces[index + offset].Split(splitChar);
                        if (subPieces.Length > 1)
                        {
                            pieces = ArrayUtilities.InsertReplace<string>(pieces, index + offset, subPieces);
                            offset += subPieces.Length - 1;
                        }
                    }
                }
            }

            return pieces;
        }

        /// <summary>
        /// Splits the string based on whitespace, being sensitive to
        /// quotes. Always returns a non-null array, possibly zero-length.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="dividerChars">The divider chars.</param>
        /// <returns></returns>
        public static string[] SplitQuoteSensitive(string line, params char[] dividerChars)
        {
            return SplitQuoteSensitive(line, false, dividerChars);
        }

        /// <summary>
        /// Splits the string based on whitespace, being sensitive to
        /// quotes. Always returns a non-null array, possibly zero-length.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="retainDivider">if set to <c>true</c> [retain divider].</param>
        /// <param name="dividerChars">The divider chars.</param>
        /// <returns></returns>
        public static string[] SplitQuoteSensitive(string line, bool retainDivider, params char[] dividerChars)
        {
            List<string> result = new List<string>();
            if (line != null)
            {
                if (dividerChars == null || dividerChars.Length == 0)
                {
                    // no divider chars specified, use the default
                    dividerChars = DefaultQuoteSensitiveChars;
                }

                SplitQuoteSensitiveState state = SplitQuoteSensitiveState.InEther;
                int length = line.Length;
                char c;
                StringBuilder sb = new StringBuilder(length);
                char matchChar = '\0';

                for (int i = 0; i < length; i++)
                {
                    c = line[i];
                    if (char.IsWhiteSpace(c))
                    {
                        switch (state)
                        {
                            case SplitQuoteSensitiveState.InPiece:
                                // the piece has ended
                                result.Add(sb.ToString());
                                sb.Length = 0;
                                state = SplitQuoteSensitiveState.InEther;
                                break;
                            case SplitQuoteSensitiveState.InDivision:
                                // whitespace within quotes
                                sb.Append(c);
                                break;

                            // ignore:
                            //case SplitQuoteSensitiveState.InEther:
                        }
                    }
                    else if (CharUtilities.IsCharacterOneOf(c, dividerChars) && (matchChar == '\0' || matchChar == c))
                    {
                        switch (state)
                        {
                            case SplitQuoteSensitiveState.InEther:
                                state = SplitQuoteSensitiveState.InDivision;
                                matchChar = c;
                                if (retainDivider)
                                {
                                    sb.Append(c);
                                }
                                break;
                            case SplitQuoteSensitiveState.InPiece:
                                // quote in the middle of a piece
                                sb.Append(c);
                                break;
                            case SplitQuoteSensitiveState.InDivision:
                                // Finish the piece
                                result.Add(sb.ToString());
                                sb.Length = 0;
                                matchChar = '\0';
                                state = SplitQuoteSensitiveState.InEther;
                                if (retainDivider)
                                {
                                    sb.Append(c);
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (state == SplitQuoteSensitiveState.InEther)
                        {
                            state = SplitQuoteSensitiveState.InPiece;
                        }
                        sb.Append(c);
                    }
                }

                // See if there is any trailing content
                switch (state)
                {
                    case SplitQuoteSensitiveState.InPiece:
                    case SplitQuoteSensitiveState.InDivision:
                        result.Add(sb.ToString());
                        break;
                }
            }
            return result.ToArray();
        }

        private enum SplitQuoteSensitiveState
        {
            InEther,
            InPiece,
            InDivision
        }

        /// <summary>
        /// Ensures that within <paramref name="str"/> there are no two
        /// consecutive whitespace characters.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string RemoveConsecutiveWhitespace(string str)
        {
            return ReplaceConsecutiveWhitespace(str, " ");
        }

        /// <summary>
        /// Ensures that within <paramref name="str"/> there are no two
        /// consecutive whitespace characters.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static string ReplaceConsecutiveWhitespace(string str, string replacement)
        {
            return Regex.Replace(str, @"\s+", replacement, RegexOptions.Compiled);
        }

        /// <summary>
        /// Removes all characters passed in from the string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string RemoveCharacters(string str, params char[] chars)
        {
            if (chars != null)
            {
                str = Regex.Replace(str, "[" + new string(chars) + "]+", "");
            }
            return str;
        }

        /// <summary>
        /// Remove all characters that are not in the passed in array
        /// from the string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string RemoveCharactersInverse(string str, params char[] chars)
        {
            if (chars != null)
            {
                str = Regex.Replace(str, "[^" + new string(chars) + "]+", "");
            }
            return str;
        }

        /// <summary>
        /// Removes the empty pieces.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static string[] RemoveEmptyPieces(string[] array)
        {
            int index = IndexOfEmptyPiece(array);
            while (index != -1)
            {
                array = ArrayUtilities.Remove<string>(array, index);
                index = IndexOfEmptyPiece(array, index);
            }
            return array;
        }

        /// <summary>
        /// Indexes the of empty piece.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public static int IndexOfEmptyPiece(string[] array)
        {
            return IndexOfEmptyPiece(array, 0);
        }

        /// <summary>
        /// Indexes the of empty piece.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns></returns>
        public static int IndexOfEmptyPiece(string[] array, int startIndex)
        {
            for (int i = startIndex; i < array.Length; i++)
            {
                if (string.IsNullOrEmpty(array[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Splits into two pieces based on the find character.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="find">The find.</param>
        /// <param name="trim">if set to <c>true</c> [trim].</param>
        /// <param name="piece1">The piece1.</param>
        /// <param name="piece2">The piece2.</param>
        public static void FirstSplit(string str, char find, bool trim, out string piece1, out string piece2)
        {
            piece1 = piece2 = null;
            int index = str.IndexOf(find);
            if (index == -1)
            {
                piece1 = str;
            }
            else
            {
                if (index == 0)
                {
                    piece1 = "";
                    piece2 = str.Substring(index + 1);
                }
                else if (index == str.Length - 1)
                {
                    piece1 = str.Substring(0, str.Length - 1);
                    piece2 = "";
                }
                else
                {
                    piece1 = str.Substring(0, index);
                    piece2 = str.Substring(index + 1);
                }
                if (trim)
                {
                    piece1 = piece1.Trim();
                    piece2 = piece2.Trim();
                }
            }
        }

        /// <summary>
        /// Extracts the first number in the string, discarding the rest.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static int ExtractFirstNumber(string str)
        {
            int result = 0;
            bool foundDigit = false;
            foreach (char c in str)
            {
                if (char.IsDigit(c))
                {
                    result = (result * 10) + int.Parse(c.ToString());
                    foundDigit = true;
                }
                else if (foundDigit)
                {
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Searches for all matches to the <paramref name="searches"/>
        /// parameters, and returns the index which is the furthest to
        /// the end of the string. Returns -1 if none of the strings
        /// can be found.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="searches">The searches.</param>
        /// <returns></returns>
        public static int LastIndexOfAny(string str, params string[] searches)
        {
            int result = -1;
            if (searches != null)
            {
                foreach (string search in searches)
                {
                    int index = str.LastIndexOf(search);
                    if (index > result)
                    {
                        result = index;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Computes the non colliding hash
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string ComputeNonCollidingHash(string str)
        {
            byte[] data = CryptographyUtilities.ComputeSHA1Hash(str);
            return StringUtilities.GetStringFromBytes(data);
        }

        /// <summary>
        /// Splits <paramref name="str"/> based on the index. The first element
        /// is the left portion, and the second element
        /// is the right portion. The character at index <paramref name="index"/>
        /// is either included at the end of the left portion, or at the
        /// beginning of the right portion, depending on <paramref name="isIndexInFirstPortion"/>
        /// The return result is never null, and the elements
        /// are never null, so one of the elements may be an empty string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="index">The index.</param>
        /// <param name="isIndexInFirstPortion">if set to <c>true</c> [is index in first portion].</param>
        /// <returns></returns>
        public static string[] SplitOn(string str, int index, bool isIndexInFirstPortion)
        {
            string one, two;
            if (index == -1)
            {
                one = str;
                two = "";
            }
            else
            {
                if (index == 0)
                {
                    if (isIndexInFirstPortion)
                    {
                        one = str[0].ToString();
                        two = str.Substring(1);
                    }
                    else
                    {
                        one = "";
                        two = str;
                    }
                }
                else if (index == str.Length - 1)
                {
                    if (isIndexInFirstPortion)
                    {
                        one = str;
                        two = "";
                    }
                    else
                    {
                        one = str.Substring(0, str.Length - 1);
                        two = str[str.Length - 1].ToString();
                    }
                }
                else
                {
                    one = str.Substring(0, isIndexInFirstPortion ? index + 1 : index);
                    two = str.Substring(isIndexInFirstPortion ? index + 1 : index);
                }
            }

            return new string[] { one, two };
        }

        /// <summary>
        /// Formats the precision.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="precision">The precision.</param>
        /// <returns></returns>
        public static string FormatPrecision(decimal value, int precision)
        {
            return FormatPrecision(value, precision, false);
        }

        /// <summary>
        /// Formats the precision.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="precision">The precision.</param>
        /// <param name="chopOffZeros">if set to <c>true</c> [chop off zeros].</param>
        /// <returns></returns>
        public static string FormatPrecision(decimal value, int precision, bool chopOffZeros)
        {
            string result = value.ToString();
            int periodIndex = result.LastIndexOf('.');
            if (periodIndex != -1)
            {
                string decimalPortion = CutRight(result.Substring(periodIndex + 1), precision);

                if (chopOffZeros && ConversionUtilities.ParseLong(decimalPortion) == 0)
                {
                    decimalPortion = null;
                }

                result = result.Substring(0, periodIndex);

                if (decimalPortion != null)
                {
                    result += "." + decimalPortion;
                }
            }
            return result;
        }

        /// <summary>
        /// Cuts the right.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="resultingLength">Length of the resulting.</param>
        /// <returns></returns>
        public static string CutRight(string str, int resultingLength)
        {
            if (str != null && str.Length > resultingLength)
            {
                str = str.Substring(0, resultingLength);
            }
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="showDecimalPart"></param>
        /// <returns></returns>
        public static string FormatNumberWithBytes(long num, bool showDecimalPart)
        {
            return FormatNumberWithBytes(num, showDecimalPart, 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num">Bytes</param>
        /// <param name="showDecimalPart"></param>
        /// <param name="decimalPrecision"></param>
        /// <returns></returns>
        public static string FormatNumberWithBytes(long num, bool showDecimalPart, int decimalPrecision)
        {
            decimal val = num;
            string decorator = "B";
            if (num > GlobalConstants.BytesInAPetabyte)
            {
                val = (decimal)num / (decimal)GlobalConstants.BytesInAPetabyte;
                decorator = "P" + decorator;
            }
            else if (num > GlobalConstants.BytesInATerabyte)
            {
                val = (decimal)num / (decimal)GlobalConstants.BytesInATerabyte;
                decorator = "T" + decorator;
            }
            else if (num > GlobalConstants.BytesInAGigabyte)
            {
                val = (decimal)num / (decimal)GlobalConstants.BytesInAGigabyte;
                decorator = "G" + decorator;
            }
            else if (num > GlobalConstants.BytesInAMegabyte)
            {
                val = (decimal)num / (decimal)GlobalConstants.BytesInAMegabyte;
                decorator = "M" + decorator;
            }
            else if (num > GlobalConstants.BytesInAKilobyte)
            {
                val = (decimal)num / (decimal)GlobalConstants.BytesInAKilobyte;
                decorator = "K" + decorator;
            }

            string result;
            if (showDecimalPart)
            {
                result = FormatPrecision(val, decimalPrecision, true);
            }
            else
            {
                result = ((long)val).ToString();
            }
            return result + " " + decorator;
        }

        /// <summary>
        /// Calculates the M d5 sum.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string CalculateMD5Sum(string str)
        {
            return CalculateMD5Sum(str, Encoding.UTF8);
        }

        /// <summary>
        /// Calculates the M d5 sum.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string CalculateMD5Sum(string str, Encoding encoding)
        {
            using (MD5CryptoServiceProvider md5CryptoProvider = new MD5CryptoServiceProvider())
            {
                byte[] bytes = encoding.GetBytes(str);
                bytes = md5CryptoProvider.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("X2").ToLower());
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Trims the newlines.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string TrimNewlines(string str)
        {
            return TrimNewlinesRight(TrimNewlinesLeft(str));
        }

        /// <summary>
        /// Trims the newlines left.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string TrimNewlinesLeft(string str)
        {
            if (str != null)
            {
                int cutLeft = 0;
                int length = str.Length;
                char c;

                for (int i = 0; i < length; i++)
                {
                    c = str[i];
                    if (c == '\r' || c == '\n')
                    {
                        cutLeft++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (cutLeft > 0)
                {
                    str = str.Substring(cutLeft);
                }
            }
            return str;
        }

        /// <summary>
        /// Trims the newlines right.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string TrimNewlinesRight(string str)
        {
            if (str != null)
            {
                int cutRight = 0;
                int length = str.Length;
                char c;

                for (int i = length - 1; i >= 0; i--)
                {
                    c = str[i];
                    if (c == '\r' || c == '\n')
                    {
                        cutRight++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (cutRight > 0)
                {
                    str = str.Substring(0, str.Length - cutRight);
                }
            }
            return str;
        }

        /// <summary>
        /// Creates the string.
        /// </summary>
        /// <param name="repeat">The repeat.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static string CreateString(string repeat, int count)
        {
            if (string.IsNullOrEmpty(repeat) || count <= 0)
            {
                return repeat;
            }
            StringBuilder sb = new StringBuilder(repeat.Length & count);
            while (count-- > 0)
            {
                sb.Append(repeat);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Joins the specified integers.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        public static string Join(params int[] ids)
        {
            if (ids != null)
            {
                if (ids.Length > 0)
                {
                    StringBuilder result = new StringBuilder(ids.Length * 3);
                    foreach (int id in ids)
                    {
                        if (result.Length > 0)
                        {
                            result.Append(',');
                        }
                        result.Append(id);
                    }
                    return result.ToString();
                }
                return string.Empty;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string Join(IEnumerable list)
        {
            return Join(",", list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string Join(string separator, IEnumerable list)
        {
            StringBuilder sb = new StringBuilder(255);
            foreach (object o in list)
            {
                if (sb.Length > 0)
                {
                    sb.Append(separator);
                }
                if (o != null)
                {
                    sb.Append(o.ToString());
                }
                else
                {
                    sb.Append("null");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="find"></param>
        /// <returns></returns>
        public static int CountInstances(string str, char find)
        {
            return CountInstances(str, find.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="find"></param>
        /// <returns></returns>
        public static int CountInstances(string str, string find)
        {
            int result = 0;

            if (!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(find))
            {
                int i = str.IndexOf(find);
                while (i != -1)
                {
                    result++;
                    i = str.IndexOf(find, i + find.Length);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="numCharacters"></param>
        /// <returns></returns>
        public static string CutRightCharacters(string str, int numCharacters)
        {
            if (!string.IsNullOrEmpty(str) && numCharacters > 0 && numCharacters <= str.Length)
            {
                str = str.Substring(0, str.Length - numCharacters);
            }
            return str;
        }

        /// <summary>
        /// Equivalent to new Uri(uri).GetLeftPart(UriPartial.Authority), except it attempts
        /// to be more efficient. Always ends in a trailing slash.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetUriAuthority(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException("uri");
            }
            return new Uri(uri).GetLeftPart(UriPartial.Authority) + "/";
        }

        /// <summary>
        /// Returns a non-null string (but it may be an empty string).
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetPastUriAuthority(string uri)
        {
            return GetPastUriAuthority(uri, false);
        }

        /// <summary>
        /// Returns a non-null string (but it may be an empty string).
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="cutOffQuery"></param>
        /// <returns></returns>
        public static string GetPastUriAuthority(string uri, bool cutOffQuery)
        {
            string authority = GetUriAuthority(uri);
            uri = uri.Substring(authority.Length);

            if (cutOffQuery)
            {
                int qIndex = uri.IndexOf('?');
                if (qIndex != -1)
                {
                    uri = uri.Substring(0, qIndex);
                }
            }

            return uri;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public static string GetHumandReadableText(string output)
        {
            if (!string.IsNullOrEmpty(output))
            {
                output = output.Replace("\r\n", "<br />").Replace("\n", "<br />");
            }
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UppercaseWordStarts(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.ToLower();
                str = str[0].ToString().ToUpper() + str.Remove(0, 1);
                // TODO find spaces and uppercase the characters after each space
            }
            return str;
        }
    }
}
