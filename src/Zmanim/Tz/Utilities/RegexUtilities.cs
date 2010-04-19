using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PublicDomain
{
    /// <summary>
    /// Common regular expressions.
    /// http://www.codeproject.com/dotnet/RegexTutorial.asp
    /// </summary>
    public static class RegexUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        public const string HtmlBreakExpression = @"<\s*br\s*/?\s*>";

        /// <summary>
        /// 
        /// </summary>
        public const string HtmlParagraphExpression = @"<\s*p\s*/?\s*>";

        /// <summary>
        /// 
        /// </summary>
        public const string HtmlBreakOrParagraphExpression = @"<\s*([bp]r?)\s*/?\s*>";

        /// <summary>
        /// 
        /// </summary>
        public static Regex HtmlBreak = new Regex(HtmlBreakExpression, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        public static Regex HtmlParagraph = new Regex(HtmlParagraphExpression, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        public static Regex HtmlBreakOrParagraph = new Regex(HtmlBreakOrParagraphExpression, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        public const string HtmlBreakOrParagraphTrimLeftExpression = @"^" + HtmlBreakOrParagraphExpression;

        /// <summary>
        /// 
        /// </summary>
        public const string HtmlBreakOrParagraphTrimRightExpression = HtmlBreakOrParagraphExpression + @"$";

        /// <summary>
        /// 
        /// </summary>
        public static Regex HtmlBreakOrParagraphTrim = new Regex(string.Format("({0})|({1})", HtmlBreakOrParagraphTrimLeftExpression, HtmlBreakOrParagraphTrimRightExpression), RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        public const string UriChars = @"[^\s)<>\]}!([]+";

        /// <summary>
        /// 
        /// </summary>
        public static readonly Regex Uri = new Regex(@"\w+://" + UriChars, RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Regex UriLenient = new Regex(@"(\w+://)?" + UriChars, RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Regex Email = new Regex(@"^[\w-\.]{1,}\@([\da-zA-Z-]{1,}\.){1,}[\da-zA-Z-]{2,3}$", RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Regex NonWordDigitRegex = new Regex(@"[^a-zA-Z0-9]+", RegexOptions.Compiled);

        /// <summary>
        /// Gets the capture. Group number 0 is the entire match. Group
        /// number 1 is the first matched group from the left, and so on.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <param name="groupNumber">The group number.</param>
        /// <returns></returns>
        public static string GetCapture(Match match, int groupNumber)
        {
            if (match.Success && match.Groups.Count > groupNumber)
            {
                return match.Groups[groupNumber].ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the last capture.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public static string GetLastCapture(Match match)
        {
            return GetLastCapture(match, 0);
        }

        /// <summary>
        /// Gets the last Nth capture, specified by <paramref name="offset"/>.
        /// If <paramref name="offset"/> is 0, then this will return the last
        /// capture. If it is 1, then this will return the second-to-last
        /// capture and so on.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <param name="offset">The Nth last capture. If 0, then this will return the last
        /// capture. If it is 1, then this will return the second-to-last
        /// capture and so on.</param>
        /// <returns></returns>
        public static string GetLastCapture(Match match, int offset)
        {
            return GetCapture(match, match.Groups.Count - 1 - offset);
        }

        /// <summary>
        /// Matches any.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="regexs">The regexs.</param>
        /// <returns></returns>
        public static int MatchAny(string input, params Regex[] regexs)
        {
            Match trash;
            return MatchAny(input, out trash, regexs);
        }

        /// <summary>
        /// Matches any.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="successfulMatch">The successful match.</param>
        /// <param name="regexs">The regexs.</param>
        /// <returns></returns>
        public static int MatchAny(string input, out Match successfulMatch, params Regex[] regexs)
        {
            successfulMatch = null;
            if (regexs != null)
            {
                for (int i = 0; i < regexs.Length; i++)
                {
                    Match m = regexs[i].Match(input);
                    if (m.Success)
                    {
                        successfulMatch = m;
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Capture index number 0 is the entire match. Capture
        /// index 1 is the first matched group from the left, and so on.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="captureIndex">Capture index number 0 is the entire match. Capture
        /// index 1 is the first matched group from the left, and so on.</param>
        /// <returns></returns>
        public static string Extract(string input, string regularExpression, int captureIndex)
        {
            Match m = Regex.Match(input, regularExpression);
            if (m.Success)
            {
                return GetCapture(m, captureIndex);
            }
            return null;
        }

        /// <summary>
        /// Capture index number 0 is the entire match. Capture
        /// index 1 is the first matched group from the left, and so on.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="captureIndex">Index of the capture.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static string Extract(string input, string regularExpression, int captureIndex, RegexOptions options)
        {
            Match m = Regex.Match(input, regularExpression, options);
            if (m.Success)
            {
                return RegexUtilities.GetCapture(m, captureIndex);
            }
            return null;
        }

        /// <summary>
        /// Capture index number 0 is the entire match. Capture
        /// index 1 is the first matched group from the left, and so on.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="captureIndex">Capture index number 0 is the entire match. Capture
        /// index 1 is the first matched group from the left, and so on.
        /// </param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static string Replace(string input, string regularExpression, int captureIndex, string replacement)
        {
            return Replace(input, regularExpression, captureIndex, new StaticStringReplacer(replacement).Replace);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capturedIndex"></param>
        /// <param name="capturedValue"></param>
        /// <returns></returns>
        public delegate string CallbackRegexReplacement(int capturedIndex, string capturedValue);

        private class StaticStringReplacer
        {
            private string m_replacement;

            public StaticStringReplacer(string replacement)
            {
                m_replacement = replacement;
            }

            public string Replace(int capturedIndex, string capturedValue)
            {
                return m_replacement;
            }
        }

        /// <summary>
        /// Replaces the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="regularExpression">The regular expression.</param>
        /// <param name="captureIndex">Index of the capture.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        public static string Replace(string input, string regularExpression, int captureIndex, CallbackRegexReplacement replacement)
        {
            Match m = Regex.Match(input, regularExpression);
            StringBuilder result = new StringBuilder(input.Length);
            while (m.Success)
            {
                Group g = m.Groups[captureIndex];
                string[] pieces = StringUtilities.SplitOn(input, g.Index, false);
                result.Append(pieces[0]);
                result.Append(replacement(captureIndex, g.ToString()));
                input = pieces[1].Substring(g.Length);

                // Get the next match
                m = Regex.Match(input, regularExpression);
            }
            result.Append(input);
            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveNonWordCharacters(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = NonWordDigitRegex.Replace(str, "");
            }
            return str;
        }
    }
}
