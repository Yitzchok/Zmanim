using System;
using System.Collections.Generic;
using System.Text;
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
        /// Splits <paramref name="str"/> based on finding the last location of any of the charactesr from
        /// <paramref name="anyOf"/>. The first element
        /// is the left portion, and the second element
        /// is the right portion.
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
    }
}
