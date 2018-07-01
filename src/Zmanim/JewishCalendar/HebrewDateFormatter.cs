// * Zmanim .NET API
// * Copyright (C) 2004-2010 Eliyahu Hershfeld
// *
// * Converted to C# by AdminJew
// *
// * This file is part of Zmanim .NET API.
// *
// * Zmanim .NET API is free software: you can redistribute it and/or modify
// * it under the terms of the GNU Lesser General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * Zmanim .NET API is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU Lesser General Public License for more details.
// *
// * You should have received a copy of the GNU Lesser General Public License
// * along with Zmanim.NET API.  If not, see <http://www.gnu.org/licenses/lgpl.html>.

#if !NOHEBREWCALENDAR
using System.Text;
using System;

namespace Zmanim.JewishCalendar
{
    /// <summary>
    /// The HebrewDateFormatter class formats a <seealso cref="JewishDate"/>.
    /// 
    /// The class formats Jewish dates in Hebrew or Latin chars, and has various settings. Sample full date output includes
    /// (using various options):
    /// <ul>
    /// <li>21 Shevat, 5729</li>
    /// <li>&#x5DB;&#x5D0; &#x5E9;&#x5D1;&#x5D8; &#x5EA;&#x5E9;&#x5DB;&#x5D8;</li>
    /// <li>&#x5D4;&#x5F3; &#x5DB;&#x5F4;&#x5D0; &#x5E9;&#x5D1;&#x5D8; &#x5EA;&#x5E9;&#x5DB;&#x5F4;&#x5D8;</li>
    /// <li>&#x5DB;&#x5F4;&#x5D0; &#x5E9;&#x5D1;&#x5D8; &#x5EA;&#x5E9;&#x5DA;&#x5F3;</li>
    /// </ul>
    /// </summary>
    /// <seealso cref="JewishDate"></seealso>
    /// <seealso cref="JewishCalendar"></seealso>
    /// 
    /// @author &copy; Eliyahu Hershfeld 2011
    /// @version 0.3 </seealso>
    public class HebrewDateFormatter
    {
        private bool hebrewFormat = false;
        private bool useLonghebrewYears = false;
        private bool useGershGershayim = true;
        private bool longWeekFormat = true;

		private static JewishCalendar jewishCalendar = new JewishCalendar();

        /// <summary>
        /// returns if the <seealso cref="#formatDayOfWeek(JewishDate)"/> will use the long format such as
        /// &#x05E8;&#x05D0;&#x05E9;&#x05D5;&#x05DF; or short such as &#x05D0; when formatting the day of week in
        /// <seealso cref="isHebrewFormat() Hebrew"/>.
        /// </summary>
        /// <returns> the longWeekFormat </returns>
        /// <seealso cref="setLongWeekFormat(boolean)"></seealso>
        /// <seealso cref="formatDayOfWeek(JewishDate)"></seealso>
        public virtual bool LongWeekFormat
        {
            get
            {
                return longWeekFormat;
            }
            set
            {
                this.longWeekFormat = value;
            }
        }


        private const string GERESH = "\u05F3";
        private const string GERSHAYIM = "\u05F4";
        private string[] transliteratedMonths = { "Nissan", "Iyar", "Sivan", "Tammuz", "Av", "Elul", "Tishrei", "Cheshvan", "Kislev", "Teves", "Shevat", "Adar", "Adar II", "Adar I" };
        private string hebrewOmerPrefix = "\u05D1";

        private string transliteratedShabbosDayOfweek = "Shabbos";

        /// <summary>
        /// Returns the day of Shabbos transliterated into Latin chars. The default uses Ashkenazi pronounciation "Shabbos".
        /// This can be overwritten using the <seealso cref="#setTransliteratedShabbosDayOfWeek(String)"/>
        /// </summary>
        /// <returns> the transliteratedShabbos. The default list of months uses Ashkenazi pronounciation "Shabbos". </returns>
        /// <seealso cref="setTransliteratedShabbosDayOfWeek(String)"></seealso>
        /// <seealso cref="formatDayOfWeek(JewishDate)"></seealso>
        public virtual string TransliteratedShabbosDayOfWeek
        {
            get
            {
                return transliteratedShabbosDayOfweek;
            }
            set
            {
                this.transliteratedShabbosDayOfweek = value;
            }
        }


        private string[] transliteratedHolidays = { "Erev Pesach", "Pesach", "Chol Hamoed Pesach", "Pesach Sheni", "Erev Shavuos", "Shavuos", "Seventeenth of Tammuz", "Tishah B'Av", "Tu B'Av", "Erev Rosh Hashana", "Rosh Hashana", "Fast of Gedalyah", "Erev Yom Kippur", "Yom Kippur", "Erev Succos", "Succos", "Chol Hamoed Succos", "Hoshana Rabbah", "Shemini Atzeres", "Simchas Torah", "Erev Chanukah", "Chanukah", "Tenth of Teves", "Tu B'Shvat", "Fast of Esther", "Purim", "Shushan Purim", "Purim Katan", "Rosh Chodesh", "Yom HaShoah", "Yom Hazikaron", "Yom Ha'atzmaut", "Yom Yerushalayim" };

        /// <summary>
        /// Returns the list of holidays transliterated into Latin chars. This is used by the
        /// <seealso cref="#formatYomTov(JewishCalendar)"/> when formatting the Yom Tov String. The default list of months uses
        /// Ashkenazi pronunciation in typical American English spelling.
        /// </summary>
        /// <returns> the list of holidays "Adar", "Adar II", "Adar I". The default list is currently "Erev Pesach", "Pesach",
        ///         "Chol Hamoed Pesach", "Pesach Sheni", "Erev Shavuos", "Shavuos", "Seventeenth of Tammuz", "Tishah B'Av",
        ///         "Tu B'Av", "Erev Rosh Hashana", "Rosh Hashana", "Fast of Gedalyah", "Erev Yom Kippur", "Yom Kippur",
        ///         "Erev Succos", "Succos", "Chol Hamoed Succos", "Hoshana Rabbah", "Shemini Atzeres", "Simchas Torah",
        ///         "Erev Chanukah", "Chanukah", "Tenth of Teves", "Tu B'Shvat", "Fast of Esther", "Purim", "Shushan Purim",
        ///         "Purim Katan", "Rosh Chodesh", "Yom HaShoah", "Yom Hazikaron", "Yom Ha'atzmaut", "Yom Yerushalayim"
        /// </returns>
        /// <seealso cref="#setTransliteratedMonthList(String[])"> </seealso>
        /// <seealso cref="#formatYomTov(JewishCalendar)"> </seealso>
        /// <seealso cref="#isHebrewFormat()"> </seealso>
        public virtual string[] TransliteratedHolidayList
        {
            get
            {
                return transliteratedHolidays;
            }
            set
            {
                this.transliteratedHolidays = value;
            }
        }


        /// <summary>
        /// Hebrew holiday list
        /// </summary>
        private string[] hebrewHolidays = { "\u05E2\u05E8\u05D1 \u05E4\u05E1\u05D7", "\u05E4\u05E1\u05D7", "\u05D7\u05D5\u05DC \u05D4\u05DE\u05D5\u05E2\u05D3 \u05E4\u05E1\u05D7", "\u05E4\u05E1\u05D7 \u05E9\u05E0\u05D9", "\u05E2\u05E8\u05D1 \u05E9\u05D1\u05D5\u05E2\u05D5\u05EA", "\u05E9\u05D1\u05D5\u05E2\u05D5\u05EA", "\u05E9\u05D1\u05E2\u05D4 \u05E2\u05E9\u05E8 \u05D1\u05EA\u05DE\u05D5\u05D6", "\u05EA\u05E9\u05E2\u05D4 \u05D1\u05D0\u05D1", "\u05D8\u05F4\u05D5 \u05D1\u05D0\u05D1", "\u05E2\u05E8\u05D1 \u05E8\u05D0\u05E9 \u05D4\u05E9\u05E0\u05D4", "\u05E8\u05D0\u05E9 \u05D4\u05E9\u05E0\u05D4", "\u05E6\u05D5\u05DD \u05D2\u05D3\u05DC\u05D9\u05D4", "\u05E2\u05E8\u05D1 \u05D9\u05D5\u05DD \u05DB\u05D9\u05E4\u05D5\u05E8", "\u05D9\u05D5\u05DD \u05DB\u05D9\u05E4\u05D5\u05E8", "\u05E2\u05E8\u05D1 \u05E1\u05D5\u05DB\u05D5\u05EA", "\u05E1\u05D5\u05DB\u05D5\u05EA", "\u05D7\u05D5\u05DC \u05D4\u05DE\u05D5\u05E2\u05D3 \u05E1\u05D5\u05DB\u05D5\u05EA", "\u05D4\u05D5\u05E9\u05E2\u05E0\u05D0 \u05E8\u05D1\u05D4", "\u05E9\u05DE\u05D9\u05E0\u05D9 \u05E2\u05E6\u05E8\u05EA", "\u05E9\u05DE\u05D7\u05EA \u05EA\u05D5\u05E8\u05D4", "\u05E2\u05E8\u05D1 \u05D7\u05E0\u05D5\u05DB\u05D4", "\u05D7\u05E0\u05D5\u05DB\u05D4", "\u05E2\u05E9\u05E8\u05D4 \u05D1\u05D8\u05D1\u05EA", "\u05D8\u05F4\u05D5 \u05D1\u05E9\u05D1\u05D8", "\u05EA\u05E2\u05E0\u05D9\u05EA \u05D0\u05E1\u05EA\u05E8", "\u05E4\u05D5\u05E8\u05D9\u05DD", "\u05E4\u05D5\u05E8\u05D9\u05DD \u05E9\u05D5\u05E9\u05DF", "\u05E4\u05D5\u05E8\u05D9\u05DD \u05E7\u05D8\u05DF", "\u05E8\u05D0\u05E9 \u05D7\u05D5\u05D3\u05E9", "\u05D9\u05D5\u05DD \u05D4\u05E9\u05D5\u05D0\u05D4", "\u05D9\u05D5\u05DD \u05D4\u05D6\u05D9\u05DB\u05E8\u05D5\u05DF", "\u05D9\u05D5\u05DD \u05D4\u05E2\u05E6\u05DE\u05D0\u05D5\u05EA", "\u05D9\u05D5\u05DD \u05D9\u05E8\u05D5\u05E9\u05DC\u05D9\u05DD" };

        /// <summary>
        /// Formats the Yom Tov (holiday) in Hebrew or transliterated Latin characters.
        /// </summary>
        /// <param name="jewishCalendar"> </param>
        /// <returns> the formatted holiday or an empty String if the day is not a holiday. </returns>
        /// <seealso cref="isHebrewFormat()"></seealso>
		public virtual string FormatYomTov(DateTime dt, bool inIsrael)
        {
			JewishCalendar.JewishHoliday holiday = jewishCalendar.GetJewishHoliday (dt, inIsrael);

			int index = (int)holiday;

            if (holiday == JewishCalendar.JewishHoliday.CHANUKAH)
            {
				int dayOfChanukah = jewishCalendar.GetDayOfChanukah(dt);
                return hebrewFormat ? (FormatHebrewNumber(dayOfChanukah) + " " + hebrewHolidays[index]) : (transliteratedHolidays[index] + dayOfChanukah);
            }
            return index == -1 ? "" : hebrewFormat ? hebrewHolidays[index] : transliteratedHolidays[index];
        }

		public virtual string FormatRoshChodesh(DateTime dt)
        {
			
			if (!jewishCalendar.IsRoshChodesh(dt))
            {
                return "";
            }
            string formattedRoshChodesh = "";
			int dayOfMonth = jewishCalendar.GetDayOfMonth(dt);
			JewishCalendar.JewishMonth month = jewishCalendar.GetJewishMonth (dt);
			int year = jewishCalendar.GetYear (dt);
			bool isLeapYear = jewishCalendar.IsLeapYearFromDateTime (dt);

			if (dayOfMonth == 30)
            {
				if (month < JewishCalendar.JewishMonth.ADAR || (month == JewishCalendar.JewishMonth.ADAR && isLeapYear))
                {
                    month++;
                } // roll to Nissan
                else
                {
                    month = JewishCalendar.JewishMonth.NISSAN;
                }
            }

			DateTime updatedDateTime = jewishCalendar.GetJewishDateTime (year, month, dayOfMonth);

            // This method is only about formatting, so we shouldn't make any changes to the params passed in...
			formattedRoshChodesh = hebrewFormat ? hebrewHolidays[(int)JewishCalendar.JewishHoliday.ROSH_CHODESH] : transliteratedHolidays[(int)JewishCalendar.JewishHoliday.ROSH_CHODESH];
			formattedRoshChodesh += " " + FormatMonth(updatedDateTime);
            return formattedRoshChodesh;
        }

        /// <summary>
        /// Returns if the formatter is set to use Hebrew formatting in the various formatting methods.
        /// </summary>
        /// <returns> the hebrewFormat </returns>
        /// <seealso cref="SetHebrewFormat(bool)"></seealso>
        /// <seealso cref="Format"></seealso>
        /// <seealso cref="FormatDayOfWeek"></seealso>
        /// <seealso cref="FormatMonth"></seealso>
        /// <seealso cref="FormatOmer"></seealso>
        /// <seealso cref="formatParsha(JewishCalendar)"></seealso>
        /// <seealso cref="FormatYomTov"></seealso>
        public virtual bool HebrewFormat
        {
            get
            {
                return hebrewFormat;
            }
            set
            {
                this.hebrewFormat = value;
            }
        }


        /// <summary>
        /// Returns the Hebrew Omer prefix. By default it is the letter &#x5D1;, but can be set to &#x5DC; (or any other
        /// prefix) using the <seealso cref="#setHebrewOmerPrefix(String)"/>.
        /// </summary>
        /// <returns> the hebrewOmerPrefix
        /// </returns>
        /// <seealso cref="SetHebrewOmerPrefix(String)"> </seealso>
        /// <seealso cref="FormatOmer(JewishCalendar)"> </seealso>
        public virtual string HebrewOmerPrefix
        {
            get
            {
                return hebrewOmerPrefix;
            }
            set
            {
                this.hebrewOmerPrefix = value;
            }
        }


        /// <summary>
        /// Returns the list of months transliterated into Latin chars. The default list of months uses Ashkenazi
        /// pronunciation in typical American English spelling. This list has a length of 14 with 3 variations for Adar -
        /// "Adar", "Adar II", "Adar I"
        /// </summary>
        /// <returns> the list of months beginning in Nissan and ending in in "Adar", "Adar II", "Adar I". The default list is
        ///         currently "Nissan", "Iyar", "Sivan", "Tammuz", "Av", "Elul", "Tishrei", "Cheshvan", "Kislev", "Teves",
        ///         "Shevat", "Adar", "Adar II", "Adar I" </returns>
        /// <seealso cref="setTransliteratedMonthList(String[])"></seealso>
        public virtual string[] TransliteratedMonthList
        {
            get
            {
                return transliteratedMonths;
            }
            set
            {
                this.transliteratedMonths = value;
            }
        }


        /// <summary>
        /// Unicode list of Hebrew months.
        /// </summary>
        /// <seealso cref="FormatMonth"></seealso>
        private static readonly string[] hebrewMonths = { "\u05E0\u05D9\u05E1\u05DF", "\u05D0\u05D9\u05D9\u05E8", "\u05E1\u05D9\u05D5\u05D5\u05DF", "\u05EA\u05DE\u05D5\u05D6", "\u05D0\u05D1", "\u05D0\u05DC\u05D5\u05DC", "\u05EA\u05E9\u05E8\u05D9", "\u05D7\u05E9\u05D5\u05D5\u05DF", "\u05DB\u05E1\u05DC\u05D5", "\u05D8\u05D1\u05EA", "\u05E9\u05D1\u05D8", "\u05D0\u05D3\u05E8", "\u05D0\u05D3\u05E8 \u05D1", "\u05D0\u05D3\u05E8 \u05D0" };

        /// <summary>
        /// list of transliterated parshiyos using the default Ashkenazi pronounciation. The formatParsha method uses this
        /// for transliterated parsha display. This list can be overridden (for Sephardi English transliteration for example)
        /// by setting the <seealso cref="#setTransliteratedParshiosList(String[])"/>.
        /// </summary>
        /// <seealso cref="FXAssemblyormatParsha(JewishCalendar)"></seealso>
        private string[] transliteratedParshios = { "Bereshis", "Noach", "Lech Lecha", "Vayera", "Chayei Sara", "Toldos", "Vayetzei", "Vayishlach", "Vayeshev", "Miketz", "Vayigash", "Vayechi", "Shemos", "Vaera", "Bo", "Beshalach", "Yisro", "Mishpatim", "Terumah", "Tetzaveh", "Ki Sisa", "Vayakhel", "Pekudei", "Vayikra", "Tzav", "Shmini", "Tazria", "Metzora", "Achrei Mos", "Kedoshim", "Emor", "Behar", "Bechukosai", "Bamidbar", "Nasso", "Beha'aloscha", "Sh'lach", "Korach", "Chukas", "Balak", "Pinchas", "Matos", "Masei", "Devarim", "Vaeschanan", "Eikev", "Re'eh", "Shoftim", "Ki Seitzei", "Ki Savo", "Nitzavim", "Vayeilech", "Ha'Azinu", "Vayakhel Pekudei", "Tazria Metzora", "Achrei Mos Kedoshim", "Behar Bechukosai", "Chukas Balak", "Matos Masei", "Nitzavim Vayeilech" };

        /// <summary>
        /// Retruns the list of transliterated parshiyos used by this formatter.
        /// </summary>
        /// <returns> the list of transliterated Parshios </returns>
        public virtual string[] TransliteratedParshiosList
        {
            get
            {
                return transliteratedParshios;
            }
            set
            {
                this.transliteratedParshios = value;
            }
        }


        /// <summary>
        /// Unicode list of Hebrew parshiyos.
        /// </summary>
        private static readonly string[] hebrewParshiyos = { "\u05D1\u05E8\u05D0\u05E9\u05D9\u05EA", "\u05E0\u05D7", "\u05DC\u05DA \u05DC\u05DA", "\u05D5\u05D9\u05E8\u05D0", "\u05D7\u05D9\u05D9 \u05E9\u05E8\u05D4", "\u05EA\u05D5\u05DC\u05D3\u05D5\u05EA", "\u05D5\u05D9\u05E6\u05D0", "\u05D5\u05D9\u05E9\u05DC\u05D7", "\u05D5\u05D9\u05E9\u05D1", "\u05DE\u05E7\u05E5", "\u05D5\u05D9\u05D2\u05E9", "\u05D5\u05D9\u05D7\u05D9", "\u05E9\u05DE\u05D5\u05EA", "\u05D5\u05D0\u05E8\u05D0", "\u05D1\u05D0", "\u05D1\u05E9\u05DC\u05D7", "\u05D9\u05EA\u05E8\u05D5", "\u05DE\u05E9\u05E4\u05D8\u05D9\u05DD", "\u05EA\u05E8\u05D5\u05DE\u05D4", "\u05EA\u05E6\u05D5\u05D4", "\u05DB\u05D9 \u05EA\u05E9\u05D0", "\u05D5\u05D9\u05E7\u05D4\u05DC", "\u05E4\u05E7\u05D5\u05D3\u05D9", "\u05D5\u05D9\u05E7\u05E8\u05D0", "\u05E6\u05D5", "\u05E9\u05DE\u05D9\u05E0\u05D9", "\u05EA\u05D6\u05E8\u05D9\u05E2", "\u05DE\u05E6\u05E8\u05E2", "\u05D0\u05D7\u05E8\u05D9 \u05DE\u05D5\u05EA", "\u05E7\u05D3\u05D5\u05E9\u05D9\u05DD", "\u05D0\u05DE\u05D5\u05E8", "\u05D1\u05D4\u05E8", "\u05D1\u05D7\u05E7\u05EA\u05D9", "\u05D1\u05DE\u05D3\u05D1\u05E8", "\u05E0\u05E9\u05D0", "\u05D1\u05D4\u05E2\u05DC\u05EA\u05DA", "\u05E9\u05DC\u05D7 \u05DC\u05DA", "\u05E7\u05E8\u05D7", "\u05D7\u05D5\u05E7\u05EA", "\u05D1\u05DC\u05E7", "\u05E4\u05D9\u05E0\u05D7\u05E1", "\u05DE\u05D8\u05D5\u05EA", "\u05DE\u05E1\u05E2\u05D9", "\u05D3\u05D1\u05E8\u05D9\u05DD", "\u05D5\u05D0\u05EA\u05D7\u05E0\u05DF", "\u05E2\u05E7\u05D1", "\u05E8\u05D0\u05D4", "\u05E9\u05D5\u05E4\u05D8\u05D9\u05DD", "\u05DB\u05D9 \u05EA\u05E6\u05D0", "\u05DB\u05D9 \u05EA\u05D1\u05D5\u05D0", "\u05E0\u05D9\u05E6\u05D1\u05D9\u05DD", "\u05D5\u05D9\u05DC\u05DA", "\u05D4\u05D0\u05D6\u05D9\u05E0\u05D5", "\u05D5\u05D9\u05E7\u05D4\u05DC \u05E4\u05E7\u05D5\u05D3\u05D9", "\u05EA\u05D6\u05E8\u05D9\u05E2 \u05DE\u05E6\u05E8\u05E2", "\u05D0\u05D7\u05E8\u05D9 \u05DE\u05D5\u05EA \u05E7\u05D3\u05D5\u05E9\u05D9\u05DD", "\u05D1\u05D4\u05E8 \u05D1\u05D7\u05E7\u05EA\u05D9", "\u05D7\u05D5\u05E7\u05EA \u05D1\u05DC\u05E7", "\u05DE\u05D8\u05D5\u05EA \u05DE\u05E1\u05E2\u05D9", "\u05E0\u05D9\u05E6\u05D1\u05D9\u05DD \u05D5\u05D9\u05DC\u05DA" };

        /// <summary>
        /// Unicode list of Hebrew days of week.
        /// </summary>
        private static readonly string[] hebrewDaysOfWeek = { "\u05E8\u05D0\u05E9\u05D5\u05DF", "\u05E9\u05E0\u05D9", "\u05E9\u05DC\u05D9\u05E9\u05D9", "\u05E8\u05D1\u05D9\u05E2\u05D9", "\u05D7\u05DE\u05D9\u05E9\u05D9", "\u05E9\u05E9\u05D9", "\u05E9\u05D1\u05EA" };

        /// <summary>
        /// Formats the day of week. If <seealso cref="#isHebrewFormat() Hebrew formatting"/> is set, it will display in the format
        /// &#x05E8;&#x05D0;&#x05E9;&#x05D5;&#x05DF; etc. If Hebrew formatting is not in use it will return it in the format
        /// of Sunday etc. There are various formatting options that will affect the output.
        /// </summary>
        /// <param name="jewishDate"> </param>
        /// <returns> the formatted day of week </returns>
        /// <seealso cref="isHebrewFormat()"></seealso>
        /// <seealso cref="isLongWeekFormat()"></seealso>
		public virtual string FormatDayOfWeek(DateTime dt)
        {
			int dayOfWeek = jewishCalendar.GetJewishDayOfWeek(dt);

            if (hebrewFormat)
            {
                StringBuilder sb = new StringBuilder();
				sb.Append(longWeekFormat ? hebrewDaysOfWeek[dayOfWeek - 1] : FormatHebrewNumber(dayOfWeek));
                return sb.ToString();
            }
            else
            {
				return dayOfWeek == 7 ? TransliteratedShabbosDayOfWeek : dt.ToString("dddd");
            }
        }
      
        /// <summary>
        /// Returns whether the class is set to use the Geresh &#x5F3; and Gershayim &#x5F4; in formatting Hebrew dates and
        /// numbers. When true and output would look like &#x5DB;&#x5F4;&#x5D0; &#x5E9;&#x5D1;&#x5D8;
        /// &#x5EA;&#x5E9;&#x5DA;&#x5F3;. When set to false, this output would display as &#x5DB&#x5D0; &#x5E9;&#x5D1;&#x5D8;
        /// &#x5EA;&#x5E9;&#x5DA;.
        /// </summary>
        /// <returns> true if set to use the Geresh &#x5F3; and Gershayim &#x5F4; in formatting Hebrew dates and numbers. </returns>
        public virtual bool UseGershGershayim
        {
            get
            {
                return useGershGershayim;
            }
            set
            {
                this.useGershGershayim = value;
            }
        }


        /// <summary>
        /// Returns whether the class is set to use the thousands digit when formatting. When formatting a Hebrew Year,
        /// traditionally the thousands digit is omitted and output for a year such as 5729 (1969 Gregorian) would be
        /// calculated for 729 and format as &#x5EA;&#x5E9;&#x5DB;&#x5F4;&#x5D8;. When set to true the long format year such
        /// as &#x5D4;&#x5F3; &#x5EA;&#x5E9;&#x5DB;&#x5F4;&#x5D8; for 5729/1969 is returned.
        /// </summary>
        /// <returns> true if set to use the the thousands digit when formatting Hebrew dates and numbers. </returns>
        public virtual bool UseLongHebrewYears
        {
            get
            {
                return useLonghebrewYears;
            }
            set
            {
                this.useLonghebrewYears = value;
            }
        }


        /// <summary>
        /// Formats the Jewish date. If the formatter is set to Hebrew, it will format in the form, "day Month year" for
        /// example &#x5DB;&#x5F4;&#x5D0; &#x5E9;&#x5D1;&#x5D8; &#x5EA;&#x5E9;&#x5DB;&#x5F4;&#x5D8;, and the format
        /// "21 Shevat, 5729" if not.
        /// </summary>
        /// <param name="jewishDate">
        ///            the JewishDate to be formatted </param>
        /// <returns> the formatted date. If the formatter is set to Hebrew, it will format in the form, "day Month year" for
        ///         example &#x5DB;&#x5F4;&#x5D0; &#x5E9;&#x5D1;&#x5D8; &#x5EA;&#x5E9;&#x5DB;&#x5F4;&#x5D8;, and the format
        ///         "21 Shevat, 5729" if not. </returns>
		public virtual string Format(DateTime dt)

        {
            if (HebrewFormat)
            {
				return FormatHebrewNumber(jewishCalendar.GetDayOfMonth(dt)) + " " + FormatMonth(dt) + " " + FormatHebrewNumber(jewishCalendar.GetYear(dt));
            }
            else
            {
				return jewishCalendar.GetDayOfMonth(dt) + " " + FormatMonth(dt) + ", " + jewishCalendar.GetYear(dt);
            }
        }

        /// <summary>
        /// Returns a string of the current Hebrew month such as "Tishrei". Returns a string of the current Hebrew month such
        /// as "&#x5D0;&#x5D3;&#x5E8; &#x5D1;&#x5F3;".
        /// </summary>
        /// <param name="jewishDate">
        ///            the JewishDate to format </param>
        /// <returns> the formatted month name </returns>
        /// <seealso cref= #isHebrewFormat() </seealso>
        /// <seealso cref= #setHebrewFormat(boolean) </seealso>
        /// <seealso cref= #getTransliteratedMonthList() </seealso>
        /// <seealso cref= #setTransliteratedMonthList(String[]) </seealso>
		public virtual string FormatMonth(DateTime dt)
        {
            //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
            //ORIGINAL LINE: final int month = jewishDate.getJewishMonth();
			JewishCalendar.JewishMonth month = jewishCalendar.GetJewishMonth(dt);
			bool isLeapYear = jewishCalendar.IsLeapYearFromDateTime (dt);

            if (HebrewFormat)
            {
				if (isLeapYear && month == JewishCalendar.JewishMonth.ADAR)
                {
                    return hebrewMonths[13] + (useGershGershayim ? GERESH : ""); // return Adar I, not Adar in a leap year
                }
				else if (isLeapYear && month == JewishCalendar.JewishMonth.ADAR_II)
                {
                    return hebrewMonths[12] + (useGershGershayim ? GERESH : "");
                }
                else
                {
					return hebrewMonths[(int)month - 1];
                }
            }
            else
            {
				if (isLeapYear && month == JewishCalendar.JewishMonth.ADAR)
                {
                    return transliteratedMonths[13]; // return Adar I, not Adar in a leap year
                }
                else
                {
					return transliteratedMonths[(int)month - 1];
                }
            }
        }

        /// <summary>
        /// Returns a String of the Omer day in the form &#x5DC;&#x5F4;&#x5D2; &#x5D1;&#x05E2;&#x05D5;&#x05DE;&#x5E8; if
        /// Hebrew Format is set, or "Omer X" or "Lag BaOmer" if not. An empty string if there is no Omer this day.
        /// </summary>
        /// <returns> a String of the Omer day in the form or an empty string if there is no Omer this day. The default
        ///         formatting has a &#x5D1;&#x5F3; prefix that would output &#x5D1;&#x05E2;&#x05D5;&#x05DE;&#x5E8;, but this
        ///         can be set via the <seealso cref="#setHebrewOmerPrefix(String)"/> method to use a &#x5DC; and output
        ///         &#x5DC;&#x5F4;&#x5D2; &#x5DC;&#x05E2;&#x05D5;&#x05DE;&#x5E8;. </returns>
        /// <seealso cref="isHebrewFormat()"></seealso>
        /// <seealso cref="getHebrewOmerPrefix()"></seealso>
        /// <seealso cref="setHebrewOmerPrefix(String)"></seealso>
		public virtual string FormatOmer(DateTime dt)
        {
			int omer = jewishCalendar.GetDayOfOmer(dt);
            if (omer == -1)
            {
                return "";
            }
            if (hebrewFormat)
            {
                return FormatHebrewNumber(omer) + " " + hebrewOmerPrefix + "\u05E2\u05D5\u05DE\u05E8";
            }
            else
            {
                if (omer == 33) // if lag b'omer
                {
                    return "Lag BaOmer";
                }
                else
                {
                    return "Omer " + omer;
                }
            }
        }

        /// <summary>
        /// Experimental and incomplete
        /// </summary>
        /// <param name="moladChalakim"> </param>
        /// <returns> the formatted molad. FIXME: define proper format in English and Hebrew. </returns>
        private string FormatMolad(long moladChalakim)
        {
            long adjustedChalakim = moladChalakim;
            int MINUTE_CHALAKIM = 18;
            int HOUR_CHALAKIM = 1080;
            int DAY_CHALAKIM = 24 * HOUR_CHALAKIM;

            long days = adjustedChalakim / DAY_CHALAKIM;
            adjustedChalakim = adjustedChalakim - (days * DAY_CHALAKIM);
            int hours = (int)((adjustedChalakim / HOUR_CHALAKIM));
            if (hours >= 6)
            {
                days += 1;
            }
            adjustedChalakim = adjustedChalakim - (hours * HOUR_CHALAKIM);
            int minutes = (int)(adjustedChalakim / MINUTE_CHALAKIM);
            adjustedChalakim = adjustedChalakim - minutes * MINUTE_CHALAKIM;
            return "Day: " + days % 7 + " hours: " + hours + ", minutes " + minutes + ", chalakim: " + adjustedChalakim;
        }

        /// <summary>
        /// Returns the kviah in the traditional 3 letter Hebrew format where the first letter represents the day of week of
        /// Rosh Hashana, the second letter represents the lengths of Cheshvan and Kislev ({@link JewishDate#SHELAIMIM
        /// Shelaimim} , <seealso cref="JewishDate#KESIDRAN Kesidran"/> or <seealso cref="JewishDate#CHASERIM Chaserim"/>) and the 3rd letter
        /// represents the day of week of Pesach. For example 5729 (1969) would return &#x5D1;&#x5E9;&#x5D4; (Rosh Hashana on
        /// Monday, Shelaimim, and Pesach on Thursday), while 5771 (2011) would return &#x5D4;&#x5E9;&#x5D2; (Rosh Hashana on
        /// Thursday, Shelaimim, and Pesach on Tuesday).
        /// </summary>
        /// <param name="jewishYear">
        ///            the Jewish year </param>
        /// <returns> the Hebrew String such as &#x5D1;&#x5E9;&#x5D4; for 5729 (1969) and &#x5D4;&#x5E9;&#x5D2; for 5771
        ///         (2011). </returns>
		/// 
		/// 
        
		public virtual string GetFormattedKviah(int jewishYear)
        {
			DateTime dt = jewishCalendar.GetJewishDateTime (jewishYear, JewishCalendar.JewishMonth.TISHREI, 1);// set date to Rosh Hashana

			JewishCalendar.JewishYearType yearType = jewishCalendar.GetJewishYearType (dt);
			int roshHashanaDayOfweek = jewishCalendar.GetJewishDayOfWeek(dt);
            string returnValue = FormatHebrewNumber(roshHashanaDayOfweek);
			returnValue += (yearType == JewishCalendar.JewishYearType.CHASERIM ? "\u05D7" : yearType == JewishCalendar.JewishYearType.SHELAIMIM ? "\u05E9" : "\u05DB");
			dt = jewishCalendar.GetJewishDateTime (jewishYear, JewishCalendar.JewishMonth.NISSAN, 15); // set to Pesach of the given year
			int pesachDayOfweek = jewishCalendar.GetJewishDayOfWeek(dt);
            returnValue += FormatHebrewNumber(pesachDayOfweek);
            returnValue = returnValue.Replace(GERESH, ""); // geresh is never used in the kviah format
            // boolean isLeapYear = JewishDate.isJewishLeapYear(jewishYear);
            // for efficiency we can avoid the expensive recalculation of the pesach day of week by adding 1 day to Rosh
            // Hashana for a 353 day year, 2 for a 354 day year, 3 for a 355 or 383 day year, 4 for a 384 day year and 5 for
            // a 385 day year
            return returnValue;
        }

        /// <summary>
        /// Format the daf to the mesechta and the daf
        /// </summary>
        /// <param name="daf"></param>
        /// <returns></returns>
        public virtual string FormatDafYomiBavli(Daf daf)
        {
            if (hebrewFormat)
            {
                return daf.Masechta + " " + FormatHebrewNumber(daf.Page);
            }
            else
            {
                return daf.MasechtaTransliterated + " " + daf.Page;
            }
        }

        /// <summary>
        /// Returns a Hebrew formatted string of a number. The method can calculate from 0 - 9999.
        /// <ul>
        /// <li>Single digit numbers such as 3, 30 and 100 will be returned with a &#x5F3; (<a
        /// href="http://en.wikipedia.org/wiki/Geresh">Geresh</a>) appended as at the end. For example &#x5D2;&#x5F3;,
        /// &#x5DC;&#x5F3; and &#x5E7;&#x5F3;</li>
        /// <li>multi digit numbers such as 21 and 769 will be returned with a &#x5F4; (<a
        /// href="http://en.wikipedia.org/wiki/Gershayim">Gershayim</a>) between the second to last and last letters. For
        /// example &#x5DB;&#x5F4;&#x5D0;, &#x5EA;&#x5E9;&#x5DB;&#x5F4;&#x5D8;</li>
        /// <li>15 and 16 will be returned as &#x5D8;&#x5F4;&#x5D5; and &#x5D8;&#x5F4;&#x5D6;</li>
        /// <li>Single digit numbers (years assumed) such as 6000 (%1000=0) will be returned as &#x5D5;&#x5F3;
        /// &#x5D0;&#x5DC;&#x5E4;&#x5D9;&#x5DD;</li>
        /// <li>0 will return &#x5D0;&#x5E4;&#x05E1;</li>
        /// </ul>
        /// </summary>
        /// <param name="number">
        ///            the number to be formatted. It will trow an IllegalArgumentException if the number is < 0 or > 9999. </param>
        /// <returns> the Hebrew formatted number such as &#x5EA;&#x5E9;&#x5DB;&#x5F4;&#x5D8;
        ///  </returns>
        public virtual string FormatHebrewNumber(int number)
        {
            if (number < 0)
            {
                throw new System.ArgumentException("negative numbers can't be formatted");
            }
            else if (number > 9999)
            {
                throw new System.ArgumentException("numbers > 9999 can't be formatted");
            }

            string ALAFIM = "\u05D0\u05DC\u05E4\u05D9\u05DD";
            string EFES = "\u05D0\u05E4\u05E1";

            string[] jHundreds = new string[] { "", "\u05E7", "\u05E8", "\u05E9", "\u05EA", "\u05EA\u05E7", "\u05EA\u05E8", "\u05EA\u05E9", "\u05EA\u05EA", "\u05EA\u05EA\u05E7" };
            string[] jTens = new string[] { "", "\u05D9", "\u05DB", "\u05DC", "\u05DE", "\u05E0", "\u05E1", "\u05E2", "\u05E4", "\u05E6" };
            string[] jTenEnds = new string[] { "", "\u05D9", "\u05DA", "\u05DC", "\u05DD", "\u05DF", "\u05E1", "\u05E2", "\u05E3", "\u05E5" };
            string[] tavTaz = new string[] { "\u05D8\u05D5", "\u05D8\u05D6" };
            string[] jOnes = new string[] { "", "\u05D0", "\u05D1", "\u05D2", "\u05D3", "\u05D4", "\u05D5", "\u05D6", "\u05D7", "\u05D8" };

            if (number == 0) // do we realy need this? Should it be applicable to a date?
            {
                return EFES;
            }
            int shortNumber = number % 1000; // discard thousands
            // next check for all possible single Hebrew digit years
            bool singleDigitNumber = (shortNumber < 11 || (shortNumber < 100 && shortNumber % 10 == 0) || (shortNumber <= 400 && shortNumber % 100 == 0));
            int thousands = number / 1000; // get # thousands
            StringBuilder sb = new StringBuilder();
            // append thousands to String
            if (number % 1000 == 0) // in year is 5000, 4000 etc
            {
                sb.Append(jOnes[thousands]);
                if (UseGershGershayim)
                {
                    sb.Append(GERESH);
                }
                sb.Append(" ");
                sb.Append(ALAFIM); // add # of thousands plus word thousand (overide alafim boolean)
                return sb.ToString();
            } // if alafim boolean display thousands
            else if (useLonghebrewYears && number >= 1000)
            {
                sb.Append(jOnes[thousands]);
                if (UseGershGershayim)
                {
                    sb.Append(GERESH); // append thousands quote
                }
                sb.Append(" ");
            }
            number = number % 1000; // remove 1000s
            int hundreds = number / 100; // # of hundreds
            sb.Append(jHundreds[hundreds]); // add hundreds to String
            number = number % 100; // remove 100s
            if (number == 15) // special case 15
            {
                sb.Append(tavTaz[0]);
            } // special case 16
            else if (number == 16)
            {
                sb.Append(tavTaz[1]);
            }
            else
            {
                int tens = number / 10;
                if (number % 10 == 0) // if evenly divisable by 10
                {
                    if (singleDigitNumber == false)
                    {
                        sb.Append(jTenEnds[tens]); // end letters so years like 5750 will end with an end nun
                    }
                    else
                    {
                        sb.Append(jTens[tens]); // standard letters so years like 5050 will end with a regular nun
                    }
                }
                else
                {
                    sb.Append(jTens[tens]);
                    number = number % 10;
                    sb.Append(jOnes[number]);
                }
            }
            if (UseGershGershayim)
            {
                if (singleDigitNumber == true)
                {
                    sb.Append(GERESH); // append single quote
                } // append double quote before last digit
                else
                {
                    sb.Insert(sb.Length - 1, GERSHAYIM);
                }
            }
            return sb.ToString();
        }
    }
}
#endif