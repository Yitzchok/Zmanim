using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace PublicDomain
{
    /// <summary>
    /// Parses the tz database files.
    /// 
    /// Notes:
    /// * The zone.tab file is a mapping between ISO 3166 2-character country codes
    /// and the main ZONE for that country.
    /// * See the 'Theory' file in tzcode
    /// </summary>
    [Serializable]
    public class TzDatabase
    {
        /// <summary>
        /// -
        /// </summary>
        public const string NotApplicableValue = "-";

        /// <summary>
        /// 
        /// </summary>
        public const string TzDatabaseDirectory = @"..\..\..\..\tzdata\";

        /// <summary>
        /// 
        /// </summary>
        public const string Iso3166TabFile = TzDatabaseDirectory + @"iso3166.tab";

        /// <summary>
        /// 
        /// </summary>
        public const string ZoneTabFile = TzDatabaseDirectory + @"zone.tab";

        /// <summary>
        /// 
        /// </summary>
        public const string FactoryZoneName = "Factory";

        /// <summary>
        /// Reads the tz database from the specific <paramref name="dir"/>.
        /// All files without extensions are checked for relevant data. The
        /// directory is not recursively searched. Parameters <paramref name="rules"/>,
        /// <paramref name="zones"/>, and <paramref name="links"/> should be non-null
        /// arrays into which the database will be added.
        /// </summary>
        /// <param name="dir">The dir.</param>
        /// <param name="rules">The rules.</param>
        /// <param name="zones">The zones.</param>
        /// <param name="links">The links.</param>
        public static void ReadDatabase(string dir, List<TzRule> rules, List<TzZone> zones, List<string[]> links)
        {
            if (dir == null)
            {
                throw new ArgumentNullException("dir");
            }
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            FileInfo[] files = dirInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                // If there is no file extension, we assume
                // that it is a data file.
                if (string.IsNullOrEmpty(file.Extension))
                {
                    ReadDatabaseFile(file, rules, zones, links);
                }
            }
        }

        /// <summary>
        /// Reads the database file.
        /// 
        /// See zic.txt in tzcode
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="rules">The rules.</param>
        /// <param name="zones">The zones.</param>
        /// <param name="links">The links.</param>
        /// <exception cref="PublicDomain.TzDatabase.TzException"/>
        private static void ReadDatabaseFile(FileInfo file, List<TzRule> rules, List<TzZone> zones, List<string[]> links)
        {
            string[] lines = System.IO.File.ReadAllLines(file.FullName);
            TzZone tempZone;
            int length = lines.Length;
            for (int i = 0; i < length; i++)
            {
                string line = lines[i];
                if (line == null)
                {
                    continue;
                }

                // This line may be a continuation Zone
                if (line.Length > 0)
                {
                    // Avoid comment lines
                    string leftTrimmed = line.TrimStart();

                    if (leftTrimmed.Length > 0 && leftTrimmed[0] != '#')
                    {
                        // Non-comment line
                        if (char.IsWhiteSpace(line[0]) && zones.Count > 0)
                        {
                            if (line.Trim() != string.Empty)
                            {
                                // This is a continuation of a previous Zone
                                TzZone previousZone = zones[zones.Count - 1];
                                zones.Add(TzDatabase.CloneDataZone(previousZone, line));
                            }
                        }
                        else
                        {
                            string[] pieces = StringUtilities.SplitQuoteSensitive(line, true, '\"', '#');
                            if (pieces.Length > 0)
                            {
                                switch (pieces[0].ToLower())
                                {
                                    case "rule":
                                        rules.Add(TzDatabase.ParseDataRule(line));
                                        break;
                                    case "zone":
                                        tempZone = TzDatabase.ParseDataZone(line);
                                        if (tempZone.ZoneName != FactoryZoneName)
                                        {
                                            zones.Add(tempZone);
                                        }
                                        break;
                                    case "link":
                                        links.Add(StringUtilities.RemoveEmptyPieces(pieces));
                                        break;
                                    case "leap":
                                        // Not yet handled
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static TzZone FindTzDataZone(List<TzZone> zones, string zoneName)
        {
            foreach (TzZone zone in zones)
            {
                if (zone.ZoneName.Equals(zoneName))
                {
                    return zone;
                }
            }
            throw new TzParseException("Could not find LINKed zone {0}", zoneName);
        }

        /// <summary>
        /// Parses the tz database iso3166.tab file and returns a map
        /// which maps the ISO 3166 two letter country code to the
        /// country name.
        /// </summary>
        /// <param name="iso3166TabFile">The iso3166 tab file.</param>
        /// <returns></returns>
        public static Dictionary<string, Iso3166> ParseIso3166Tab(string iso3166TabFile)
        {
            Dictionary<string, Iso3166> map = new Dictionary<string, Iso3166>();
            string[] lines = System.IO.File.ReadAllLines(iso3166TabFile);
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line) && line[0] != '#')
                {
                    // We expect two characters for the country code,
                    // followed by the country name
                    Iso3166 iso = new Iso3166(line.Substring(0, 2), line.Substring(2).Trim());
                    map[iso.TwoLetterCode] = iso;
                }
            }
            return map;
        }

        /// <summary>
        /// Parses the tz database zone.tab file into all the zone descriptions.
        /// 
        /// From 'Theory' file:
        /// "The file 'zone.tab' lists the geographical locations used to name
        /// time zone rule files.  It is intended to be an exhaustive list
        /// of canonical names for geographic regions."
        /// </summary>
        /// <param name="tabFile"></param>
        /// <returns></returns>
        public static List<PublicDomain.TzTimeZone.TzZoneDescription> ParseZoneTab(string tabFile)
        {
            List<PublicDomain.TzTimeZone.TzZoneDescription> result = new List<PublicDomain.TzTimeZone.TzZoneDescription>();
            string[] lines = System.IO.File.ReadAllLines(tabFile);
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line) && line[0] != '#')
                {
                    string[] pieces = line.Split('\t');
                    result.Add(new PublicDomain.TzTimeZone.TzZoneDescription(pieces[0], Iso6709.Parse(pieces[1]), pieces[2], pieces.Length > 3 ? pieces[3] : null));
                }
            }
            return result;
        }

        /// <summary>
        /// Logical representation of a RULE field in the tz database.
        /// </summary>
        [Serializable]
        public class TzRule : IComparable<TzRule>
        {
            /// <summary>
            /// 
            /// </summary>
            public const string ModifierDaylight = "D";

            /// <summary>
            /// 
            /// </summary>
            public const string ModifierStandard = "S";

            /// <summary>
            /// 
            /// </summary>
            public const string ModifierWar = "W";

            /// <summary>
            /// 
            /// </summary>
            public const string ModifierPeace = "P";

            private string m_ruleName;
            private int m_fromYear;
            private int m_toYear;
            private Month m_startMonth;
            private int m_startDay = -1;
            private DayOfWeek? m_startDay_DayOfWeek;
            private TimeSpan m_startTime;
            private TimeModifier m_startTimeModifier;
            private TimeSpan m_saveTime;
            private string m_modifier;
            private string m_comment;

            /// <summary>
            /// Initializes a new instance of the <see cref="PublicDomain.TzDatabase.TzRule"/> class.
            /// </summary>
            public TzRule()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="PublicDomain.TzDatabase.TzRule"/> class.
            /// </summary>
            /// <param name="ruleName">Name of the rule.</param>
            /// <param name="fromYear">From.</param>
            /// <param name="toYear">To.</param>
            /// <param name="startMonth">The start month.</param>
            /// <param name="startDay">The start day.</param>
            /// <param name="startDay_dayOfWeek">The start day_day of week.</param>
            /// <param name="startTime">The start time.</param>
            /// <param name="startTimeModifier">The start time modifier.</param>
            /// <param name="saveTime">The save time.</param>
            /// <param name="modifier">The modifier.</param>
            /// <param name="comment">The comment.</param>
            public TzRule(string ruleName, int fromYear, int toYear, Month startMonth, int startDay,
                DayOfWeek? startDay_dayOfWeek, TimeSpan startTime, TimeModifier startTimeModifier,
                TimeSpan saveTime, string modifier, string comment)
            {
                m_ruleName = ruleName;
                m_fromYear = fromYear;
                m_toYear = toYear;
                m_startMonth = startMonth;
                m_startDay = startDay;
                m_startDay_DayOfWeek = startDay_dayOfWeek;
                m_startTime = startTime;
                m_startTimeModifier = startTimeModifier;
                m_saveTime = saveTime;
                m_modifier = modifier;
                m_comment = PrepareComment(comment);
            }

            /// <summary>
            /// Gives the (arbitrary) name of the set of rules this
            /// rule is part of.
            /// </summary>
            public string RuleName
            {
                get
                {
                    return m_ruleName;
                }
            }

            /// <summary>
            /// Gives the first year in which the rule applies.  Any
            /// integer year can be supplied; the Gregorian calendar
            /// is assumed.  The word minimum (or an abbreviation)
            /// means the minimum year representable as an integer.
            /// The word maximum (or an abbreviation) means the
            /// maximum year representable as an integer.  Rules can
            /// describe times that are not representable as time
            /// values, with the unrepresentable times ignored; this
            /// allows rules to be portable among hosts with
            /// differing time value types.
            /// </summary>
            public int FromYear
            {
                get
                {
                    return m_fromYear;
                }
            }

            /// <summary>
            /// Gives the final year in which the rule applies.  In
            /// addition to minimum and maximum (as above), the word
            /// only (or an abbreviation) may be used to repeat the
            /// value of the FROM field.
            /// </summary>
            public int ToYear
            {
                get
                {
                    return m_toYear;
                }
            }

            /// <summary>
            /// Names the month in which the rule takes effect.
            /// January = 1, February = 2, ..., December = 12
            /// </summary>
            public Month StartMonth
            {
                get
                {
                    return m_startMonth;
                }
            }

            /// <summary>
            /// Gives the day on which the rule takes effect.
            /// Recognized forms include:
            /// 
            ///      5        the fifth of the month
            ///      lastSun  the last Sunday in the month
            ///      lastMon  the last Monday in the month
            ///      Sun&gt;=8   first Sunday on or after the eighth
            ///      Sun&lt;=25  last Sunday on or before the 25th
            /// </summary>
            public int StartDay
            {
                get
                {
                    return m_startDay;
                }
            }

            /// <summary>
            /// Gives the day on which the rule takes effect.
            /// Recognized forms include:
            /// 
            ///      5        the fifth of the month
            ///      lastSun  the last Sunday in the month
            ///      lastMon  the last Monday in the month
            ///      Sun&gt;=8   first Sunday on or after the eighth
            ///      Sun&lt;=25  last Sunday on or before the 25th
            /// </summary>
            public DayOfWeek? StartDay_DayOfWeek
            {
                get
                {
                    return m_startDay_DayOfWeek;
                }
            }

            /// <summary>
            /// Gives the time of day at which the rule takes
            /// effect.  Recognized forms include:
            /// 
            ///      2        time in hours
            ///      2:00     time in hours and minutes
            ///      15:00    24-hour format time (for times after noon)
            ///      1:28:14  time in hours, minutes, and seconds
            ///      -        equivalent to 0
            /// 
            ///  where hour 0 is midnight at the start of the day,
            ///  and hour 24 is midnight at the end of the day.
            /// </summary>
            public TimeSpan StartTime
            {
                get
                {
                    return m_startTime;
                }
            }

            /// <summary>
            /// In the absence of an indicator, wall clock time is assumed.
            /// </summary>
            public TimeModifier StartTimeModifier
            {
                get
                {
                    return m_startTimeModifier;
                }
            }

            /// <summary>
            /// Gives the amount of time to be added to local
            /// standard time when the rule is in effect.  This
            /// field has the same format as the AT field (although,
            /// of course, the w and s suffixes are not used).
            /// </summary>
            public TimeSpan SaveTime
            {
                get
                {
                    return m_saveTime;
                }
            }

            /// <summary>
            /// Gives the "variable part" (for example, the "S" or
            /// "D" in "EST" or "EDT") of time zone abbreviations to
            /// be used when this rule is in effect.  If this field
            /// is -, the variable part is null.
            /// </summary>
            public string Modifier
            {
                get
                {
                    return m_modifier;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public string Comment
            {
                get
                {
                    return m_comment;
                }
            }

            /// <summary>
            /// Gets the start date time.
            /// </summary>
            /// <param name="utcOffset">The utc offset.</param>
            /// <returns></returns>
            public DateTime GetFromDateTime(TimeSpan utcOffset)
            {
                return GetDateTime(FromYear, utcOffset);
            }

            /// <summary>
            /// Gets the date time.
            /// </summary>
            /// <param name="year">The year.</param>
            /// <param name="utcOffset">The utc offset.</param>
            /// <returns></returns>
            public DateTime GetDateTime(int year, TimeSpan utcOffset)
            {
                return GetDateTime(year, utcOffset, null);
            }

            /// <summary>
            /// Gets the start date time.
            /// </summary>
            /// <param name="year">The year.</param>
            /// <param name="utcOffset">The utc offset.</param>
            /// <param name="rule2">The rule2.</param>
            /// <returns></returns>
            public DateTime GetDateTime(int year, TimeSpan utcOffset, TzRule rule2)
            {
                return TzDatabase.GetDateTime(year, StartMonth, StartDay, StartDay_DayOfWeek, StartTime, StartTimeModifier, DateTimeKind.Local, utcOffset, rule2 != null ? rule2.SaveTime : SaveTime);
            }

            /// <summary>
            /// Gets the end date time.
            /// </summary>
            /// <param name="utcOffset">The utc offset.</param>
            /// <returns></returns>
            public DateTime GetToDateTime(TimeSpan utcOffset)
            {
                return GetDateTime(ToYear, utcOffset);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool DoesStartLastDay()
            {
                return StartDay_DayOfWeek != null && StartDay == -1;
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                string result = "Rule";
                result += "\t" + RuleName;
                result += "\t" + FromYear;
                result += "\t" + ToYear;
                result += "\t" + NotApplicableValue;
                result += "\t" + StartMonth.ToString().Substring(0, 3);
                result += "\t";
                if (DoesStartLastDay())
                {
                    result += "last" + this.StartDay_DayOfWeek.Value.ToString().Substring(0, 3);
                }
                else
                {
                    if (this.StartDay_DayOfWeek != null)
                    {
                        result += this.StartDay_DayOfWeek.Value.ToString().Substring(0, 3);
                        result += ">=";
                    }
                    result += this.StartDay;
                }
                result += "\t" + this.StartTime + TzDatabase.GetTimeModifierToString(StartTimeModifier);
                result += " " + this.SaveTime;
                result += "\t" + this.Modifier;
                if (!string.IsNullOrEmpty(Comment))
                {
                    result += " # " + Comment;
                }
                return result;
            }

            /// <summary>
            /// Gets the object string.
            /// </summary>
            /// <returns></returns>
            public string GetObjectString()
            {
                string result = string.Format("new PublicDomain.TzDatabase.TzRule({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})",
                    '\"' + RuleName + '\"',
                    FromYear,
                    ToYear,
                    StartMonth == 0 ? "0" : "PublicDomain.Month." + StartMonth.ToString(),
                    StartDay,
                    StartDay_DayOfWeek == null ? "null" : "DayOfWeek." + StartDay_DayOfWeek.Value.ToString(),
                    GetNewTimeSpanString(StartTime.Ticks),
                    "TzDatabase.TimeModifier." + StartTimeModifier.ToString(),
                    GetNewTimeSpanString(SaveTime.Ticks),
                    Modifier == null ? "null" : '\"' + Modifier + '\"',
                    Comment == null ? "null" : '\"' + Comment + '\"'
                );
                return result;
            }

            /// <summary>
            /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
            /// <returns>
            /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
            /// </returns>
            public override bool Equals(object obj)
            {
                TzRule c = obj as TzRule;
                if (c != null)
                {
                    return c.RuleName == RuleName &&
                        c.FromYear == FromYear &&
                        c.ToYear == ToYear &&
                        c.StartMonth == StartMonth &&
                        c.StartDay == StartDay &&
                        c.StartDay_DayOfWeek == StartDay_DayOfWeek &&
                        c.StartTime == StartTime &&
                        c.StartTimeModifier == StartTimeModifier &&
                        c.SaveTime == SaveTime;
                }
                return base.Equals(obj);
            }

            /// <summary>
            /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
            /// </summary>
            /// <returns>
            /// A hash code for the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            /// <summary>
            /// Compares the current object with another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.
            /// </returns>
            public int CompareTo(TzRule other)
            {
                if (Equals(other))
                {
                    return 0;
                }
                else
                {
                    if (GetFromDateTime(TimeSpan.Zero) > other.GetFromDateTime(TimeSpan.Zero))
                    {
                        return 1;
                    }
                    return -1;
                }
            }
        }

        /// <summary>
        /// Logical representation of a ZONE data field in the tz database.
        /// </summary>
        [Serializable]
        public class TzZone : ICloneable, IComparable<TzZone>
        {
            private string m_zoneName;
            private TimeSpan m_utcOffset;
            private string m_ruleName;
            private string m_format;
            private int m_untilYear;
            private Month m_untilMonth = 0;
            private int m_untilDay = -1;
            private DayOfWeek? m_untilDay_DayOfWeek;
            private TimeSpan m_untilTime;
            private TimeModifier m_untilTimeModifier;
            private string m_comment;

            /// <summary>
            /// Initializes a new instance of the <see cref="PublicDomain.TzDatabase.TzZone"/> class.
            /// </summary>
            public TzZone()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="PublicDomain.TzDatabase.TzZone"/> class.
            /// </summary>
            /// <param name="zoneName">Name of the zone.</param>
            /// <param name="utcOffset">The utc offset.</param>
            /// <param name="ruleName">Name of the rule.</param>
            /// <param name="format">The format.</param>
            /// <param name="untilYear">The until year.</param>
            /// <param name="untilMonth">The until month.</param>
            /// <param name="untilDay">The until day.</param>
            /// <param name="untilDay_dayOfWeek">The until day_day of week.</param>
            /// <param name="untilTime">The until time.</param>
            /// <param name="untilTimeModifier">The until time modifier.</param>
            /// <param name="comment">The comment.</param>
            public TzZone(string zoneName, TimeSpan utcOffset, string ruleName,
                string format, int untilYear, Month untilMonth, int untilDay, DayOfWeek? untilDay_dayOfWeek,
                TimeSpan untilTime, TimeModifier untilTimeModifier, string comment)
            {
                m_zoneName = zoneName;
                m_utcOffset = utcOffset;
                m_ruleName = ruleName;
                m_format = format;
                m_untilYear = untilYear;
                m_untilMonth = untilMonth;
                m_untilDay = untilDay;
                m_untilDay_DayOfWeek = untilDay_dayOfWeek;
                m_untilTime = untilTime;
                m_untilTimeModifier = untilTimeModifier;
                m_comment = PrepareComment(comment);
            }

            /// <summary>
            /// The name of the time zone.  This is the name used in
            /// creating the time conversion information file for the
            /// zone.
            /// </summary>
            public string ZoneName
            {
                get
                {
                    return m_zoneName;
                }
            }

            /// <summary>
            /// The amount of time to add to UTC to get standard time
            /// in this zone.  This field has the same format as the
            /// AT and SAVE fields of rule lines; begin the field with
            /// a minus sign if time must be subtracted from UTC.
            /// </summary>
            public TimeSpan UtcOffset
            {
                get
                {
                    return m_utcOffset;
                }
            }

            /// <summary>
            /// The name of the rule(s) that apply in the time zone
            /// or, alternately, an amount of time to add to local
            /// standard time.  If this field is - then standard time
            /// always applies in the time zone.
            /// </summary>
            public string RuleName
            {
                get
                {
                    return m_ruleName;
                }
            }

            /// <summary>
            /// The format for time zone abbreviations in this time
            /// zone.  The pair of characters %s is used to show where
            /// the "variable part" of the time zone abbreviation
            /// goes.  Alternately, a slash (/) separates standard and
            /// daylight abbreviations.
            /// </summary>
            public string Format
            {
                get
                {
                    return m_format;
                }
            }

            /// <summary>
            /// </summary>
            /// <param name="rule">The rule.</param>
            /// <returns></returns>
            public string FormatModifier(TzRule rule)
            {
                string result = Format;
                if (!string.IsNullOrEmpty(result) && result != TzDatabase.NotApplicableValue)
                {
                    string modifier = rule.Modifier;
                    if (!string.IsNullOrEmpty(modifier) && modifier != TzDatabase.NotApplicableValue)
                    {
                        result = result.Replace("%s", modifier);
                    }
                    else
                    {
                        result = result.Replace("%s", string.Empty);
                    }
                }
                return result;
            }

            /// <summary>
            /// The time at which the UTC offset or the rule(s) change
            /// for a location.  It is specified as a year, a month, a
            /// day, and a time of day.  If this is specified, the
            /// time zone information is generated from the given UTC
            /// offset and rule change until the time specified.  The
            /// month, day, and time of day have the same format as
            /// the IN, ON, and AT fields of a rule; trailing fields
            /// can be omitted, and default to the earliest possible
            /// value for the missing fields.
            /// </summary>
            public int UntilYear
            {
                get
                {
                    return m_untilYear;
                }
            }

            /// <summary>
            /// The time at which the UTC offset or the rule(s) change
            /// for a location.  It is specified as a year, a month, a
            /// day, and a time of day.  If this is specified, the
            /// time zone information is generated from the given UTC
            /// offset and rule change until the time specified.  The
            /// month, day, and time of day have the same format as
            /// the IN, ON, and AT fields of a rule; trailing fields
            /// can be omitted, and default to the earliest possible
            /// value for the missing fields.
            /// </summary>
            public Month UntilMonth
            {
                get
                {
                    return m_untilMonth;
                }
            }

            /// <summary>
            /// The time at which the UTC offset or the rule(s) change
            /// for a location.  It is specified as a year, a month, a
            /// day, and a time of day.  If this is specified, the
            /// time zone information is generated from the given UTC
            /// offset and rule change until the time specified.  The
            /// month, day, and time of day have the same format as
            /// the IN, ON, and AT fields of a rule; trailing fields
            /// can be omitted, and default to the earliest possible
            /// value for the missing fields.
            /// </summary>
            public int UntilDay
            {
                get
                {
                    return m_untilDay;
                }
            }

            /// <summary>
            /// The time at which the UTC offset or the rule(s) change
            /// for a location.  It is specified as a year, a month, a
            /// day, and a time of day.  If this is specified, the
            /// time zone information is generated from the given UTC
            /// offset and rule change until the time specified.  The
            /// month, day, and time of day have the same format as
            /// the IN, ON, and AT fields of a rule; trailing fields
            /// can be omitted, and default to the earliest possible
            /// value for the missing fields.
            /// </summary>
            public DayOfWeek? UntilDay_DayOfWeek
            {
                get
                {
                    return m_untilDay_DayOfWeek;
                }
            }

            /// <summary>
            /// The time at which the UTC offset or the rule(s) change
            /// for a location.  It is specified as a year, a month, a
            /// day, and a time of day.  If this is specified, the
            /// time zone information is generated from the given UTC
            /// offset and rule change until the time specified.  The
            /// month, day, and time of day have the same format as
            /// the IN, ON, and AT fields of a rule; trailing fields
            /// can be omitted, and default to the earliest possible
            /// value for the missing fields.
            /// </summary>
            public TimeSpan UntilTime
            {
                get
                {
                    return m_untilTime;
                }
            }

            /// <summary>
            /// </summary>
            public TimeModifier UntilTimeModifier
            {
                get
                {
                    return m_untilTimeModifier;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public string Comment
            {
                get
                {
                    return m_comment;
                }
            }

            /// <summary>
            /// Gets the until date time.
            /// </summary>
            /// <returns></returns>
            public DateTime GetUntilDateTime()
            {
                return TzDatabase.GetDateTime(UntilYear, UntilMonth, UntilDay, UntilDay_DayOfWeek, UntilTime, UntilTimeModifier, DateTimeKind.Local, UtcOffset, TimeSpan.Zero);
            }

            /// <summary>
            /// Gets the local time.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <returns></returns>
            public DateTime GetLocalTime(DateTime point)
            {
                switch (point.Kind)
                {
                    case DateTimeKind.Local:
                        return point + UtcOffset;
                    case DateTimeKind.Unspecified:
                        if (TzTimeZone.TreatUnspecifiedKindAsLocal)
                        {
                            return point + UtcOffset;
                        }
                        else
                        {
                            throw new ArgumentException("unspecified kind");
                        }
                    case DateTimeKind.Utc:
                        return new DateTime(point.Ticks, DateTimeKind.Local) + UtcOffset;
                    default:
                        throw new NotImplementedException();
                }
            }


            /// <summary>
            /// Gets the universal time.
            /// </summary>
            /// <param name="point">The point.</param>
            /// <returns></returns>
            public DateTime GetUniversalTime(DateTime point)
            {
                switch (point.Kind)
                {
                    case DateTimeKind.Local:
                        return new DateTime(point.Ticks, DateTimeKind.Utc) - UtcOffset;
                    case DateTimeKind.Unspecified:
                        if (TzTimeZone.TreatUnspecifiedKindAsLocal)
                        {
                            return new DateTime(point.Ticks, DateTimeKind.Utc) - UtcOffset;
                        }
                        else
                        {
                            throw new ArgumentException("unspecified kind");
                        }
                    case DateTimeKind.Utc:
                        return point - UtcOffset;
                    default:
                        throw new NotImplementedException();
                }
            }

            /// <summary>
            /// Determines whether [is greater than until] [the specified point].
            /// </summary>
            /// <param name="point">The point.</param>
            /// <returns>
            /// 	<c>true</c> if [is greater than until] [the specified point]; otherwise, <c>false</c>.
            /// </returns>
            public bool IsGreaterThanUntil(DateTime point)
            {
                if (point > GetUntilDateTime())
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Determines whether this instance has rules.
            /// </summary>
            /// <returns>
            /// 	<c>true</c> if this instance has rules; otherwise, <c>false</c>.
            /// </returns>
            public bool HasRules()
            {
                TimeSpan trash;
                return !string.IsNullOrEmpty(RuleName) && !RuleName.Trim().Equals(NotApplicableValue) && !DateTimeUtlities.TryParseTimeSpan(RuleName, DateTimeUtlities.TimeSpanAssumption.Hours, out trash);
            }

            /// <summary>
            /// Creates a new object that is a copy of the current instance.
            /// </summary>
            /// <returns>
            /// A new object that is a copy of this instance.
            /// </returns>
            public object Clone()
            {
                return (TzZone)MemberwiseClone();
            }

            /// <summary>
            /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
            /// <returns>
            /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
            /// </returns>
            public override bool Equals(object obj)
            {
                TzZone z = obj as TzZone;
                if (z != null)
                {
                    return z.ZoneName == ZoneName &&
                        z.RuleName == RuleName &&
                        z.UntilDay == UntilDay &&
                        z.UntilDay_DayOfWeek == UntilDay_DayOfWeek &&
                        z.UntilMonth == UntilMonth &&
                        z.UntilTime == UntilTime &&
                        z.UntilTimeModifier == UntilTimeModifier &&
                        z.UntilYear == UntilYear &&
                        z.UtcOffset == UtcOffset;
                }
                return base.Equals(obj);
            }

            /// <summary>
            /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
            /// </summary>
            /// <returns>
            /// A hash code for the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                return ZoneName;
            }

            /// <summary>
            /// Gets the object string.
            /// </summary>
            /// <returns></returns>
            public string GetObjectString()
            {
                string result = string.Format("new PublicDomain.TzDatabase.TzZone({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})",
                    '\"' + ZoneName + '\"',
                    GetNewTimeSpanString(UtcOffset.Ticks),
                    '\"' + RuleName + '\"',
                    Format == null ? "null" : '\"' + Format + '\"',
                    UntilYear,
                    UntilMonth == 0 ? "0" : "PublicDomain.Month." + UntilMonth.ToString(),
                    UntilDay,
                    UntilDay_DayOfWeek == null ? "null" : ("DayOfWeek." + UntilDay_DayOfWeek.Value.ToString()),
                    GetNewTimeSpanString(UntilTime.Ticks),
                    "TzDatabase.TimeModifier." + UntilTimeModifier.ToString(),
                    Comment == null ? "null" : '\"' + Comment + '\"'
                );

                return result;
            }

            /// <summary>
            /// Compares the current object with another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.
            /// </returns>
            public int CompareTo(TzZone other)
            {
                if (Equals(other))
                {
                    return 0;
                }
                else
                {
                    if (GetUntilDateTime() > other.GetUntilDateTime())
                    {
                        return 1;
                    }
                    return -1;
                }
            }

            internal void SetZoneName(string zoneName)
            {
                m_zoneName = zoneName;
            }

            internal void SetUtcOffset(TimeSpan utcOffset)
            {
                m_utcOffset = utcOffset;
            }

            internal void SetRuleName(string ruleName)
            {
                m_ruleName = ruleName;
            }

            internal void SetFormat(string format)
            {
                m_format = format;
            }

            internal void SetUntilYear(int untilYear)
            {
                m_untilYear = untilYear;
            }

            internal void SetUntilMonth(Month untilMonth)
            {
                m_untilMonth = untilMonth;
            }

            internal void SetUntilDay(int untilDay)
            {
                m_untilDay = untilDay;
            }

            internal void SetUntilDay_DayOfWeek(DayOfWeek? untilDay_DayOfWeek)
            {
                m_untilDay_DayOfWeek = untilDay_DayOfWeek;
            }

            internal void SetUntilTime(TimeSpan untilTime)
            {
                m_untilTime = untilTime;
            }

            internal void SetUntilTimeModifier(TimeModifier untilTimeModifier)
            {
                m_untilTimeModifier = untilTimeModifier;
            }

            internal void SetComment(string comment)
            {
                m_comment = PrepareComment(comment);
            }
        }

        /// <summary>
        /// Gets the tz data day.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="startDay">The start day.</param>
        /// <param name="startDay_dayOfWeek">The start day_day of week.</param>
        public static void GetTzDataDay(string str, out int startDay, out DayOfWeek? startDay_dayOfWeek)
        {
            startDay = 0;
            startDay_dayOfWeek = null;
            if (ConversionUtilities.IsStringAnInteger(str))
            {
                startDay = int.Parse(str);
            }
            else
            {
                if (str.Contains(">="))
                {
                    startDay_dayOfWeek = DateTimeUtlities.ParseDayOfWeek(str.Trim().Substring(0, 3));
                    startDay = int.Parse(str.Substring(str.LastIndexOf('=') + 1));
                }
                else if (str.ToLower().StartsWith("last"))
                {
                    startDay_dayOfWeek = DateTimeUtlities.ParseDayOfWeek(str.Substring("last".Length));
                    startDay = -1;
                }
            }
        }

        /// <summary>
        /// Gets the tz data time.
        /// </summary>
        /// <param name="saveTime">The save time.</param>
        /// <param name="timeModifier">The time modifier.</param>
        /// <returns></returns>
        public static TimeSpan GetTzDataTime(string saveTime, out TimeModifier timeModifier)
        {
            timeModifier = TimeModifier.LocalWallTime;
            if (char.IsLetter(saveTime[saveTime.Length - 1]))
            {
                timeModifier = ParseTimeModifier(saveTime[saveTime.Length - 1].ToString());
                saveTime = saveTime.Substring(0, saveTime.Length - 1);
            }
            return DateTimeUtlities.ParseTimeSpan(saveTime, DateTimeUtlities.TimeSpanAssumption.Hours);
        }

        /// <summary>
        /// Parses the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static TzRule ParseDataRule(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            string[] pieces = StringUtilities.SplitQuoteSensitive(str, true, '\"', '#');
            if (pieces.Length != 10 && pieces.Length != 11)
            {
                throw new TzParseException("Rule has an invalid number of pieces: {0}, expecting {1} ({2})", pieces.Length, 10, str);
            }
            string ruleName = pieces[1];
            int fromYear = 0;
            if (pieces[2] != "min")
            {
                fromYear = int.Parse(pieces[2]);
            }
            int toYear;
            switch (pieces[3])
            {
                case "only":
                    toYear = fromYear;
                    break;
                case "max":
                    toYear = int.MaxValue;
                    break;
                default:
                    toYear = int.Parse(pieces[3]);
                    break;
            }
            Month startMonth = DateTimeUtlities.ParseMonth(pieces[5]);

            int startDay;
            DayOfWeek? startDay_DayOfWeek;
            TzDatabase.GetTzDataDay(pieces[6], out startDay, out startDay_DayOfWeek);

            TimeModifier startTimeModifier;
            TimeSpan startTime = TzDatabase.GetTzDataTime(pieces[7], out startTimeModifier);
            TimeSpan saveTime = DateTimeUtlities.ParseTimeSpan(pieces[8], DateTimeUtlities.TimeSpanAssumption.Hours);
            string modifier = pieces[9];
            string comment = null;
            if (pieces.Length == 11)
            {
                comment = pieces[10].Trim();
            }
            return new TzRule(ruleName, fromYear, toYear, startMonth, startDay, startDay_DayOfWeek, startTime, startTimeModifier, saveTime, modifier, comment);
        }

        private static string PrepareComment(string comment)
        {
            if (comment != null)
            {
                comment = comment.Trim();
                if (comment.Length >= 1 && comment[0] == '#')
                {
                    comment = comment.Substring(1).Trim();
                }
            }
            return comment;
        }

        /// <summary>
        /// Parses the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static TzZone ParseDataZone(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException("str");
            }
            TzZone z = new TzZone();
            ParsePieces(str, z);
            return z;
        }

        /// <summary>
        /// Parses the pieces.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="z">The z.</param>
        private static void ParsePieces(string str, TzZone z)
        {
            string[] pieces = StringUtilities.SplitQuoteSensitive(str, true, '\"', '#');
            z.SetZoneName(pieces[1]);

            if (z.ZoneName != FactoryZoneName)
            {
                z.SetUtcOffset(DateTimeUtlities.ParseTimeSpan(pieces[2], DateTimeUtlities.TimeSpanAssumption.Hours));
                z.SetRuleName(pieces[3]);
                z.SetFormat(pieces[4]);

                z.SetComment(null);
                SetMaxZone(z);

                // The rest of the format is optional an erratic, so we combine
                // the rest of the array into a big string
                if (pieces.Length > 5)
                {
                    if (pieces[5][0] == '#')
                    {
                        z.SetComment(pieces[5].Trim());
                    }
                    else
                    {
                        z.SetUntilYear(int.Parse(pieces[5]));
                        z.SetUntilDay(1);
                        z.SetUntilMonth(Month.January);
                        z.SetUntilTime(TimeSpan.Zero);

                        if (pieces.Length > 6)
                        {
                            if (pieces[6][0] == '#')
                            {
                                z.SetComment(pieces[6].Trim());
                            }
                            else
                            {
                                z.SetUntilMonth(DateTimeUtlities.ParseMonth(pieces[6]));
                                if (pieces.Length > 7)
                                {
                                    if (pieces[7][0] == '#')
                                    {
                                        z.SetComment(pieces[7].Trim());
                                    }
                                    else
                                    {
                                        int untilDay;
                                        DayOfWeek? untilDay_DayOfWeek;
                                        TzDatabase.GetTzDataDay(pieces[7], out untilDay, out untilDay_DayOfWeek);
                                        z.SetUntilDay(untilDay);
                                        z.SetUntilDay_DayOfWeek(untilDay_DayOfWeek);
                                        if (pieces.Length > 8)
                                        {
                                            if (pieces[8][0] == '#')
                                            {
                                                z.SetComment(pieces[8].Trim());
                                            }
                                            else
                                            {
                                                TimeModifier untilTimeModifier;
                                                z.SetUntilTime(TzDatabase.GetTzDataTime(pieces[8], out untilTimeModifier));
                                                z.SetUntilTimeModifier(untilTimeModifier);
                                                if (pieces.Length > 9)
                                                {
                                                    z.SetComment(pieces[9].Trim());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void SetMaxZone(TzZone z)
        {
            // Reset any potential cloned date
            z.SetUntilYear(int.MaxValue);
            z.SetUntilMonth(Month.December);
            z.SetUntilDay(31);
            z.SetUntilDay_DayOfWeek(null);
            z.SetUntilTime(TimeSpan.Zero);
            z.SetUntilTimeModifier(TimeModifier.LocalWallTime);
        }

        /// <summary>
        /// Clones the specified line.
        /// </summary>
        /// <param name="zone">The zone.</param>
        /// <param name="line">The line.</param>
        /// <returns></returns>
        public static TzZone CloneDataZone(TzZone zone, string line)
        {
            TzZone z = (TzZone)zone.Clone();
            line = "Zone\t" + z.ZoneName + "\t" + string.Join("\t", StringUtilities.RemoveEmptyPieces(line.Split('\t')));
            ParsePieces(line, z);
            return z;
        }

        /// <summary>
        /// Thrown when there is an error interpreting the tz database.
        /// </summary>
        [Serializable]
        public class TzException : BaseException
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TzException"/> class.
            /// </summary>
            public TzException()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TzException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="formatParameters">The format parameters.</param>
            public TzException(string message, params object[] formatParameters)
                : base(message, formatParameters)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TzException"/> class.
            /// </summary>
            /// <param name="inner">The inner.</param>
            /// <param name="message">The message.</param>
            /// <param name="formatParameters">The format parameters.</param>
            public TzException(Exception inner, string message, params object[] formatParameters)
                : base(inner, message, formatParameters)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TzException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
            protected TzException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
            }
        }

        /// <summary>
        /// Thrown when there is a parse exception parsing the tz databse.
        /// </summary>
        [Serializable]
        public class TzParseException : TzException
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TzParseException"/> class.
            /// </summary>
            public TzParseException()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TzParseException"/> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="formatParameters">The format parameters.</param>
            public TzParseException(string message, params object[] formatParameters)
                : base(message, formatParameters)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TzParseException"/> class.
            /// </summary>
            /// <param name="inner">The inner.</param>
            /// <param name="message">The message.</param>
            /// <param name="formatParameters">The format parameters.</param>
            public TzParseException(Exception inner, string message, params object[] formatParameters)
                : base(message, inner, formatParameters)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TzParseException"/> class.
            /// </summary>
            /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
            /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
            /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
            /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
            protected TzParseException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
            }
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="pieceYear">The piece year.</param>
        /// <param name="pieceMonth">The piece month.</param>
        /// <param name="pieceDay">The piece day.</param>
        /// <param name="pieceDayOfWeek">The piece day of week.</param>
        /// <param name="pieceTime">The piece time.</param>
        /// <param name="timeModifier">The time modifier.</param>
        /// <param name="inflectionKind">Kind of the inflection.</param>
        /// <param name="utcOffset">The utc offset.</param>
        /// <param name="save">The save.</param>
        /// <returns></returns>
        public static DateTime GetDateTime(int pieceYear, Month pieceMonth, int pieceDay, DayOfWeek? pieceDayOfWeek, TimeSpan pieceTime, TimeModifier timeModifier, DateTimeKind inflectionKind, TimeSpan utcOffset, TimeSpan save)
        {
            int year = pieceYear;
            int month = pieceMonth == 0 ? (int)Month.January : (int)pieceMonth;
            int day = pieceDay == -1 ? 1 : pieceDay;

            if (year == int.MaxValue)
            {
                return DateTime.MaxValue;
            }
            else if (year == int.MinValue)
            {
                return DateTime.MinValue;
            }
            else
            {
                if (inflectionKind == DateTimeKind.Unspecified)
                {
                    throw new ArgumentException("inflectionKind cannot be " + DateTimeKind.Unspecified);
                }

                // Check if it is a last* day
                if (pieceDayOfWeek != null)
                {
                    if (pieceDay == -1)
                    {
                        // It's the last day of some weekday
                        DateTime untilDay = DateTimeUtlities.GetLastDay(year, month, pieceDayOfWeek.Value);
                        day = untilDay.Day;
                    }
                    else
                    {
                        // This means we're looking for the first day of the week, as
                        // specified, which is on or after the UntilDay
                        DateTime start = new DateTime(year, month, day);
                        DayOfWeek val = pieceDayOfWeek.Value;
                        for (; start.DayOfWeek != val; start = start.AddDays(1))
                        {
                        }
                        day = start.Day;
                    }
                }

                DateTime result = new DateTime(year, month, day, pieceTime.Hours, pieceTime.Minutes, pieceTime.Seconds, inflectionKind);

                ApplyTimeModifier(timeModifier, inflectionKind, ref utcOffset, ref save, ref result);

                return result;
            }
        }

        /// <summary>
        /// Applies the time modifier.
        /// </summary>
        /// <param name="timeModifier">The time modifier.</param>
        /// <param name="inflectionKind">Kind of the inflection.</param>
        /// <param name="utcOffset">The utc offset.</param>
        /// <param name="save">The save.</param>
        /// <param name="result">The result.</param>
        public static void ApplyTimeModifier(TimeModifier timeModifier, DateTimeKind inflectionKind, ref TimeSpan utcOffset, ref TimeSpan save, ref DateTime result)
        {
            switch (inflectionKind)
            {
                case DateTimeKind.Local:

                    switch (timeModifier)
                    {
                        case TimeModifier.LocalStandardTime:
                            result += save;
                            break;
                        case TimeModifier.LocalWallTime:
                            // do nothing
                            break;
                        case TimeModifier.UniversalTime:
                            result += utcOffset;
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    break;
                case DateTimeKind.Utc:

                    switch (timeModifier)
                    {
                        case TimeModifier.LocalStandardTime:
                            result -= utcOffset;
                            //result += save;
                            break;
                        case TimeModifier.LocalWallTime:
                            result -= utcOffset;
                            break;
                        case TimeModifier.UniversalTime:
                            // do nothing
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static string GetNewTimeSpanString(long val)
        {
            return val == 0 ? "TimeSpan.Zero" : "new TimeSpan(" + val + ")";
        }

        /// <summary>
        /// Gets the time modifier to string.
        /// </summary>
        /// <param name="timeModifier">The time modifier.</param>
        /// <returns></returns>
        public static string GetTimeModifierToString(TimeModifier timeModifier)
        {
            return GetTimeModifierToString(timeModifier, true);
        }

        /// <summary>
        /// Gets the time modifier to string.
        /// </summary>
        /// <param name="timeModifier">The time modifier.</param>
        /// <param name="useEmptyDefault">if set to <c>true</c> [use empty default].</param>
        /// <returns></returns>
        public static string GetTimeModifierToString(TimeModifier timeModifier, bool useEmptyDefault)
        {
            switch (timeModifier)
            {
                case TimeModifier.LocalStandardTime:
                    return "s";
                case TimeModifier.LocalWallTime:
                    return useEmptyDefault ? string.Empty : "w";
                case TimeModifier.UniversalTime:
                    return "u";
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Parses the time modifier.
        /// </summary>
        /// <param name="timeModifier">The time modifier.</param>
        /// <returns></returns>
        public static TimeModifier ParseTimeModifier(string timeModifier)
        {
            if (!string.IsNullOrEmpty(timeModifier))
            {
                timeModifier = timeModifier.Trim().ToLower();
                switch (timeModifier)
                {
                    case "w":
                        return TimeModifier.LocalWallTime;
                    case "s":
                        return TimeModifier.LocalStandardTime;
                    case "u":
                    case "g":
                    case "z":
                        return TimeModifier.UniversalTime;
                }
            }

            return TimeModifier.LocalWallTime;
        }

        /// <summary>
        /// 
        /// </summary>
        public enum TimeModifier
        {
            /// <summary>
            /// Default. Wall clock time; actual local time
            /// tz modifier: no letter or w
            /// </summary>
            LocalWallTime,

            /// <summary>
            /// Local standard time; winter time
            /// tz modifier: s
            /// </summary>
            LocalStandardTime,

            /// <summary>
            /// UTC time
            /// tz modifier: u or g or z
            /// </summary>
            UniversalTime
        }
    }
}
