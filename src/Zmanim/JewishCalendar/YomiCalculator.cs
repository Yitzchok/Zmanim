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

using System;

namespace Zmanim.JewishCalendar
{

    /// <summary>
    /// This class calculates the Daf Yomi page (daf) for a given date. The class currently only supports Daf Yomi Bavli, but
    /// may cover Yerushalmi, Mishna Yomis etc in the future.
    /// 
    /// @author &copy; Bob Newell (original C code)
    /// @author &copy; Eliyahu Hershfeld 2011
    /// @version 0.0.1
    /// </summary>
    public class YomiCalculator
    {

        private static DateTime dafYomiStartDate = new DateTime(1923, 9, 11);
        private static int dafYomiJulianStartDay = GetJulianDay(dafYomiStartDate);
        private static DateTime shekalimChangeDate = new DateTime(1975, 6, 24);
        private static int shekalimJulianChangeDay = GetJulianDay(shekalimChangeDate);

        /// <summary>
        /// Returns the <a href="http://en.wikipedia.org/wiki/Daf_yomi">Daf Yomi</a> <a
        /// href="http://en.wikipedia.org/wiki/Talmud">Bavli</a> <seealso cref="Daf"/> for a given date. The first Daf Yomi cycle
        /// started on Rosh Hashana 5684 (September 11, 1923) and calculations prior to this date will result in an
        /// IllegalArgumentException thrown. For historical calculations (supported by this method), it is important to note
        /// that a change in length of the cycle was instituted starting in the eighth Daf Yomi cycle beginning on June 24,
        /// 1975. The Daf Yomi Bavli cycle has a single masechta of the Talmud Yerushalmi - Shekalim as part of the cycle.
        /// Unlike the Bavli where the number of daf per masechta was standardized since the original <a
        /// href="http://en.wikipedia.org/wiki/Daniel_Bomberg">Bomberg Edition</a> published from 1520 - 1523, there is no
        /// uniform page length in the Yerushalmi. The early cycles had the Yerushalmi Shekalim length of 13 days following
        /// the <a href="http://en.wikipedia.org/wiki/Zhytomyr">Zhytomyr</a> Shas used by <a
        /// href="http://en.wikipedia.org/wiki/Meir_Shapiro">Rabbi Meir Shapiro</a>. With the start of the eighth Daf Yomi
        /// cycle beginning on June 24, 1975 the length of the Yerushalmi shekalim was changed from 13 to 22 daf to follow
        /// the Vilna Shas that is in common use today.
        /// </summary>
        /// <param name="calendar">
        ///            the calendar date for calculation </param>
        /// <returns> the <seealso cref="Daf"/>.
        /// </returns>
        /// <exception cref="IllegalArgumentException">
        ///             if the date is prior to the September 11, 1923 start date of the first Daf Yomi cycle </exception>
        public static Daf GetDafYomiBavli(DateTime date)
        {
            /*
             * The number of daf per masechta. Since the number of blatt in Shekalim changed on the 8th Daf Yomi cycle
             * beginning on June 24, 1975 from 13 to 22, the actual calculation for blattPerMasechta[4] will later be
             * adjusted based on the cycle.
             */
            int[] blattPerMasechta = { 64, 157, 105, 121, 22, 88, 56, 40, 35, 31, 32, 29, 27, 122, 112, 91, 66, 49, 90, 82, 119, 119, 176, 113, 24, 49, 76, 14, 120, 110, 142, 61, 34, 34, 28, 22, 4, 9, 5, 73 };


            Daf dafYomi = null;
            int julianDay = GetJulianDay(date);
            int cycleNo = 0;
            int dafNo = 0;
            if (date < dafYomiStartDate)
            {
                // TODO: should we return a null or throw an IllegalArgumentException?
                throw new System.ArgumentException(date + " is prior to organized Daf Yomi Bavli cycles that started on " + dafYomiStartDate);
            }
            if (date.Equals(shekalimChangeDate) || date > shekalimChangeDate)
            {
                cycleNo = 8 + ((julianDay - shekalimJulianChangeDay) / 2711);
                dafNo = ((julianDay - shekalimJulianChangeDay) % 2711);
            }
            else
            {
                cycleNo = 1 + ((julianDay - dafYomiJulianStartDay) / 2702);
                dafNo = ((julianDay - dafYomiJulianStartDay) % 2702);
            }

            int total = 0;
            int masechta = -1;
            int blatt = 0;

            /* Fix Shekalim for old cycles. */
            if (cycleNo <= 7)
            {
                blattPerMasechta[4] = 13;
            }
            else
            {
                blattPerMasechta[4] = 22; // correct any change that may have been changed from a prior calculation
            }
            /* Finally find the daf. */
            for (int j = 0; j < blattPerMasechta.Length; j++)
            {
                masechta++;
                total = total + blattPerMasechta[j] - 1;
                if (dafNo < total)
                {
                    blatt = 1 + blattPerMasechta[j] - (total - dafNo);
                    /* Fiddle with the weird ones near the end. */
                    if (masechta == 36)
                    {
                        blatt += 21;
                    }
                    else if (masechta == 37)
                    {
                        blatt += 24;
                    }
                    else if (masechta == 38)
                    {
                        blatt += 32;
                    }

                    bool isWithNextMasechta = IsCurrentBlattWithNextMasechta(masechta, blatt);


                    dafYomi = new Daf(masechta, blatt, isWithNextMasechta);
                    break;
                }
            }

            return dafYomi;
        }

        private static bool IsCurrentBlattWithNextMasechta(int masechta, int blatt)
        {
            return masechta == 35 && blatt == 22 || masechta == 36 && blatt == 25;
        }

        /// <summary>
        /// Return the <a href="http://en.wikipedia.org/wiki/Julian_day">Julian day</a> from a Java Date.
        /// </summary>
        /// <param name="date">
        ///            The Java Date </param>
        /// <returns> the Julian day number corresponding to the date </returns>
        private static int GetJulianDay(DateTime date)
        {
            DateTime calendar = new DateTime();
            calendar = date;
            int year = calendar.Year;
            int month = calendar.Month;
            int day = calendar.Day;
            if (month <= 2)
            {
                year -= 1;
                month += 12;
            }
            int a = year / 100;
            int b = 2 - a + a / 4;
            return (int)(Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1)) + day + b - 1524.5);
        }
    }
}