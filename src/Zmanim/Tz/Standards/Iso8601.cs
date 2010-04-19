using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PublicDomain
{
    /// <summary>
    /// http://www.w3.org/TR/NOTE-datetime
    /// http://www.cl.cam.ac.uk/~mgk25/iso-time.html
    /// </summary>
    public class Iso8601
    {
        /// <summary>
        /// 
        /// </summary>
        public const char UtcZuluIdentifier = 'Z';

        private static Regex FormatYear, FormatYearAndMonth,
            FormatComplete, FormatCompleteHM,
            FormatCompleteHMS, FormatCompleteHMSF;

        static Iso8601()
        {
            string format = @"^(\d\d\d\d)";
            string tzd = @"(Z|((\+|-)\d\d:\d\d))";

            FormatYear = new Regex(format + "$");

            format += @"-?(\d\d)";
            FormatYearAndMonth = new Regex(format + "$");

            format += @"-?(\d\d)";
            FormatComplete = new Regex(format + "$");

            format += @"T(\d\d):(\d\d)";
            FormatCompleteHM = new Regex(format + tzd + "$");

            format += @":(\d\d)";
            FormatCompleteHMS = new Regex(format + tzd + "$");

            format += @".(\d+)";
            FormatCompleteHMSF = new Regex(format + tzd + "$");
        }

        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static bool TryParse(string str, out TzDateTime dateTime)
        {
            return TryParse(str, null, out dateTime);
        }

        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="localTimeZone">The local time zone.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static bool TryParse(string str, TzTimeZone localTimeZone, out TzDateTime dateTime)
        {
            return DoParse(str, localTimeZone, out dateTime);
        }

        /// <summary>
        /// Parses the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static TzDateTime Parse(string str)
        {
            return Parse(str, null);
        }

        /// <summary>
        /// Parses the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="localTimeZone">The local time zone.</param>
        /// <returns></returns>
        public static TzDateTime Parse(string str, TzTimeZone localTimeZone)
        {
            TzDateTime result;
            if (!DoParse(str, localTimeZone, out result))
            {
                ThrowInvalidFormatException(str);
            }
            return result;
        }

        private static bool DoParse(string str, TzTimeZone localTimeZone, out TzDateTime result)
        {
            result = null;

            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }

            Match m;
            int matchIndex = RegexUtilities.MatchAny(str, out m,
                FormatYear,
                FormatYearAndMonth,
                FormatComplete,
                FormatCompleteHM,
                FormatCompleteHMS,
                FormatCompleteHMSF);
            int year = 0, month = 1, day = 1, hours = 0, minutes = 0, seconds = 0, millisecond = 0;
            DateTimeKind kind = DateTimeKind.Local;
            TzTimeZone timezone = localTimeZone;

            if (matchIndex != -1)
            {
                if (matchIndex >= 0)
                {
                    year = int.Parse(RegexUtilities.GetCapture(m, 1));
                }
                if (matchIndex >= 1)
                {
                    month = int.Parse(RegexUtilities.GetCapture(m, 2));
                }
                if (matchIndex >= 2)
                {
                    day = int.Parse(RegexUtilities.GetCapture(m, 3));
                }
                if (matchIndex >= 3)
                {
                    hours = int.Parse(RegexUtilities.GetCapture(m, 4));
                    minutes = int.Parse(RegexUtilities.GetCapture(m, 5));

                    // At this level, we also expect a time zone designator
                    kind = DateTimeKind.Utc;
                    string tzd = RegexUtilities.GetLastCapture(m, 2);
                    if (tzd == UtcZuluIdentifier.ToString())
                    {
                        timezone = TzTimeZone.ZoneUTC;
                    }
                    else
                    {
                        timezone = TzTimeZone.GetTimeZoneByOffset(RegexUtilities.GetLastCapture(m, 1));
                    }
                }
                if (matchIndex >= 4)
                {
                    seconds = int.Parse(RegexUtilities.GetCapture(m, 6));
                }
                if (matchIndex >= 5)
                {
                    millisecond = int.Parse(RegexUtilities.GetCapture(m, 7));
                }
                result = new TzDateTime(year, month, day, hours, minutes, seconds, millisecond, kind, timezone);
                return true;
            }
            return false;
        }

        private static void ThrowInvalidFormatException(string str)
        {
            throw new TzDatabase.TzParseException("Date/time does not conform to ISO 8601 format ({0}).", str);
        }

        /// <summary>
        /// Gets the time zone data.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <param name="useZuluModifier">if set to <c>true</c> [use zulu modifier].</param>
        /// <returns></returns>
        public static string GetTimeZoneData(TimeSpan timeSpan, bool useZuluModifier)
        {
            string result;
            if (useZuluModifier && timeSpan.Hours == 0 && timeSpan.Minutes == 0 && timeSpan.Seconds == 0 && timeSpan.Milliseconds == 0)
            {
                result = UtcZuluIdentifier.ToString();
            }
            else
            {
                result = (DateTimeUtlities.IsTimeSpanNegative(timeSpan) ? "-" : "+") + string.Format("{0:##}:{1:##}", timeSpan.Hours, timeSpan.Minutes);
            }
            return result;
        }
    }
}
