// * Zmanim .NET API
// * Copyright (C) 2004-2011 Eliyahu Hershfeld
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

using System.Globalization;
using Zmanim.Extensions;

namespace Zmanim.HebrewCalendar
{
    using System;

    /// <summary>
    /// The JewishDate class allows one to maintain an instance of a Gregorian date along with the corresponding Jewish date.
    /// This class can use the standard Java Date and Calendar classes for setting it, but does not subclass these classes or
    /// use them internally to any extensive use. This class also does not have a concept of a time (which the Date class
    /// does). Please note that the calendar does not currently support dates prior to 1/1/1 Gregorian. Also keep in mind
    /// that the Gregorian calendar started on October 15, 1582, so any calculations prior to that are suspect (at least from
    /// a Gregorian perspective). While 1/1/1 Gregorian and forward are technically supported, any calculations prior to <a
    /// href="http://en.wikipedia.org/wiki/Hillel_II">Hillel II's (Hakatan's</a>) calendar (4119 in the Jewish Calendar / 359
    /// CE Julian as recorded by <a href="http://en.wikipedia.org/wiki/Hai_Gaon">Rav Hai Gaon</a>) would be just an
    /// approximation.
    /// 
    /// This open source Java code was written by <a href="http://www.facebook.com/avromf">Avrom Finkelstien</a> from his C++
    /// code. It was refactored to fit the KosherJava Zmanim API with simplification of the code, enhancements and some bug
    /// fixing.
    /// 
    /// Some of Avrom's original C++ code was translated from <a href="http://emr.cs.uiuc.edu/~reingold/calendar.C">C/C++
    /// code</a> in <a href="http://www.calendarists.com">Calendrical Calculations</a> by Nachum Dershowitz and Edward M.
    /// Reingold, Software-- Practice & Experience, vol. 20, no. 9 (September, 1990), pp. 899- 928. Any method with the mark
    /// "ND+ER" indicates that the method was taken from this source with minor modifications.
    /// 
    /// If you are looking for a class that implements a Jewish calendar version of the Calendar class, one is available from
    /// the <a href="http://site.icu-project.org/" >ICU (International Components for Unicode)</a> project, formerly part of
    /// IBM's DeveloperWorks.
    /// </summary>
    /// <seealso cref= net.sourceforge.zmanim.hebrewcalendar.JewishCalendar </seealso>
    /// <seealso cref= net.sourceforge.zmanim.hebrewcalendar.HebrewDateFormatter </seealso>
    /// <seealso cref= java.util.Date </seealso>
    /// <seealso cref= java.util.Calendar
    /// @author &copy; Avrom Finkelstien 2002
    /// @author &copy; Eliyahu Hershfeld 2011 - 2012
    /// @version 0.2.6 </seealso>
    public class JewishDate : IComparable<JewishDate>, ICloneable
    {
        /// <summary>
        /// Value of the month field indicating Nissan, the first numeric month of the year in the Jewish calendar. With the
        /// year starting at <seealso cref="#TISHREI"/>, it would actually be the 7th (or 8th in a {@link #isJewishLeapYear() leap
        /// year}) month of the year.
        /// </summary>
        public const int NISSAN = 1;

        /// <summary>
        /// Value of the month field indicating Iyar, the second numeric month of the year in the Jewish calendar. With the
        /// year starting at <seealso cref="#TISHREI"/>, it would actually be the 8th (or 9th in a {@link #isJewishLeapYear() leap
        /// year}) month of the year.
        /// </summary>
        public const int IYAR = 2;

        /// <summary>
        /// Value of the month field indicating Sivan, the third numeric month of the year in the Jewish calendar. With the
        /// year starting at <seealso cref="#TISHREI"/>, it would actually be the 9th (or 10th in a {@link #isJewishLeapYear() leap
        /// year}) month of the year.
        /// </summary>
        public const int SIVAN = 3;

        /// <summary>
        /// Value of the month field indicating Tammuz, the fourth numeric month of the year in the Jewish calendar. With the
        /// year starting at <seealso cref="#TISHREI"/>, it would actually be the 10th (or 11th in a {@link #isJewishLeapYear() leap
        /// year}) month of the year.
        /// </summary>
        public const int TAMMUZ = 4;

        /// <summary>
        /// Value of the month field indicating Av, the fifth numeric month of the year in the Jewish calendar. With the year
        /// starting at <seealso cref="#TISHREI"/>, it would actually be the 11th (or 12th in a <seealso cref="#isJewishLeapYear() leap year"/>)
        /// month of the year.
        /// </summary>
        public const int AV = 5;

        /// <summary>
        /// Value of the month field indicating Elul, the sixth numeric month of the year in the Jewish calendar. With the
        /// year starting at <seealso cref="#TISHREI"/>, it would actually be the 12th (or 13th in a {@link #isJewishLeapYear() leap
        /// year}) month of the year.
        /// </summary>
        public const int ELUL = 6;

        /// <summary>
        /// Value of the month field indicating Tishrei, the seventh numeric month of the year in the Jewish calendar. With
        /// the year starting at this month, it would actually be the 1st month of the year.
        /// </summary>
        public const int TISHREI = 7;

        /// <summary>
        /// Value of the month field indicating Cheshvan/marcheshvan, the eighth numeric month of the year in the Jewish
        /// calendar. With the year starting at <seealso cref="#TISHREI"/>, it would actually be the 2nd month of the year.
        /// </summary>
        public const int CHESHVAN = 8;

        /// <summary>
        /// Value of the month field indicating Kislev, the ninth numeric month of the year in the Jewish calendar. With the
        /// year starting at <seealso cref="#TISHREI"/>, it would actually be the 3rd month of the year.
        /// </summary>
        public const int KISLEV = 9;

        /// <summary>
        /// Value of the month field indicating Teves, the tenth numeric month of the year in the Jewish calendar. With the
        /// year starting at <seealso cref="#TISHREI"/>, it would actually be the 4th month of the year.
        /// </summary>
        public const int TEVES = 10;

        /// <summary>
        /// Value of the month field indicating Shevat, the eleventh numeric month of the year in the Jewish calendar. With
        /// the year starting at <seealso cref="#TISHREI"/>, it would actually be the 5th month of the year.
        /// </summary>
        public const int SHEVAT = 11;

        /// <summary>
        /// Value of the month field indicating Adar (or Adar I in a <seealso cref="#isJewishLeapYear() leap year"/>), the twelfth
        /// numeric month of the year in the Jewish calendar. With the year starting at <seealso cref="#TISHREI"/>, it would actually
        /// be the 6th month of the year.
        /// </summary>
        public const int ADAR = 12;

        /// <summary>
        /// Value of the month field indicating Adar II, the leap (intercalary or embolismic) thirteenth (Undecimber) numeric
        /// month of the year added in Jewish <seealso cref="#isJewishLeapYear() leap year"/>). The leap years are years 3, 6, 8, 11,
        /// 14, 17 and 19 of a 19 year cycle. With the year starting at <seealso cref="#TISHREI"/>, it would actually be the 7th month
        /// of the year.
        /// </summary>
        public const int ADAR_II = 13;

        /// <summary>
        /// the Jewish epoch using the RD (Rata Die/Fixed Date or Reingold Dershowitz) day used in Calendrical Calculations.
        /// Day 1 is January 1, 0001 Gregorian
        /// </summary>
        private const int JEWISH_EPOCH = -1373429;

        private const int CHALAKIM_PER_MINUTE = 18;
        private const int CHALAKIM_PER_HOUR = 1080;
        private const int CHALAKIM_PER_DAY = 25920; // 24 * 1080
        private const long CHALAKIM_PER_MONTH = 765433; // (29 * 24 + 12) * 1080 + 793
        /// <summary>
        /// Days from the beginning of Sunday till molad BaHaRaD. Calculated as 1 day, 5 hours and 204 chalakim = (24 + 5) *
        /// 1080 + 204 = 31524
        /// </summary>
        private const int CHALAKIM_MOLAD_TOHU = 31524;

        /// <summary>
        /// A short year where both <seealso cref="#CHESHVAN"/> and <seealso cref="#KISLEV"/> are 29 days.
        /// </summary>
        /// <seealso cref= #getCheshvanKislevKviah() </seealso>
        /// <seealso cref= HebrewDateFormatter#getFormattedKviah(int) </seealso>
        public const int CHASERIM = 0;

        /// <summary>
        /// An ordered year where <seealso cref="#CHESHVAN"/> is 29 days and <seealso cref="#KISLEV"/> is 30 days.
        /// </summary>
        /// <seealso cref= #getCheshvanKislevKviah() </seealso>
        /// <seealso cref= HebrewDateFormatter#getFormattedKviah(int) </seealso>
        public const int KESIDRAN = 1;

        /// <summary>
        /// A long year where both <seealso cref="#CHESHVAN"/> and <seealso cref="#KISLEV"/> are 30 days.
        /// </summary>
        /// <seealso cref= #getCheshvanKislevKviah() </seealso>
        /// <seealso cref= HebrewDateFormatter#getFormattedKviah(int) </seealso>
        public const int SHELAIMIM = 2;

        private int jewishMonth;
        private int jewishDay;
        private int jewishYear;
        private int moladHours;
        private int moladMinutes;
        private int moladChalakim;

        /// <summary>
        /// Returns the molad hours. Only a JewishDate object populated with <seealso cref="#getMolad()"/>,
        /// <seealso cref="#setJewishDate(int, int, int, int, int, int)"/> or <seealso cref="#setMoladHours(int)"/> will have this field
        /// populated. A regular JewishDate object will have this field set to 0.
        /// </summary>
        /// <returns> the molad hours </returns>
        /// <seealso cref= #setMoladHours(int) </seealso>
        /// <seealso cref= #getMolad() </seealso>
        /// <seealso cref= #setJewishDate(int, int, int, int, int, int) </seealso>
        public virtual int MoladHours
        {
            get
            {
                return moladHours;
            }
            set
            {
                this.moladHours = value;
            }
        }


        /// <summary>
        /// Returns the molad minutes. Only an object populated with <seealso cref="#getMolad()"/>,
        /// <seealso cref="#setJewishDate(int, int, int, int, int, int)"/> or or <seealso cref="#setMoladMinutes(int)"/> will have these fields
        /// populated. A regular JewishDate object will have this field set to 0.
        /// </summary>
        /// <returns> the molad minutes </returns>
        /// <seealso cref= #setMoladMinutes(int) </seealso>
        /// <seealso cref= #getMolad() </seealso>
        /// <seealso cref= #setJewishDate(int, int, int, int, int, int) </seealso>
        public virtual int MoladMinutes
        {
            get
            {
                return moladMinutes;
            }
            set
            {
                this.moladMinutes = value;
            }
        }


        /// <summary>
        /// Sets the molad chalakim/parts. The expectation is that the traditional minute-less chalakim will be broken out to
        /// <seealso cref="#setMoladMinutes(int) minutes"/> and chalakim, so 793 (TaShTZaG) parts would have the minutes set to 44 and
        /// chalakim to 1.
        /// </summary>
        /// <param name="moladChalakim">
        ///            the molad chalakim/parts to set </param>
        /// <seealso cref= #getMoladChalakim() </seealso>
        /// <seealso cref= #setMoladMinutes(int) </seealso>
        /// <seealso cref= #getMolad() </seealso>
        /// <seealso cref= #setJewishDate(int, int, int, int, int, int)
        ///  </seealso>
        public virtual int MoladChalakim
        {
            set
            {
                this.moladChalakim = value;
            }
            get
            {
                return moladChalakim;
            }
        }


        /// <summary>
        /// Returns the last day in a gregorian month
        /// </summary>
        /// <param name="month">
        ///            the Gregorian month </param>
        /// <returns> the last day of the Gregorian month </returns>
        internal virtual int getLastDayOfGregorianMonth(int month)
        {
            return getLastDayOfGregorianMonth(month, gregorianYear);
        }

        /// <summary>
        /// The month, where 1 == January, 2 == February, etc... Note that this is different than the Java's Calendar class
        /// where January ==0
        /// </summary>
        private int gregorianMonth;

        /// <summary>
        /// The day of the Gregorian month </summary>
        private int gregorianDayOfMonth;

        /// <summary>
        /// The Gregorian year </summary>
        private int gregorianYear;

        /// <summary>
        /// 1 == Sunday, 2 == Monday, etc... </summary>
        private int dayOfWeek;

        private int gregorianAbsDate;

        /// <summary>
        /// Returns the number of days in a given month in a given month and year.
        /// </summary>
        /// <param name="month">
        ///            the month. As with other cases in this class, this is 1-based, not zero-based. </param>
        /// <param name="year">
        ///            the year (only impacts February) </param>
        /// <returns> the number of days in the month in the given year </returns>
        private static int getLastDayOfGregorianMonth(int month, int year)
        {
            switch (month)
            {
                case 2:
                    if ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0))
                    {
                        return 29;
                    }
                    else
                    {
                        return 28;
                    }
                case 4:
                case 6:
                case 9:
                case 11:
                    return 30;
                default:
                    return 31;
            }
        }

        /// <summary>
        /// Computes the Gregorian date from the absolute date. ND+ER
        /// </summary>
        private void absDateToDate(int absDate)
        {
            int year = absDate / 366; // Search forward year by year from approximate year
            while (absDate >= gregorianDateToAbsDate(year + 1, 1, 1))
            {
                year++;
            }

            int month = 1; // Search forward month by month from January
            while (absDate > gregorianDateToAbsDate(year, month, getLastDayOfGregorianMonth(month, year)))
            {
                month++;
            }

            int dayOfMonth = absDate - gregorianDateToAbsDate(year, month, 1) + 1;
            setInternalGregorianDate(year, month, dayOfMonth);
        }

        /// <summary>
        /// Returns the absolute date (days since January 1, 0001 on the Gregorian calendar).
        /// </summary>
        /// <returns> the number of days since January 1, 1 </returns>
        protected internal virtual int AbsDate
        {
            get
            {
                return gregorianAbsDate;
            }
        }

        /// <summary>
        /// Computes the absolute date from a Gregorian date. ND+ER
        /// </summary>
        /// <param name="year">
        ///            the Gregorian year </param>
        /// <param name="month">
        ///            the Gregorian month. Unlike the Java Calendar where January has the value of 0,This expects a 1 for
        ///            January </param>
        /// <param name="dayOfMonth">
        ///            the day of the month (1st, 2nd, etc...) </param>
        /// <returns> the absolute Gregorian day </returns>
        private static int gregorianDateToAbsDate(int year, int month, int dayOfMonth)
        {
            int absDate = dayOfMonth;
            for (int m = month - 1; m > 0; m--)
            {
                absDate += getLastDayOfGregorianMonth(m, year); // days in prior months of the year
            }
            return (absDate + 365 * (year - 1) + (year - 1) / 4 - (year - 1) / 100 + (year - 1) / 400); // plus prior years divisible by 400 -  minus prior century years -  Julian leap days before this year -  days in previous years ignoring leap days -  days this year
        }

        /// <summary>
        /// Returns if the year is a Jewish leap year. Years 3, 6, 8, 11, 14, 17 and 19 in the 19 year cycle are leap years.
        /// </summary>
        /// <param name="year">
        ///            the Jewish year. </param>
        /// <returns> true if it is a leap year </returns>
        /// <seealso cref= #isJewishLeapYear() </seealso>
        private static bool isJewishLeapYear(int year)
        {
            return ((7 * year) + 1) % 19 < 7;
        }

        /// <summary>
        /// Returns if the year the calendar is set to is a Jewish leap year. Years 3, 6, 8, 11, 14, 17 and 19 in the 19 year
        /// cycle are leap years.
        /// </summary>
        /// <returns> true if it is a leap year </returns>
        /// <seealso cref= #isJewishLeapYear(int) </seealso>
        public virtual bool JewishLeapYear
        {
            get
            {
                return isJewishLeapYear(JewishYear);
            }
        }

        /// <summary>
        /// Returns the last month of a given Jewish year. This will be 12 on a non <seealso cref="#isJewishLeapYear(int) leap year"/>
        /// or 13 on a leap year.
        /// </summary>
        /// <param name="year">
        ///            the Jewish year. </param>
        /// <returns> 12 on a non leap year or 13 on a leap year </returns>
        /// <seealso cref= #isJewishLeapYear(int) </seealso>
        private static int getLastMonthOfJewishYear(int year)
        {
            return isJewishLeapYear(year) ? ADAR_II : ADAR;
        }

        /// <summary>
        /// Returns the number of days elapsed from the Sunday prior to the start of the Jewish calendar to the mean
        /// conjunction of Tishri of the Jewish year.
        /// </summary>
        /// <param name="year">
        ///            the Jewish year </param>
        /// <returns> the number of days elapsed from prior to the molad Tohu BaHaRaD (Be = Monday, Ha= 5 hours and Rad =204
        ///         chalakim/parts) prior to the start of the Jewish calendar, to the mean conjunction of Tishri of the
        ///         Jewish year. BeHaRaD is 23:11:20 on Sunday night(5 hours 204/1080 chalakim after sunset on Sunday
        ///         evening). </returns>
        public static int getJewishCalendarElapsedDays(int year)
        {
            long chalakimSince = getChalakimSinceMoladTohu(year, JewishDate.TISHREI);
            int moladDay = (int)(chalakimSince / (long)CHALAKIM_PER_DAY);
            int moladParts = (int)(chalakimSince - moladDay * (long)CHALAKIM_PER_DAY);
            // delay Rosh Hashana for the 4 dechiyos
            return addDechiyos(year, moladDay, moladParts);
        }

        // private static int getJewishCalendarElapsedDaysOLD(int year) {
        // // Jewish lunar month = 29 days, 12 hours and 793 chalakim
        // // Molad Tohu = BeHaRaD - Monday, 5 hours (11 PM) and 204 chalakim
        // final int chalakimTashTZag = 793; // chalakim in a lunar month
        // final int chalakimTohuRaD = 204; // chalakim from original molad Tohu BeHaRaD
        // final int hoursTohuHa = 5; // hours from original molad Tohu BeHaRaD
        // final int dayTohu = 1; // Monday (0 based)
        //
        // int monthsElapsed = (235 * ((year - 1) / 19)) // Months in complete 19 year lunar (Metonic) cycles so far
        // + (12 * ((year - 1) % 19)) // Regular months in this cycle
        // + ((7 * ((year - 1) % 19) + 1) / 19); // Leap months this cycle
        // // start with Molad Tohu BeHaRaD
        // // start with RaD of BeHaRaD and add TaShTzaG (793) chalakim plus elapsed chalakim
        // int partsElapsed = chalakimTohuRaD + chalakimTashTZag * (monthsElapsed % 1080);
        // // start with Ha hours of BeHaRaD, add 12 hour remainder of lunar month add hours elapsed
        // int hoursElapsed = hoursTohuHa + 12 * monthsElapsed + 793 * (monthsElapsed / 1080) + partsElapsed / 1080;
        // // start with Monday of BeHaRaD = 1 (0 based), add 29 days of the lunar months elapsed
        // int conjunctionDay = dayTohu + 29 * monthsElapsed + hoursElapsed / 24;
        // int conjunctionParts = 1080 * (hoursElapsed % 24) + partsElapsed % 1080;
        // return addDechiyos(year, conjunctionDay, conjunctionParts);
        // }

        /// <summary>
        /// Adds the 4 dechiyos for molad Tishrei. These are:
        /// <ol>
        /// <li>Lo ADU Rosh - Rosh Hashana can't fall on a Sunday, Wednesday or Friday. If the molad fell on one of these
        /// days, Rosh Hashana is delayed to the following day.</li>
        /// <li>Molad Zaken - If the molad of Tishrei falls after 12 noon, Rosh Hashana is delayed to the following day. If
        /// the following day is ADU, it will be delayed an additional day.</li>
        /// <li>GaTRaD - If on a non leap year the molad of Tishrei falls on a Tuesday (Ga) on or after 9 hours (T) and 204
        /// chalakim (TRaD) it is delayed till Thursday (one day delay, plus one day for Lo ADU Rosh)</li>
        /// <li>BeTuTaKFoT - if the year following a leap year falls on a Monday (Be) on or after 15 hours (Tu) and 589
        /// chalakim (TaKFoT) it is delayed till Tuesday</li>
        /// </ol>
        /// </summary>
        /// <param name="year"> </param>
        /// <param name="moladDay"> </param>
        /// <param name="moladParts"> </param>
        /// <returns> the nimber of elapsed days in the JewishCalendar adjusted for the 4 dechiyos. </returns>
        private static int addDechiyos(int year, int moladDay, int moladParts)
        {
            int roshHashanaDay = moladDay; // if no dechiyos
            // delay Rosh Hashana for the dechiyos of the Molad - new moon 1 - Molad Zaken, 2- GaTRaD 3- BeTuTaKFoT
            if ((moladParts >= 19440) || (((moladDay % 7) == 2) && (moladParts >= 9924) && !isJewishLeapYear(year)) || (((moladDay % 7) == 1) && (moladParts >= 16789) && (isJewishLeapYear(year - 1)))) // in a year following a leap year - end Dechiya of BeTuTaKFoT -  TRaD = 15 hours, 589 parts or later (15 * 1080 + 589) -  start Dechiya of BeTuTaKFoT - Be = is on a Monday -  of a non-leap year - end Dechiya of GaTRaD -  TRaD = 9 hours, 204 parts or later (9 * 1080 + 204) -  start Dechiya of GaTRaD - Ga = is a Tuesday -  Dechiya of Molad Zaken - molad is >= midday (18 hours * 1080 chalakim)
            {
                roshHashanaDay += 1; // Then postpone Rosh HaShanah one day
            }
            // start 4th Dechiya - Lo ADU Rosh - Rosh Hashana can't occur on A- sunday, D- Wednesday, U - Friday
            if (((roshHashanaDay % 7) == 0) || ((roshHashanaDay % 7) == 3) || ((roshHashanaDay % 7) == 5)) // or Friday - end 4th Dechiya - Lo ADU Rosh -  or Wednesday, -  If Rosh HaShanah would occur on Sunday,
            {
                roshHashanaDay = roshHashanaDay + 1; // Then postpone it one (more) day
            }
            return roshHashanaDay;
        }

        /// <summary>
        /// Returns the number of chalakim (parts - 1080 to the hour) from the original hypothetical Molad Tohu to the year
        /// and month passed in.
        /// </summary>
        /// <param name="year">
        ///            the Jewish year </param>
        /// <param name="month">
        ///            the Jewish month the Jewish month, with the month numbers starting from Nisan. Use the JewishDate
        ///            constants such as <seealso cref="JewishDate#TISHREI"/>. </param>
        /// <returns> the number of chalakim (parts - 1080 to the hour) from the original hypothetical Molad Tohu </returns>
        private static long getChalakimSinceMoladTohu(int year, int month)
        {
            // Jewish lunar month = 29 days, 12 hours and 793 chalakim
            // chalakim since Molad Tohu BeHaRaD - 1 day, 5 hours and 204 chalakim
            int monthOfYear = getJewishMonthOfYear(year, month);
            int monthsElapsed = (235 * ((year - 1) / 19)) + (12 * ((year - 1) % 19)) + ((7 * ((year - 1) % 19) + 1) / 19) + (monthOfYear - 1); // add elapsed months till the start of the molad of the month -  Leap months this cycle -  Regular months in this cycle -  Months in complete 19 year lunar (Metonic) cycles so far
            // return chalakim prior to BeHaRaD + number of chalakim since
            return CHALAKIM_MOLAD_TOHU + (CHALAKIM_PER_MONTH * monthsElapsed);
        }

        /// <summary>
        /// Returns the number of chalakim (parts - 1080 to the hour) from the original hypothetical Molad Tohu to the Jewish
        /// year and month that this Object is set to.
        /// </summary>
        /// <returns> the number of chalakim (parts - 1080 to the hour) from the original hypothetical Molad Tohu </returns>
        public virtual long ChalakimSinceMoladTohu
        {
            get
            {
                return getChalakimSinceMoladTohu(jewishYear, jewishMonth);
            }
        }

        /// <summary>
        /// Converts the the <seealso cref="JewishDate#NISSAN"/> based constants used by this class to numeric month starting from
        /// <seealso cref="JewishDate#TISHREI"/>. This is required for Molad claculations.
        /// </summary>
        /// <param name="year">
        ///            The Jewish year </param>
        /// <param name="month">
        ///            The Jewish Month </param>
        /// <returns> the Jewish month of the year starting with Tishrei </returns>
        private static int getJewishMonthOfYear(int year, int month)
        {
            bool isLeapYear = JewishDate.isJewishLeapYear(year);
            return (month + (isLeapYear ? 6 : 5)) % (isLeapYear ? 13 : 12) + 1;
        }

        /// <summary>
        /// Validates the components of a Jewish date for validity. It will throw an <seealso cref="IllegalArgumentException"/> if the
        /// Jewish date is earlier than 18 Teves, 3761 (1/1/1 Gregorian), a month < 1 or > 12 (or 13 on a
        /// <seealso cref="#isJewishLeapYear(int) leap year"/>), the day of month is < 1 or > 30, an hour < 0 or > 23, a minute < 0 >
        /// 59 or chalakim < 0 > 17. For larger a larger number of chalakim such as 793 (TaShTzaG) break the chalakim into
        /// minutes (18 chalakim per minutes, so it would be 44 minutes and 1 chelek in the case of 793/TaShTzaG).
        /// </summary>
        /// <param name="year">
        ///            the Jewish year to validate. It will reject any year <= 3761 (lower than the year 1 Gregorian). </param>
        /// <param name="month">
        ///            the Jewish month to validate. It will reject a month < 1 or > 12 (or 13 on a leap year) . </param>
        /// <param name="dayOfMonth">
        ///            the day of the Jewish month to validate. It will reject any value < 1 or > 30 TODO: check calling
        ///            methods to see if there is any reason that the class can validate that 30 is invalid for some months. </param>
        /// <param name="hours">
        ///            the hours (for molad calculations). It will reject an hour < 0 or > 23 </param>
        /// <param name="minutes">
        ///            the minutes (for molad calculations). It will reject a minute < 0 or > 59 </param>
        /// <param name="chalakim">
        ///            the chalakim/parts (for molad calculations). It will reject a chalakim < 0 or > 17. For larger numbers
        ///            such as 793 (TaShTzaG) break the chalakim into minutes (18 chalakim per minutes, so it would be 44
        ///            minutes and 1 chelek in the case of 793/TaShTzaG)
        /// </param>
        /// <exception cref="IllegalArgumentException">
        ///             if a A Jewish date earlier than 18 Teves, 3761 (1/1/1 Gregorian), a month < 1 or > 12 (or 13 on a
        ///             leap year), the day of month is < 1 or > 30, an hour < 0 or > 23, a minute < 0 > 59 or chalakim < 0 >
        ///             17. For larger a larger number of chalakim such as 793 (TaShTzaG) break the chalakim into minutes (18
        ///             chalakim per minutes, so it would be 44 minutes and 1 chelek in the case of 793 (TaShTzaG). </exception>
        private static void validateJewishDate(int year, int month, int dayOfMonth, int hours, int minutes, int chalakim)
        {
            if (month < NISSAN || month > getLastMonthOfJewishYear(year))
            {
                throw new System.ArgumentException("The Jewish month has to be between 1 and 12 (or 13 on a leap year). " + month + " is invalid for the year " + year + ".");
            }
            if (dayOfMonth < 1 || dayOfMonth > 30)
            {
                throw new System.ArgumentException("The Jewish day of month can't be < 1 or > 30.  " + dayOfMonth + " is invalid.");
            }
            // reject dates prior to 18 Teves, 3761 (1/1/1 AD). This restriction can be relaxed if the date coding is
            // changed/corrected
            if ((year < 3761) || (year == 3761 && (month >= TISHREI && month < TEVES)) || (year == 3761 && month == TEVES && dayOfMonth < 18))
            {
                throw new System.ArgumentException("A Jewish date earlier than 18 Teves, 3761 (1/1/1 Gregorian) can't be set. " + year + ", " + month + ", " + dayOfMonth + " is invalid.");
            }
            if (hours < 0 || hours > 23)
            {
                throw new System.ArgumentException("Hours < 0 > 23 can't be set. " + hours + " is invalid.");
            }

            if (minutes < 0 || minutes > 59)
            {
                throw new System.ArgumentException("Minutes < 0 > 59 can't be set. " + minutes + " is invalid.");
            }

            if (chalakim < 0 || chalakim > 17)
            {
                throw new System.ArgumentException("Chalakim/parts < 0 > 17 can't be set. " + chalakim + " is invalid. For larger numbers such as 793 (TaShTzaG) break the chalakim into minutes (18 chalakim per minutes, so it would be 44 minutes and 1 chelek in the case of 793 (TaShTzaG)");
            }
        }

        /// <summary>
        /// Validates the components of a Gregorian date for validity. It will throw an <seealso cref="IllegalArgumentException"/> if a
        /// year of < 1, a month < 0 or > 11 or a day of month < 1 is passed in.
        /// </summary>
        /// <param name="year">
        ///            the Gregorian year to validate. It will reject any year < 1. </param>
        /// <param name="month">
        ///            the Gregorian month number to validate. It will enforce that the month is between 0 - 11 like a
        ///            <seealso cref="GregorianCalendar"/>, where <seealso cref="Calendar#JANUARY"/> has a value of 0. </param>
        /// <param name="dayOfMonth">
        ///            the day of the Gregorian month to validate. It will reject any value < 1, but will allow values > 31
        ///            since calling methods will simply set it to the maximum for that month. TODO: check calling methods to
        ///            see if there is any reason that the class needs days > the maximum. </param>
        /// <exception cref="IllegalArgumentException">
        ///             if a year of < 1, a month < 0 or > 11 or a day of month < 1 is passed in </exception>
        /// <seealso cref= #validateGregorianYear(int) </seealso>
        /// <seealso cref= #validateGregorianMonth(int) </seealso>
        /// <seealso cref= #validateGregorianDayOfMonth(int) </seealso>
        private static void validateGregorianDate(int year, int month, int dayOfMonth)
        {
            validateGregorianMonth(month);
            validateGregorianDayOfMonth(dayOfMonth);
            validateGregorianYear(year);
        }

        /// <summary>
        /// Validates a Gregorian month for validity.
        /// </summary>
        /// <param name="month">
        ///            the Gregorian month number to validate. It will enforce that the month is between 0 - 11 like a
        ///            <seealso cref="GregorianCalendar"/>, where <seealso cref="Calendar#JANUARY"/> has a value of 0. </param>
        private static void validateGregorianMonth(int month)
        {
            if (month > 11 || month < 0)
            {
                throw new System.ArgumentException("The Gregorian month has to be between 0 - 11. " + month + " is invalid.");
            }
        }

        /// <summary>
        /// Validates a Gregorian day of month for validity.
        /// </summary>
        /// <param name="dayOfMonth">
        ///            the day of the Gregorian month to validate. It will reject any value < 1, but will allow values > 31
        ///            since calling methods will simply set it to the maximum for that month. TODO: check calling methods to
        ///            see if there is any reason that the class needs days > the maximum. </param>
        private static void validateGregorianDayOfMonth(int dayOfMonth)
        {
            if (dayOfMonth <= 0)
            {
                throw new System.ArgumentException("The day of month can't be less than 1. " + dayOfMonth + " is invalid.");
            }
        }

        /// <summary>
        /// Validates a Gregorian year for validity.
        /// </summary>
        /// <param name="year">
        ///            the Gregorian year to validate. It will reject any year < 1. </param>
        private static void validateGregorianYear(int year)
        {
            if (year < 1)
            {
                throw new System.ArgumentException("Years < 1 can't be claculated. " + year + " is invalid.");
            }
        }

        /// <summary>
        /// Returns the number of days for a given Jewish year. ND+ER
        /// </summary>
        /// <param name="year">
        ///            the Jewish year </param>
        /// <returns> the number of days for a given Jewish year. </returns>
        /// <seealso cref= #isCheshvanLong() </seealso>
        /// <seealso cref= #isKislevShort() </seealso>
        public static int getDaysInJewishYear(int year)
        {
            return getJewishCalendarElapsedDays(year + 1) - getJewishCalendarElapsedDays(year);
        }

        /// <summary>
        /// Returns the number of days for the current year that the calendar is set to.
        /// </summary>
        /// <seealso cref= #isCheshvanLong() </seealso>
        /// <seealso cref= #isKislevShort() </seealso>
        /// <seealso cref= #isJewishLeapYear() </seealso>
        public virtual int DaysInJewishYear
        {
            get
            {
                return getDaysInJewishYear(JewishYear);
            }
        }

        /// <summary>
        /// Returns if Cheshvan is long in a given Jewish year. The method name isLong is done since in a Kesidran (ordered)
        /// year Cheshvan is short. ND+ER
        /// </summary>
        /// <param name="year">
        ///            the year </param>
        /// <returns> true if Cheshvan is long in Jewish year. </returns>
        /// <seealso cref= #isCheshvanLong() </seealso>
        /// <seealso cref= #getCheshvanKislevKviah() </seealso>
        private static bool isCheshvanLong(int year)
        {
            return getDaysInJewishYear(year) % 10 == 5;
        }

        /// <summary>
        /// Returns if Cheshvan is long (30 days VS 29 days) for the current year that the calendar is set to. The method
        /// name isLong is done since in a Kesidran (ordered) year Cheshvan is short.
        /// </summary>
        /// <returns> true if Cheshvan is long for the current year that the calendar is set to </returns>
        /// <seealso cref= #isCheshvanLong() </seealso>
        public virtual bool CheshvanLong
        {
            get
            {
                return isCheshvanLong(JewishYear);
            }
        }

        /// <summary>
        /// Returns if Kislev is short (29 days VS 30 days) in a given Jewish year. The method name isShort is done since in
        /// a Kesidran (ordered) year Kislev is long. ND+ER
        /// </summary>
        /// <param name="year">
        ///            the Jewish year </param>
        /// <returns> true if Kislev is short for the given Jewish year. </returns>
        /// <seealso cref= #isKislevShort() </seealso>
        /// <seealso cref= #getCheshvanKislevKviah() </seealso>
        private static bool isKislevShort(int year)
        {
            return getDaysInJewishYear(year) % 10 == 3;
        }

        /// <summary>
        /// Returns if the Kislev is short for the year that this class is set to. The method name isShort is done since in a
        /// Kesidran (ordered) year Kislev is long.
        /// </summary>
        /// <returns> true if Kislev is short for the year that this class is set to </returns>
        public virtual bool KislevShort
        {
            get
            {
                return isKislevShort(JewishYear);
            }
        }

        /// <summary>
        /// Returns the Cheshvan and Kislev kviah (whether a Jewish year is short, regular or long). It will return
        /// <seealso cref="#SHELAIMIM"/> if both cheshvan and kislev are 30 days, <seealso cref="#KESIDRAN"/> if Cheshvan is 29 days and Kislev
        /// is 30 days and <seealso cref="#CHASERIM"/> if both are 29 days.
        /// </summary>
        /// <returns> <seealso cref="#SHELAIMIM"/> if both cheshvan and kislev are 30 days, <seealso cref="#KESIDRAN"/> if Cheshvan is 29 days and
        ///         Kislev is 30 days and <seealso cref="#CHASERIM"/> if both are 29 days. </returns>
        /// <seealso cref= #isCheshvanLong() </seealso>
        /// <seealso cref= #isKislevShort() </seealso>
        public virtual int CheshvanKislevKviah
        {
            get
            {
                if (CheshvanLong && !KislevShort)
                {
                    return SHELAIMIM;
                }
                else if (!CheshvanLong && KislevShort)
                {
                    return CHASERIM;
                }
                else
                {
                    return KESIDRAN;
                }
            }
        }

        /// <summary>
        /// Returns the number of days of a Jewish month for a given month and year.
        /// </summary>
        /// <param name="month">
        ///            the Jewish month </param>
        /// <param name="year">
        ///            the Jewish Year </param>
        /// <returns> the number of days for a given Jewish month </returns>
        private static int getDaysInJewishMonth(int month, int year)
        {
            if ((month == IYAR) || (month == TAMMUZ) || (month == ELUL) || ((month == CHESHVAN) && !(isCheshvanLong(year))) || ((month == KISLEV) && isKislevShort(year)) || (month == TEVES) || ((month == ADAR) && !(isJewishLeapYear(year))) || (month == ADAR_II))
            {
                return 29;
            }
            else
            {
                return 30;
            }
        }

        /// <summary>
        /// Returns the number of days of the Jewish month that the calendar is currently set to.
        /// </summary>
        /// <returns> the number of days for the Jewish month that the calendar is currently set to. </returns>
        public virtual int DaysInJewishMonth
        {
            get
            {
                return getDaysInJewishMonth(JewishMonth, JewishYear);
            }
        }

        /// <summary>
        /// Computes the Jewish date from the absolute date. ND+ER
        /// </summary>
        private void absDateToJewishDate()
        {
            // Approximation from below
            jewishYear = (gregorianAbsDate + JEWISH_EPOCH) / 366;
            // Search forward for year from the approximation
            while (gregorianAbsDate >= jewishDateToAbsDate(jewishYear + 1, TISHREI, 1))
            {
                jewishYear++;
            }
            // Search forward for month from either Tishri or Nisan.
            if (gregorianAbsDate < jewishDateToAbsDate(jewishYear, NISSAN, 1))
            {
                jewishMonth = TISHREI; // Start at Tishri
            }
            else
            {
                jewishMonth = NISSAN; // Start at Nisan
            }
            while (gregorianAbsDate > jewishDateToAbsDate(jewishYear, jewishMonth, DaysInJewishMonth))
            {
                jewishMonth++;
            }
            // Calculate the day by subtraction
            jewishDay = gregorianAbsDate - jewishDateToAbsDate(jewishYear, jewishMonth, 1) + 1;
        }

        /// <summary>
        /// Returns the absolute date of Jewish date. ND+ER
        /// </summary>
        /// <param name="year">
        ///            the Jewish year. The year can't be negative </param>
        /// <param name="month">
        ///            the Jewish month starting with Nisan. Nisan expects a value of 1 etc till Adar with a value of 12. For
        ///            a leap year, 13 will be the expected value for Adar II. Use the constants <seealso cref="JewishDate#NISSAN"/>
        ///            etc. </param>
        /// <param name="dayOfMonth">
        ///            the Jewish day of month. valid values are 1-30. If the day of month is set to 30 for a month that only
        ///            has 29 days, the day will be set as 29. </param>
        /// <returns> the absolute date of the Jewish date. </returns>
        private static int jewishDateToAbsDate(int year, int month, int dayOfMonth)
        {
            int elapsed = getDaysSinceStartOfJewishYear(year, month, dayOfMonth);
            // add elapsed days this year + Days in prior years + Days elapsed before absolute year 1
            return elapsed + getJewishCalendarElapsedDays(year) + JEWISH_EPOCH;
        }

        /// <summary>
        /// Returns the molad for a given year and month. Returns a JewishDate <seealso cref="Object"/> set to the date of the molad
        /// with the <seealso cref="#getMoladHours() hours"/>, <seealso cref="#getMoladMinutes() minutes"/> and {@link #getMoladChalakim()
        /// chalakim} set. In the current implementation, it sets the molad time based on a midnight date rollover. This
        /// means that Rosh Chodesh Adar II, 5771 with a molad of 7 chalakim past midnight on Shabbos 29 Adar I / March 5,
        /// 2011 12:00 AM and 7 chalakim, will have the following values: hours: 0, minutes: 0, Chalakim: 7.
        /// </summary>
        /// <returns> a JewishDate <seealso cref="Object"/> set to the date of the molad with the <seealso cref="#getMoladHours() hours"/>,
        ///         <seealso cref="#getMoladMinutes() minutes"/> and <seealso cref="#getMoladChalakim() chalakim"/> set. </returns>
        public virtual JewishDate Molad
        {
            get
            {
                JewishDate moladDate = new JewishDate(ChalakimSinceMoladTohu);
                if (moladDate.MoladHours >= 6)
                {
                    moladDate.forward();
                }
                moladDate.MoladHours = (moladDate.MoladHours + 18) % 24;
                return moladDate;
            }
        }

        /// <summary>
        /// Returns the number of days from the Jewish epoch from the number of chalakim from the epoch passed in.
        /// </summary>
        /// <param name="chalakim">
        ///            the number of chalakim since the beginning of Sunday prior to BaHaRaD </param>
        /// <returns> the number of days from the Jewish epoch </returns>
        private static int moladToAbsDate(long chalakim)
        {
            return (int)(chalakim / CHALAKIM_PER_DAY) + JEWISH_EPOCH;
        }

        /// <summary>
        /// Constructor that creates a JewishDate based on a molad passed in. The molad would be the number of chalakim/parts
        /// starting at the begining of Sunday prior to the molad Tohu BeHaRaD (Be = Monday, Ha= 5 hours and Rad =204
        /// chalakim/parts) - prior to the start of the Jewish calendar. BeHaRaD is 23:11:20 on Sunday night(5 hours 204/1080
        /// chalakim after sunset on Sunday evening).
        /// </summary>
        /// <param name="molad"> </param>
        public JewishDate(long molad)
        {
            absDateToDate(moladToAbsDate(molad));
            // long chalakimSince = getChalakimSinceMoladTohu(year, JewishDate.TISHREI);// tishrei
            int conjunctionDay = (int)(molad / (long)CHALAKIM_PER_DAY);
            int conjunctionParts = (int)(molad - conjunctionDay * (long)CHALAKIM_PER_DAY);
            MoladTime = conjunctionParts;
        }

        /// <summary>
        /// Sets the molad time (hours minutes and chalakim) based on the number of chalakim since the start of the day.
        /// </summary>
        /// <param name="chalakim">
        ///            the number of chalakim since the start of the day. </param>
        private int MoladTime
        {
            set
            {
                int adjustedChalakim = value;
                MoladHours = adjustedChalakim / CHALAKIM_PER_HOUR;
                adjustedChalakim = adjustedChalakim - (MoladHours * CHALAKIM_PER_HOUR);
                MoladMinutes = adjustedChalakim / CHALAKIM_PER_MINUTE;
                MoladChalakim = adjustedChalakim - moladMinutes * CHALAKIM_PER_MINUTE;
            }
        }

        /// <summary>
        /// returns the number of days from Rosh Hashana of the date passed in, till the full date passed in.
        /// </summary>
        /// <param name="year">
        ///            the Jewish year </param>
        /// <param name="month">
        ///            the Jewish month </param>
        /// <param name="dayOfMonth">
        ///            the day in the Jewish month </param>
        /// <returns> the number of days </returns>
        private static int getDaysSinceStartOfJewishYear(int year, int month, int dayOfMonth)
        {
            int elapsedDays = dayOfMonth;
            // Before Tishrei (from Nissan to Tishrei), add days in prior months
            if (month < TISHREI)
            {
                // this year before and after Nisan.
                for (int m = TISHREI; m <= getLastMonthOfJewishYear(year); m++)
                {
                    elapsedDays += getDaysInJewishMonth(m, year);
                }
                for (int m = NISSAN; m < month; m++)
                {
                    elapsedDays += getDaysInJewishMonth(m, year);
                }
            } // Add days in prior months this year
            else
            {
                for (int m = TISHREI; m < month; m++)
                {
                    elapsedDays += getDaysInJewishMonth(m, year);
                }
            }
            return elapsedDays;
        }

        /// <summary>
        /// Creates a Jewish date based on a Jewish year, month and day of month.
        /// </summary>
        /// <param name="jewishYear">
        ///            the Jewish year </param>
        /// <param name="jewishMonth">
        ///            the Jewish month. The method expects a 1 for Nissan ... 12 for Adar and 13 for Adar II. Use the
        ///            constants <seealso cref="#NISSAN"/> ... <seealso cref="#ADAR"/> (or <seealso cref="#ADAR_II"/> for a leap year Adar II) to avoid any
        ///            confusion. </param>
        /// <param name="jewishDayOfMonth">
        ///            the Jewish day of month. If 30 is passed in for a month with only 29 days (for example <seealso cref="#IYAR"/>,
        ///            or <seealso cref="#KISLEV"/> in a year that <seealso cref="#isKislevShort()"/>), the 29th (last valid date of the month)
        ///            will be set </param>
        /// <exception cref="IllegalArgumentException">
        ///             if the day of month is < 1 or > 30, or a year of < 0 is passed in. </exception>
        public JewishDate(int jewishYear, int jewishMonth, int jewishDayOfMonth)
        {
            setJewishDate(jewishYear, jewishMonth, jewishDayOfMonth);
        }

        /// <summary>
        /// Default constructor will set a default date to the current system date.
        /// </summary>
        public JewishDate()
        {
            resetDate();
        }

        /// <summary>
        /// A constructor that initializes the date to the <seealso cref="java.util.Date Date"/> paremeter.
        /// </summary>
        /// <param name="date">
        ///            the <code>Date</code> to set the calendar to </param>
        /// <exception cref="IllegalArgumentException">
        ///             if the date would fall prior to the January 1, 1 AD </exception>
        public JewishDate(DateTime date)
        {
            Date = date;
        }

        /// <summary>
        /// Sets the date based on a <seealso cref="java.util.Calendar Calendar"/> object. Modifies the Jewish date as well.
        /// </summary>
        /// <param name="calendar">
        ///            the <code>Calendar</code> to set the calendar to </param>
        /// <exception cref="IllegalArgumentException">
        ///             if the <seealso cref="Calendar#ERA"/> is <seealso cref="GregorianCalendar#BC"/> </exception>
        public virtual DateTime Date
        {
            set
            {

                //TODO: figure out what to do with this.
                /*if (value..get(DateTime.ERA) == GregorianCalendar..BC)
                {
                    throw new System.ArgumentException("Calendars with a BC era are not supported. The year " + value.Year + " BC is invalid.");
                }*/

                gregorianMonth = value.Month + 1;
                gregorianDayOfMonth = value.Day;
                gregorianYear = value.Year;
                gregorianAbsDate = gregorianDateToAbsDate(gregorianYear, gregorianMonth, gregorianDayOfMonth); // init the date
                absDateToJewishDate();

                dayOfWeek = Math.Abs(gregorianAbsDate % 7) + 1; // set day of week
            }
        }

        /// <summary>
        /// Sets the Gregorian Date, and updates the Jewish date accordingly. Like the Java Calendar A value of 0 is expected
        /// for January.
        /// </summary>
        /// <param name="year">
        ///            the Gregorian year </param>
        /// <param name="month">
        ///            the Gregorian month. Like the Java Calendar, this class expects 0 for January </param>
        /// <param name="dayOfMonth">
        ///            the Gregorian day of month. If this is > the number of days in the month/year, the last valid date of
        ///            the month will be set </param>
        /// <exception cref="IllegalArgumentException">
        ///             if a year of < 1, a month < 0 or > 11 or a day of month < 1 is passed in </exception>
        public virtual void setGregorianDate(int year, int month, int dayOfMonth)
        {
            validateGregorianDate(year, month, dayOfMonth);
            setInternalGregorianDate(year, month + 1, dayOfMonth);
        }

        /// <summary>
        /// Sets the hidden internal representation of the Gregorian date , and updates the Jewish date accordingly. While
        /// public getters and setters have 0 based months matching the Java Calendar classes, This class internally
        /// represents the Gregorian month starting at 1. When this is called it will not adjust the month to match the Java
        /// Calendar classes.
        /// </summary>
        /// <param name="year">
        ///            the </param>
        /// <param name="month"> </param>
        /// <param name="dayOfMonth"> </param>
        private void setInternalGregorianDate(int year, int month, int dayOfMonth)
        {
            // make sure date is a valid date for the given month, if not, set to last day of month
            if (dayOfMonth > getLastDayOfGregorianMonth(month, year))
            {
                dayOfMonth = getLastDayOfGregorianMonth(month, year);
            }
            // init month, date, year
            gregorianMonth = month;
            gregorianDayOfMonth = dayOfMonth;
            gregorianYear = year;

            gregorianAbsDate = gregorianDateToAbsDate(gregorianYear, gregorianMonth, gregorianDayOfMonth); // init date
            absDateToJewishDate();

            dayOfWeek = Math.Abs(gregorianAbsDate % 7) + 1; // set day of week
        }

        /// <summary>
        /// Sets the Jewish Date and updates the Gregorian date accordingly.
        /// </summary>
        /// <param name="year">
        ///            the Jewish year. The year can't be negative </param>
        /// <param name="month">
        ///            the Jewish month starting with Nisan. A value of 1 is expected for Nissan ... 12 for Adar and 13 for
        ///            Adar II. Use the constants <seealso cref="#NISSAN"/> ... <seealso cref="#ADAR"/> (or <seealso cref="#ADAR_II"/> for a leap year Adar
        ///            II) to avoid any confusion. </param>
        /// <param name="dayOfMonth">
        ///            the Jewish day of month. valid values are 1-30. If the day of month is set to 30 for a month that only
        ///            has 29 days, the day will be set as 29. </param>
        /// <exception cref="IllegalArgumentException">
        ///             if a A Jewish date earlier than 18 Teves, 3761 (1/1/1 Gregorian), a month < 1 or > 12 (or 13 on a
        ///             leap year) or the day of month is < 1 or > 30 is passed in </exception>
        public virtual void setJewishDate(int year, int month, int dayOfMonth)
        {
            setJewishDate(year, month, dayOfMonth, 0, 0, 0);
        }

        /// <summary>
        /// Sets the Jewish Date and updates the Gregorian date accordingly.
        /// </summary>
        /// <param name="year">
        ///            the Jewish year. The year can't be negative </param>
        /// <param name="month">
        ///            the Jewish month starting with Nisan. A value of 1 is expected for Nissan ... 12 for Adar and 13 for
        ///            Adar II. Use the constants <seealso cref="#NISSAN"/> ... <seealso cref="#ADAR"/> (or <seealso cref="#ADAR_II"/> for a leap year Adar
        ///            II) to avoid any confusion. </param>
        /// <param name="dayOfMonth">
        ///            the Jewish day of month. valid values are 1-30. If the day of month is set to 30 for a month that only
        ///            has 29 days, the day will be set as 29.
        /// </param>
        /// <param name="hours">
        ///            the hour of the day. Used for Molad calculations </param>
        /// <param name="minutes">
        ///            the minutes. Used for Molad calculations </param>
        /// <param name="chalakim">
        ///            the chalakim/parts. Used for Molad calculations. The chalakim should not exceed 17. Minutes should be
        ///            used for larger numbers.
        /// </param>
        /// <exception cref="IllegalArgumentException">
        ///             if a A Jewish date earlier than 18 Teves, 3761 (1/1/1 Gregorian), a month < 1 or > 12 (or 13 on a
        ///             leap year), the day of month is < 1 or > 30, an hour < 0 or > 23, a minute < 0 > 59 or chalakim < 0 >
        ///             17. For larger a larger number of chalakim such as 793 (TaShTzaG) break the chalakim into minutes (18
        ///             chalakim per minutes, so it would be 44 minutes and 1 chelek in the case of 793 (TaShTzaG). </exception>
        public virtual void setJewishDate(int year, int month, int dayOfMonth, int hours, int minutes, int chalakim)
        {
            validateJewishDate(year, month, dayOfMonth, hours, minutes, chalakim);

            // if 30 is passed for a month that only has 29 days (for example by rolling the month from a month that had 30
            // days to a month that only has 29) set the date to 29th
            if (dayOfMonth > getDaysInJewishMonth(month, year))
            {
                dayOfMonth = getDaysInJewishMonth(month, year);
            }

            jewishMonth = month;
            jewishDay = dayOfMonth;
            jewishYear = year;
            moladHours = hours;
            moladMinutes = minutes;
            moladChalakim = chalakim;

            gregorianAbsDate = jewishDateToAbsDate(jewishYear, jewishMonth, jewishDay); // reset Gregorian date
            absDateToDate(gregorianAbsDate);

            dayOfWeek = Math.Abs(gregorianAbsDate % 7) + 1; // reset day of week
        }

        /// <summary>
        /// Returns this object's date as a java.util.Date object. <b>Note</b>: This class does not have a concept of time.
        /// </summary>
        /// <returns> The <code>Date</code> </returns>
        public virtual DateTime Time
        {
            get
            {
                DateTime cal = new DateTime();
                cal = new DateTime(gregorianYear, gregorianMonth - 1, gregorianDayOfMonth);
                return cal;
            }
        }

        /// <summary>
        /// Resets this date to the current system date.
        /// </summary>
        public virtual void resetDate()
        {
            Date = DateTime.Now;
        }

        /// <summary>
        /// Returns a string containing the Jewish date in the form, "day Month, year" e.g. "21 Shevat, 5729". For more
        /// complex formatting, use the formatter classes.
        /// </summary>
        /// <returns> the Jewish date in the form "day Month, year" e.g. "21 Shevat, 5729" </returns>
        /// <seealso cref= HebrewDateFormatter#format(JewishDate) </seealso>
        public virtual string ToString()
        {
            return (new HebrewDateFormatter()).format(this);
        }

        /// <summary>
        /// Rolls the date forward by 1 day. It modifies both the Gregorian and Jewish dates accordingly. The API does not
        /// currently offer the ability to forward more than one day t a time, or to forward by month or year. If such
        /// manipulation is required use the <seealso cref="Calendar"/> class <seealso cref="Calendar#add(int, int)"/> or
        /// <seealso cref="Calendar#roll(int, int)"/> methods in the following manner.
        /// 
        /// <pre>
        /// <code>
        /// 	Calendar cal = jewishDate.getTime(); // get a java.util.Calendar representation of the JewishDate
        /// 	cal.add(Calendar.MONTH, 3); // add 3 Gregorian months
        /// 	jewishDate.setDate(cal); // set the updated calendar back to this class
        /// </code>
        /// </pre>
        /// </summary>
        /// <seealso cref= #back() </seealso>
        /// <seealso cref= Calendar#add(int, int) </seealso>
        /// <seealso cref= Calendar#roll(int, int) </seealso>
        public virtual void forward()
        {
            // Change Gregorian date
            if (gregorianDayOfMonth == getLastDayOfGregorianMonth(gregorianMonth, gregorianYear))
            {
                // if last day of year
                if (gregorianMonth == 12)
                {
                    gregorianYear++;
                    gregorianMonth = 1;
                    gregorianDayOfMonth = 1;
                }
                else
                {
                    gregorianMonth++;
                    gregorianDayOfMonth = 1;
                }
            } // if not last day of month
            else
            {
                gregorianDayOfMonth++;
            }

            // Change the Jewish Date
            if (jewishDay == DaysInJewishMonth)
            {
                // if it last day of elul (i.e. last day of Jewish year)
                if (jewishMonth == ELUL)
                {
                    jewishYear++;
                    jewishMonth++;
                    jewishDay = 1;
                }
                else if (jewishMonth == getLastMonthOfJewishYear(jewishYear))
                {
                    // if it is the last day of Adar, or Adar II as case may be
                    jewishMonth = NISSAN;
                    jewishDay = 1;
                }
                else
                {
                    jewishMonth++;
                    jewishDay = 1;
                }
            } // if not last date of month
            else
            {
                jewishDay++;
            }

            if (dayOfWeek == 7) // if last day of week, loop back to Sunday
            {
                dayOfWeek = 1;
            }
            else
            {
                dayOfWeek++;
            }

            gregorianAbsDate++; // increment the absolute date
        }

        /// <summary>
        /// Rolls the date back by 1 day. It modifies both the Gregorian and Jewish dates accordingly. The API does not
        /// currently offer the ability to forward more than one day t a time, or to forward by month or year. If such
        /// manipulation is required use the <seealso cref="Calendar"/> class <seealso cref="Calendar#add(int, int)"/> or
        /// <seealso cref="Calendar#roll(int, int)"/> methods in the following manner.
        /// 
        /// <pre>
        /// <code>
        /// 	Calendar cal = jewishDate.getTime(); // get a java.util.Calendar representation of the JewishDate
        /// 	cal.add(Calendar.MONTH, -3); // subtract 3 Gregorian months
        /// 	jewishDate.setDate(cal); // set the updated calendar back to this class
        /// </code>
        /// </pre>
        /// </summary>
        /// <seealso cref= #back() </seealso>
        /// <seealso cref= Calendar#add(int, int) </seealso>
        /// <seealso cref= Calendar#roll(int, int) </seealso>
        public virtual void back()
        {
            // Change Gregorian date
            if (gregorianDayOfMonth == 1) // if first day of month
            {
                if (gregorianMonth == 1) // if first day of year
                {
                    gregorianMonth = 12;
                    gregorianYear--;
                }
                else
                {
                    gregorianMonth--;
                }
                // change to last day of previous month
                gregorianDayOfMonth = getLastDayOfGregorianMonth(gregorianMonth, gregorianYear);
            }
            else
            {
                gregorianDayOfMonth--;
            }
            // change Jewish date
            if (jewishDay == 1) // if first day of the Jewish month
            {
                if (jewishMonth == NISSAN)
                {
                    jewishMonth = getLastMonthOfJewishYear(jewishYear);
                } // if Rosh Hashana
                else if (jewishMonth == TISHREI)
                {
                    jewishYear--;
                    jewishMonth--;
                }
                else
                {
                    jewishMonth--;
                }
                jewishDay = DaysInJewishMonth;
            }
            else
            {
                jewishDay--;
            }

            if (dayOfWeek == 1) // if first day of week, loop back to Saturday
            {
                dayOfWeek = 7;
            }
            else
            {
                dayOfWeek--;
            }
            gregorianAbsDate--; // change the absolute date
        }

        /// <seealso cref= Object#equals(Object) </seealso>
        public virtual bool Equals(object @object)
        {
            if (this == @object)
            {
                return true;
            }
            if (!(@object is JewishDate))
            {
                return false;
            }
            JewishDate jewishDate = (JewishDate)@object;
            return gregorianAbsDate == jewishDate.AbsDate;
        }

        public object Clone()
        {
            return ObjectCopierExtensions.Clone(this);
        }

        /// <summary>
        /// Compares two dates as per the compareTo() method in the Comparable interface. Returns a value less than 0 if this
        /// date is "less than" (before) the date, greater than 0 if this date is "greater than" (after) the date, or 0 if
        /// they are equal.
        /// </summary>
        public virtual int CompareTo(JewishDate jewishDate)
        {
            return gregorianAbsDate < jewishDate.AbsDate ? -1 : gregorianAbsDate > jewishDate.AbsDate ? 1 : 0;
        }

        /// <summary>
        /// Returns the Gregorian month (between 0-11).
        /// </summary>
        /// <returns> the Gregorian month (between 0-11). Like the java.util.Calendar, months are 0 based. </returns>
        public virtual int GregorianMonth
        {
            get
            {
                return gregorianMonth - 1;
            }
            set
            {
                validateGregorianMonth(value);
                setInternalGregorianDate(gregorianYear, value + 1, gregorianDayOfMonth);
            }
        }

        /// <summary>
        /// Returns the Gregorian day of the month.
        /// </summary>
        /// <returns> the Gregorian day of the mont </returns>
        public virtual int GregorianDayOfMonth
        {
            get
            {
                return gregorianDayOfMonth;
            }
            set
            {
                validateGregorianDayOfMonth(value);
                setInternalGregorianDate(gregorianYear, gregorianMonth, value);
            }
        }

        /// <summary>
        /// Returns the Gregotian year.
        /// </summary>
        /// <returns> the Gregorian year </returns>
        public virtual int GregorianYear
        {
            get
            {
                return gregorianYear;
            }
            set
            {
                validateGregorianYear(value);
                setInternalGregorianDate(value, gregorianMonth, gregorianDayOfMonth);
            }
        }

        /// <summary>
        /// Returns the Jewish month 1-12 (or 13 years in a leap year). The month count starts with 1 for Nisan and goes to
        /// 13 for Adar II
        /// </summary>
        /// <returns> the Jewish month from 1 to 12 (or 13 years in a leap year). The month count starts with 1 for Nisan and
        ///         goes to 13 for Adar II </returns>
        public virtual int JewishMonth
        {
            get
            {
                return jewishMonth;
            }
            set
            {
                setJewishDate(jewishYear, value, jewishDay);
            }
        }

        /// <summary>
        /// Returns the Jewish day of month.
        /// </summary>
        /// <returns> the Jewish day of the month </returns>
        public virtual int JewishDayOfMonth
        {
            get
            {
                return jewishDay;
            }
            set
            {
                setJewishDate(jewishYear, jewishMonth, value);
            }
        }

        /// <summary>
        /// Returns the Jewish year.
        /// </summary>
        /// <returns> the Jewish year </returns>
        public virtual int JewishYear
        {
            get
            {
                return jewishYear;
            }
            set
            {
                setJewishDate(value, jewishMonth, jewishDay);
            }
        }

        /// <summary>
        /// Returns the day of the week as a number between 1-7.
        /// </summary>
        /// <returns> the day of the week as a number between 1-7. </returns>
        public virtual int DayOfWeek
        {
            get
            {
                return dayOfWeek;
            }
        }
    }

}