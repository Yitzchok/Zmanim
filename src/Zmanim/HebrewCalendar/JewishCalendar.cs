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
using Zmanim.TimeZone;
using Zmanim.Utilities;
using Zmanim.Extensions;

namespace Zmanim.HebrewCalendar
{
    /// <summary>
    /// The JewishCalendar extends the JewishDate class and adds calendar methods.
    /// 
    /// This open source Java code was originally ported by <a href="http://www.facebook.com/avromf">Avrom Finkelstien</a>
    /// from his C++ code. It was refactored to fit the KosherJava Zmanim API with simplification of the code, enhancements
    /// and some bug fixing.
    /// 
    /// The methods used to obtain the parsha were derived from the source code of <a
    /// href="http://www.sadinoff.com/hebcal/">HebCal</a> by Danny Sadinoff and JCal for the Mac by Frank Yellin. Both based
    /// their code on routines by Nachum Dershowitz and Edward M. Reingold. The class allows setting whether the parsha and
    /// holiday scheme follows the Israel scheme or outside Israel scheme. The default is the outside Israel scheme.
    /// 
    /// TODO: Some do not belong in this class, but here is a partial list of what should still be implemented in some form:
    /// <ol>
    /// <li>Add Isru Chag</li>
    /// <li>Add special parshiyos (shekalim, parah, zachor and hachodesh</li>
    /// <li>Shabbos Mevarchim</li>
    /// <li>Haftorah (various minhagim)</li>
    /// <li>Daf Yomi Yerushalmi, Mishna yomis etc)</li>
    /// <li>Support showing the upcoming parsha for the middle of the week</li>
    /// </ol>
    /// </summary>
    /// <seealso cref= java.util.Date </seealso>
    /// <seealso cref= java.util.Calendar
    /// @author &copy; Avrom Finkelstien 2002
    /// @author &copy; Eliyahu Hershfeld 2011 - 2012
    /// @version 0.0.1 </seealso>
    public class JewishCalendar : JewishDate, ICloneable
    {
        public const int EREV_PESACH = 0;
        public const int PESACH = 1;
        public const int CHOL_HAMOED_PESACH = 2;
        public const int PESACH_SHENI = 3;
        public const int EREV_SHAVUOS = 4;
        public const int SHAVUOS = 5;
        public const int SEVENTEEN_OF_TAMMUZ = 6;
        public const int TISHA_BEAV = 7;
        public const int TU_BEAV = 8;
        public const int EREV_ROSH_HASHANA = 9;
        public const int ROSH_HASHANA = 10;
        public const int FAST_OF_GEDALYAH = 11;
        public const int EREV_YOM_KIPPUR = 12;
        public const int YOM_KIPPUR = 13;
        public const int EREV_SUCCOS = 14;
        public const int SUCCOS = 15;
        public const int CHOL_HAMOED_SUCCOS = 16;
        public const int HOSHANA_RABBA = 17;
        public const int SHEMINI_ATZERES = 18;
        public const int SIMCHAS_TORAH = 19;
        // public static final int EREV_CHANUKAH = 20;// probably remove this
        public const int CHANUKAH = 21;
        public const int TENTH_OF_TEVES = 22;
        public const int TU_BESHVAT = 23;
        public const int FAST_OF_ESTHER = 24;
        public const int PURIM = 25;
        public const int SHUSHAN_PURIM = 26;
        public const int PURIM_KATAN = 27;
        public const int ROSH_CHODESH = 28;
        public const int YOM_HASHOAH = 29;
        public const int YOM_HAZIKARON = 30;
        public const int YOM_HAATZMAUT = 31;
        public const int YOM_YERUSHALAYIM = 32;

        private bool inIsrael = false;
        private bool useModernHolidays = false;

        /// <summary>
        /// Is this calendar set to return modern Israeli national holidays. By default this value is false. The holidays
        /// are: "Yom HaShoah", "Yom Hazikaron", "Yom Ha'atzmaut" and "Yom Yerushalayim"
        /// </summary>
        /// <returns> the useModernHolidays true if set to return modern Israeli national holidays </returns>
        public virtual bool UseModernHolidays
        {
            get
            {
                return useModernHolidays;
            }
            set
            {
                this.useModernHolidays = value;
            }
        }


        /// <summary>
        /// Default constructor will set a default date to the current system date.
        /// </summary>
        public JewishCalendar()
            : base()
        {
        }

        /// <summary>
        /// A constructor that initializes the date to the <seealso cref="java.util.Calendar Calendar"/> parameter.
        /// </summary>
        /// <param name="calendar">
        ///            the <code>Calendar</code> to set the calendar to </param>
        public JewishCalendar(DateTime calendar)
            : base(calendar)
        {
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
        public JewishCalendar(int jewishYear, int jewishMonth, int jewishDayOfMonth)
            : base(jewishYear, jewishMonth, jewishDayOfMonth)
        {
        }

        /// <summary>
        /// Creates a Jewish date based on a Jewish date and whether in Israel
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
        /// <param name="inIsrael">
        ///            whether in Israel. This affects Yom Tov and Parsha calculations </param>
        public JewishCalendar(int jewishYear, int jewishMonth, int jewishDayOfMonth, bool inIsrael)
            : base(jewishYear, jewishMonth, jewishDayOfMonth)
        {
            InIsrael = inIsrael;
        }

        /// <summary>
        /// Sets whether to use Israel parsha and holiday scheme or not. Default is false.
        /// </summary>
        /// <param name="inIsrael">
        ///            set to true for calculations for Israel </param>
        public virtual bool InIsrael
        {
            set
            {
                this.inIsrael = value;
            }
            get
            {
                return inIsrael;
            }
        }


        /// <summary>
        /// Returns an index of the Jewish holiday or fast day for the current day, or a null if there is no holiday for this
        /// day.
        /// </summary>
        /// <returns> A String containing the holiday name or an empty string if it is not a holiday. </returns>
        public virtual int YomTovIndex
        {
            get
            {
                // check by month (starts from Nissan)
                switch (JewishMonth)
                {
                    case NISSAN:
                        if (JewishDayOfMonth == 14)
                        {
                            return EREV_PESACH;
                        }
                        else if (JewishDayOfMonth == 15 || JewishDayOfMonth == 21 || (!inIsrael && (JewishDayOfMonth == 16 || JewishDayOfMonth == 22)))
                        {
                            return PESACH;
                        }
                        else if (JewishDayOfMonth >= 17 && JewishDayOfMonth <= 20 || (JewishDayOfMonth == 16 && inIsrael))
                        {
                            return CHOL_HAMOED_PESACH;
                        }
                        if (UseModernHolidays && ((JewishDayOfMonth == 26 && DayOfWeek == 5) || (JewishDayOfMonth == 28 && DayOfWeek == 1) || (JewishDayOfMonth == 27 && DayOfWeek == 3) || (JewishDayOfMonth == 27 && DayOfWeek == 5)))
                        {
                            return YOM_HASHOAH;
                        }
                        break;
                    case IYAR:
                        if (UseModernHolidays && ((JewishDayOfMonth == 4 && DayOfWeek == 3) || ((JewishDayOfMonth == 3 || JewishDayOfMonth == 2) && DayOfWeek == 4) || (JewishDayOfMonth == 5 && DayOfWeek == 2)))
                        {
                            return YOM_HAZIKARON;
                        }
                        // if 5 Iyar falls on Wed Yom Haatzmaut is that day. If it fal1s on Friday or Shabbos it is moved back to
                        // Thursday. If it falls on Monday it is moved to Tuesday
                        if (UseModernHolidays && ((JewishDayOfMonth == 5 && DayOfWeek == 4) || ((JewishDayOfMonth == 4 || JewishDayOfMonth == 3) && DayOfWeek == 5) || (JewishDayOfMonth == 6 && DayOfWeek == 3)))
                        {
                            return YOM_HAATZMAUT;
                        }
                        if (JewishDayOfMonth == 14)
                        {
                            return PESACH_SHENI;
                        }
                        if (UseModernHolidays && JewishDayOfMonth == 28)
                        {
                            return YOM_YERUSHALAYIM;
                        }
                        break;
                    case SIVAN:
                        if (JewishDayOfMonth == 5)
                        {
                            return EREV_SHAVUOS;
                        }
                        else if (JewishDayOfMonth == 6 || (JewishDayOfMonth == 7 && !inIsrael))
                        {
                            return SHAVUOS;
                        }
                        break;
                    case TAMMUZ:
                        // push off the fast day if it falls on Shabbos
                        if ((JewishDayOfMonth == 17 && DayOfWeek != 7) || (JewishDayOfMonth == 18 && DayOfWeek == 1))
                        {
                            return SEVENTEEN_OF_TAMMUZ;
                        }
                        break;
                    case AV:
                        // if Tisha B'av falls on Shabbos, push off until Sunday
                        if ((DayOfWeek == 1 && JewishDayOfMonth == 10) || (DayOfWeek != 7 && JewishDayOfMonth == 9))
                        {
                            return TISHA_BEAV;
                        }
                        else if (JewishDayOfMonth == 15)
                        {
                            return TU_BEAV;
                        }
                        break;
                    case ELUL:
                        if (JewishDayOfMonth == 29)
                        {
                            return EREV_ROSH_HASHANA;
                        }
                        break;
                    case TISHREI:
                        if (JewishDayOfMonth == 1 || JewishDayOfMonth == 2)
                        {
                            return ROSH_HASHANA;
                        }
                        else if ((JewishDayOfMonth == 3 && DayOfWeek != 7) || (JewishDayOfMonth == 4 && DayOfWeek == 1))
                        {
                            // push off Tzom Gedalia if it falls on Shabbos
                            return FAST_OF_GEDALYAH;
                        }
                        else if (JewishDayOfMonth == 9)
                        {
                            return EREV_YOM_KIPPUR;
                        }
                        else if (JewishDayOfMonth == 10)
                        {
                            return YOM_KIPPUR;
                        }
                        else if (JewishDayOfMonth == 14)
                        {
                            return EREV_SUCCOS;
                        }
                        if (JewishDayOfMonth == 15 || (JewishDayOfMonth == 16 && !inIsrael))
                        {
                            return SUCCOS;
                        }
                        if (JewishDayOfMonth >= 17 && JewishDayOfMonth <= 20 || (JewishDayOfMonth == 16 && inIsrael))
                        {
                            return CHOL_HAMOED_SUCCOS;
                        }
                        if (JewishDayOfMonth == 21)
                        {
                            return HOSHANA_RABBA;
                        }
                        if (JewishDayOfMonth == 22)
                        {
                            return SHEMINI_ATZERES;
                        }
                        if (JewishDayOfMonth == 23 && !inIsrael)
                        {
                            return SIMCHAS_TORAH;
                        }
                        break;
                    case KISLEV: // no yomtov in CHESHVAN
                        // if (getJewishDayOfMonth() == 24) {
                        // return EREV_CHANUKAH;
                        // } else
                        if (JewishDayOfMonth >= 25)
                        {
                            return CHANUKAH;
                        }
                        break;
                    case TEVES:
                        if (JewishDayOfMonth == 1 || JewishDayOfMonth == 2 || (JewishDayOfMonth == 3 && KislevShort))
                        {
                            return CHANUKAH;
                        }
                        else if (JewishDayOfMonth == 10)
                        {
                            return TENTH_OF_TEVES;
                        }
                        break;
                    case SHEVAT:
                        if (JewishDayOfMonth == 15)
                        {
                            return TU_BESHVAT;
                        }
                        break;
                    case ADAR:
                        if (!JewishLeapYear)
                        {
                            // if 13th Adar falls on Friday or Shabbos, push back to Thursday
                            if (((JewishDayOfMonth == 11 || JewishDayOfMonth == 12) && DayOfWeek == 5) || (JewishDayOfMonth == 13 && !(DayOfWeek == 6 || DayOfWeek == 7)))
                            {
                                return FAST_OF_ESTHER;
                            }
                            if (JewishDayOfMonth == 14)
                            {
                                return PURIM;
                            }
                            else if (JewishDayOfMonth == 15)
                            {
                                return SHUSHAN_PURIM;
                            }
                        } // else if a leap year
                        else
                        {
                            if (JewishDayOfMonth == 14)
                            {
                                return PURIM_KATAN;
                            }
                        }
                        break;
                    case ADAR_II:
                        // if 13th Adar falls on Friday or Shabbos, push back to Thursday
                        if (((JewishDayOfMonth == 11 || JewishDayOfMonth == 12) && DayOfWeek == 5) || (JewishDayOfMonth == 13 && !(DayOfWeek == 6 || DayOfWeek == 7)))
                        {
                            return FAST_OF_ESTHER;
                        }
                        if (JewishDayOfMonth == 14)
                        {
                            return PURIM;
                        }
                        else if (JewishDayOfMonth == 15)
                        {
                            return SHUSHAN_PURIM;
                        }
                        break;
                }
                // if we get to this stage, then there are no holidays for the given date return -1
                return -1;
            }
        }

        /// <summary>
        /// Returns true if the current day is Yom Tov. The method returns false for Chanukah, Erev Yom tov and fast days.
        /// </summary>
        /// <returns> true if the current day is a Yom Tov </returns>
        /// <seealso cref= #isErevYomTov() </seealso>
        /// <seealso cref= #isTaanis() </seealso>
        public virtual bool YomTov
        {
            get
            {
                int holidayIndex = YomTovIndex;
                if (ErevYomTov || holidayIndex == CHANUKAH || (Taanis && holidayIndex != YOM_KIPPUR))
                {
                    return false;
                }
                return YomTovIndex != -1;
            }
        }

        /// <summary>
        /// Returns true if the current day is Chol Hamoed of Pesach or Succos.
        /// </summary>
        /// <returns> true if the current day is Chol Hamoed of Pesach or Succos </returns>
        /// <seealso cref= #isYomTov() </seealso>
        /// <seealso cref= #CHOL_HAMOED_PESACH </seealso>
        /// <seealso cref= #CHOL_HAMOED_SUCCOS </seealso>
        public virtual bool CholHamoed
        {
            get
            {
                int holidayIndex = YomTovIndex;
                return holidayIndex == JewishCalendar.CHOL_HAMOED_PESACH || holidayIndex == JewishCalendar.CHOL_HAMOED_SUCCOS;
            }
        }

        /// <summary>
        /// Returns true if the current day is erev Yom Tov. The method returns true for Erev - Pesach, Shavuos, Rosh
        /// Hashana, Yom Kippur and Succos.
        /// </summary>
        /// <returns> true if the current day is Erev - Pesach, Shavuos, Rosh Hashana, Yom Kippur and Succos </returns>
        /// <seealso cref= #isYomTov() </seealso>
        public virtual bool ErevYomTov
        {
            get
            {
                int holidayIndex = YomTovIndex;
                return holidayIndex == EREV_PESACH || holidayIndex == EREV_SHAVUOS || holidayIndex == EREV_ROSH_HASHANA || holidayIndex == EREV_YOM_KIPPUR || holidayIndex == EREV_SUCCOS;
            }
        }

        /// <summary>
        /// Returns true if the current day is Erev Rosh Chodesh. Returns false for Erev Rosh Hashana
        /// </summary>
        /// <returns> true if the current day is Erev Rosh Chodesh. Returns false for Erev Rosh Hashana </returns>
        /// <seealso cref= #isRoshChodesh() </seealso>
        public virtual bool ErevRoshChodesh
        {
            get
            {
                // Erev Rosh Hashana is not Erev Rosh Chodesh.
                return (JewishDayOfMonth == 29 && JewishMonth != ELUL);
            }
        }

        /// <summary>
        /// Return true if the day is a Taanis (fast day). Return true for 17 of Tammuz, Tisha B'Av, Yom Kippur, Fast of
        /// Gedalyah, 10 of Teves and the Fast of Esther
        /// </summary>
        /// <returns> true if today is a fast day </returns>
        public virtual bool Taanis
        {
            get
            {
                int holidayIndex = YomTovIndex;
                return holidayIndex == SEVENTEEN_OF_TAMMUZ || holidayIndex == TISHA_BEAV || holidayIndex == YOM_KIPPUR || holidayIndex == FAST_OF_GEDALYAH || holidayIndex == TENTH_OF_TEVES || holidayIndex == FAST_OF_ESTHER;
            }
        }

        /// <summary>
        /// Returns the day of Chanukah or -1 if it is not Chanukah.
        /// </summary>
        /// <returns> the day of Chanukah or -1 if it is not Chanukah. </returns>
        public virtual int DayOfChanukah
        {
            get
            {
                if (Chanukah)
                {
                    if (JewishMonth == KISLEV)
                    {
                        return JewishDayOfMonth - 24;
                    } // teves
                    else
                    {
                        return KislevShort ? JewishDayOfMonth + 5 : JewishDayOfMonth + 6;
                    }
                }
                else
                {
                    return -1;
                }
            }
        }

        public virtual bool Chanukah
        {
            get
            {
                return YomTovIndex == CHANUKAH;
            }
        }

        // These indices were originally included in the emacs 19 distribution.
        // These arrays determine the correct indices into the parsha names
        // -1 means no parsha that week, values > 52 means it is a double parsha
        private static readonly int[] Sat_short = { -1, 52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 53, 23, 24, -1, 25, 54, 55, 30, 56, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 50 };

        private static readonly int[] Sat_long = { -1, 52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 53, 23, 24, -1, 25, 54, 55, 30, 56, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 59 };

        private static readonly int[] Mon_short = { 51, 52, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 53, 23, 24, -1, 25, 54, 55, 30, 56, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 59 };

        private static readonly int[] Mon_long = { 51, 52, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 53, 23, 24, -1, 25, 54, 55, 30, 56, 33, -1, 34, 35, 36, 37, 57, 40, 58, 43, 44, 45, 46, 47, 48, 49, 59 }; // split

        private static readonly int[] Thu_normal = { 52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 53, 23, 24, -1, -1, 25, 54, 55, 30, 56, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 50 };
        private static readonly int[] Thu_normal_Israel = { 52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 53, 23, 24, -1, 25, 54, 55, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 50 };

        private static readonly int[] Thu_long = { 52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, -1, 25, 54, 55, 30, 56, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 50 };

        private static readonly int[] Sat_short_leap = { -1, 52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, -1, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 59 };

        private static readonly int[] Sat_long_leap = { -1, 52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, -1, 28, 29, 30, 31, 32, 33, -1, 34, 35, 36, 37, 57, 40, 58, 43, 44, 45, 46, 47, 48, 49, 59 };

        private static readonly int[] Mon_short_leap = { 51, 52, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, -1, 28, 29, 30, 31, 32, 33, -1, 34, 35, 36, 37, 57, 40, 58, 43, 44, 45, 46, 47, 48, 49, 59 };
        private static readonly int[] Mon_short_leap_Israel = { 51, 52, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, -1, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 59 };

        private static readonly int[] Mon_long_leap = { 51, 52, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, -1, -1, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 58, 43, 44, 45, 46, 47, 48, 49, 50 };
        private static readonly int[] Mon_long_leap_Israel = { 51, 52, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, -1, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 };

        private static readonly int[] Thu_short_leap = { 52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, -1, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 };

        private static readonly int[] Thu_long_leap = { 52, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, -1, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 59 };

        /// <summary>
        /// Returns a the index of today's parsha(ios) or a -1 if there is none. To get the name of the Parsha, use the
        /// <seealso cref="HebrewDateFormatter#formatParsha(JewishCalendar)"/>.
        /// 
        /// TODO: consider possibly return the parsha of the week for any day during the week instead of empty. To do this
        /// the simple way, create a new instance of the class in the mothod, roll it to the next shabbos. If the shabbos has
        /// no parsha, keep rolling by a week till a parsha is encountered. Possibly turn into static method that takes in a
        /// year, month, day, roll to next shabbos (not that simple with the API for date passed in) and if it is not a
        /// shabbos roll forwarde one week at a time to get the parsha. I do not think it is possible to have more than 2
        /// shabbosim in a row without a parsha, but I may be wrong.
        /// </summary>
        /// <returns> the string of the parsha. Will currently return blank for weekdays and a shabbos on a yom tov. </returns>
        public virtual int ParshaIndex
        {
            get
            {
                // if today is not Shabbos, then there is no normal parsha reading. If
                // commented our will return LAST week's parsha for a non shabbos
                if (DayOfWeek != 7)
                {
                    // return "";
                    return -1;
                }

                // kvia = whether a Jewish year is short/regular/long (0/1/2)
                // roshHashana = Rosh Hashana of this Jewish year
                // roshHashanaDay= day of week Rosh Hashana was on this year
                // week= current week in Jewish calendar from Rosh Hashana
                // array= the correct index array for this Jewish year
                // index= the index number of the parsha name
                int kvia = CheshvanKislevKviah;
                int roshHashanaDay;
                int week;
                int[] array = null;
                int index;

                JewishDate roshHashana = new JewishDate(JewishYear, TISHREI, 1); // set it to Rosh Hashana of this year

                // get day Rosh Hashana was on
                roshHashanaDay = roshHashana.DayOfWeek;

                // week is the week since the first Shabbos on or after Rosh Hashana
                week = (((AbsDate - roshHashana.AbsDate) - (7 - roshHashanaDay)) / 7);

                // determine appropriate array
                if (!JewishLeapYear)
                {
                    switch (roshHashanaDay)
                    {
                        case 7: // RH was on a Saturday
                            if (kvia == CHASERIM)
                            {
                                array = Sat_short;
                            }
                            else if (kvia == SHELAIMIM)
                            {
                                array = Sat_long;
                            }
                            break;
                        case 2: // RH was on a Monday
                            if (kvia == CHASERIM)
                            {
                                array = Mon_short;
                            }
                            else if (kvia == SHELAIMIM)
                            {
                                array = inIsrael ? Mon_short : Mon_long;
                            }
                            break;
                        case 3: // RH was on a Tuesday
                            if (kvia == KESIDRAN)
                            {
                                array = inIsrael ? Mon_short : Mon_long;
                            }
                            break;
                        case 5: // RH was on a Thursday
                            if (kvia == KESIDRAN)
                            {
                                array = inIsrael ? Thu_normal_Israel : Thu_normal;
                            }
                            else if (kvia == SHELAIMIM)
                            {
                                array = Thu_long;
                            }
                            break;
                    }
                } // if leap year
                else
                {
                    switch (roshHashanaDay)
                    {
                        case 7: // RH was on a Sat
                            if (kvia == CHASERIM)
                            {
                                array = Sat_short_leap;
                            }
                            else if (kvia == SHELAIMIM)
                            {
                                array = inIsrael ? Sat_short_leap : Sat_long_leap;
                            }
                            break;
                        case 2: // RH was on a Mon
                            if (kvia == CHASERIM)
                            {
                                array = inIsrael ? Mon_short_leap_Israel : Mon_short_leap;
                            }
                            else if (kvia == SHELAIMIM)
                            {
                                array = inIsrael ? Mon_long_leap_Israel : Mon_long_leap;
                            }
                            break;
                        case 3: // RH was on a Tue
                            if (kvia == KESIDRAN)
                            {
                                array = inIsrael ? Mon_long_leap_Israel : Mon_long_leap;
                            }
                            break;
                        case 5: // RH was on a Thu
                            if (kvia == CHASERIM)
                            {
                                array = Thu_short_leap;
                            }
                            else if (kvia == SHELAIMIM)
                            {
                                array = Thu_long_leap;
                            }
                            break;
                    }
                }
                // if something goes wrong
                if (array == null)
                {
                    throw new Exception("Unable to claculate the parsha. No index array matched any of the known types for the date: " + ToString());
                }
                // get index from array
                index = array[week];

                // If no Parsha this week
                // if (index == -1) {
                // return -1;
                // }

                // if parsha this week
                // else {
                // if (getDayOfWeek() != 7){//in weekday return next shabbos's parsha
                // System.out.print(" index=" + index + " ");
                // return parshios[index + 1];
                // this code returns odd data for yom tov. See parshas kedoshim display
                // for 2011 for example. It will also break for Sept 25, 2011 where it
                // goes one beyong the index of Nitzavim-Vayelech
                // }
                // return parshios[index];
                return index;
                // }
            }
        }

        /// <summary>
        /// Returns if the day is Rosh Chodesh. Rosh Hashana will return false
        /// </summary>
        /// <returns> true if it is Rosh Chodesh. Rosh Hashana will return false </returns>
        public virtual bool RoshChodesh
        {
            get
            {
                // Rosh Hashana is not rosh chodesh. Elul never has 30 days
                return (JewishDayOfMonth == 1 && JewishMonth != TISHREI) || JewishDayOfMonth == 30;
            }
        }

        /// <summary>
        /// Returns the int value of the Omer day or -1 if the day is not in the omer
        /// </summary>
        /// <returns> The Omer count as an int or -1 if it is not a day of the Omer. </returns>
        public virtual int DayOfOmer
        {
            get
            {
                int omer = -1; // not a day of the Omer

                // if Nissan and second day of Pesach and on
                if (JewishMonth == NISSAN && JewishDayOfMonth >= 16)
                {
                    omer = JewishDayOfMonth - 15;
                    // if Iyar
                }
                else if (JewishMonth == IYAR)
                {
                    omer = JewishDayOfMonth + 15;
                    // if Sivan and before Shavuos
                }
                else if (JewishMonth == SIVAN && JewishDayOfMonth < 6)
                {
                    omer = JewishDayOfMonth + 44;
                }
                return omer;
            }
        }

        /// <summary>
        /// Returns the molad in Standard Time in Yerushalayim as a Date. The traditional calculation uses local time. This
        /// method subtracts 20.94 minutes (20 minutes and 56.496 seconds) from the local time (Har Habayis with a longitude
        /// of 35.2354&deg; is 5.2354&deg; away from the %15 timezone longitude) to get to standard time. This method
        /// intentionally uses standard time and not dailight savings time. Java will implicitly format the time to the
        /// default (or set) Timezone.
        /// </summary>
        /// <returns> the Date representing the moment of the molad in Yerushalayim standard time (GMT + 2) </returns>
        public virtual DateTime MoladAsDate
        {
            get
            {
                JewishDate molad = Molad;
                string locationName = "Jerusalem, Israel";

                double latitude = 31.778; // Har Habayis latitude
                double longitude = 35.2354; // Har Habayis longitude

                // The molad calculation always extepcst output in standard time. Using "Asia/Jerusalem" timezone will incorrect
                // adjust for DST.
                ITimeZone yerushalayimStandardTZ = new OffsetTimeZone(2);
                GeoLocation geo = new GeoLocation(locationName, latitude, longitude, yerushalayimStandardTZ);

                double moladSeconds = molad.MoladChalakim * 10 / (double)3;
                var cal = new DateTime(molad.GregorianYear, molad.GregorianMonth, molad.GregorianDayOfMonth,
                    molad.MoladHours, molad.MoladMinutes, (int)moladSeconds, (int)(1000 * (moladSeconds - (int)moladSeconds)));

                // subtract local time difference of 20.94 minutes (20 minutes and 56.496 seconds) to get to Standard time
                return cal.AddMilliseconds(-1 * (int)geo.GetLocalMeanTimeOffset(cal));
            }
        }

        /// <summary>
        /// Returns the earliest time of Kiddush Levana calculated as 3 days after the molad. TODO: Currently returns the
        /// time even if it is during the day. It should return the
        /// <seealso cref="net.sourceforge.zmanim.ZmanimCalendar#getTzais72() Tzais"/> after to the time if the zman is between Alos
        /// and Tzais.
        /// </summary>
        /// <returns> the Date representing the moment 3 days after the molad. </returns>
        public virtual DateTime TchilasZmanKidushLevana3Days
        {
            get
            {
                DateTime molad = MoladAsDate;
                DateTime cal = new DateTime();
                cal = molad;
                cal.AddHours(72); // 3 days after the molad
                return cal;
            }
        }

        /// <summary>
        /// Returns the earliest time of Kiddush Levana calculated as 7 days after the molad as mentioned by the <a
        /// href="http://en.wikipedia.org/wiki/Yosef_Karo">Mechaber</a>. See the <a
        /// href="http://en.wikipedia.org/wiki/Yoel_Sirkis">Bach's</a> opinion on this time. TODO: Currently returns the time
        /// even if it is during the day. It should return the {@link net.sourceforge.zmanim.ZmanimCalendar#getTzais72()
        /// Tzais} after to the time if the zman is between Alos and Tzais.
        /// </summary>
        /// <returns> the Date representing the moment 7 days after the molad. </returns>
        public virtual DateTime TchilasZmanKidushLevana7Days
        {
            get
            {
                DateTime molad = MoladAsDate;
                DateTime cal = new DateTime();
                cal = molad;
                cal.AddHours(168); // 7 days after the molad
                return cal;
            }
        }

        /// <summary>
        /// Returns the latest time of Kiddush Levana according to the <a
        /// href="http://en.wikipedia.org/wiki/Yaakov_ben_Moshe_Levi_Moelin">Maharil's</a> opinion that it is calculated as
        /// halfway between molad and molad. This adds half the 29 days, 12 hours and 793 chalakim time between molad and
        /// molad (14 days, 18 hours, 22 minutes and 666 milliseconds) to the month's molad. TODO: Currently returns the time
        /// even if it is during the day. It should return the <seealso cref="net.sourceforge.zmanim.ZmanimCalendar#getAlos72() Alos"/>
        /// prior to the time if the zman is between Alos and Tzais.
        /// </summary>
        /// <returns> the Date representing the moment halfway between molad and molad. </returns>
        /// <seealso cref= #getSofZmanKidushLevana15Days() </seealso>
        public virtual DateTime SofZmanKidushLevanaBetweenMoldos
        {
            get
            {
                // add half the time between molad and molad (half of 29 days, 12 hours and 793 chalakim (44 minutes, 3.3
                // seconds), or 14 days, 18 hours, 22 minutes and 666 milliseconds)
                return MoladAsDate.Add(new TimeSpan(14, 18, 22, 1, 666));
            }
        }

        /// <summary>
        /// Returns the latest time of Kiddush Levana calculated as 15 days after the molad. This is the opinion brought down
        /// in the Shulchan Aruch (Orach Chaim 426). It should be noted that some opinions hold that the
        /// <http://en.wikipedia.org/wiki/Moses_Isserles">Rema</a> who brings down the opinion of the <a
        /// href="http://en.wikipedia.org/wiki/Yaakov_ben_Moshe_Levi_Moelin">Maharil's</a> of calculating
        /// <seealso cref="#getSofZmanKidushLevanaBetweenMoldos() half way between molad and mold"/> is of the opinion that Mechaber
        /// agrees to his opinion. Also see the Aruch Hashulchan. For additional details on the subject, See Rabbi Dovid
        /// Heber's very detailed writeup in Siman Daled (chapter 4) of <a
        /// href="http://www.worldcat.org/oclc/461326125">Shaarei Zmanim</a>. TODO: Currently returns the time even if it is
        /// during the day. It should return the <seealso cref="net.sourceforge.zmanim.ZmanimCalendar#getAlos72() Alos"/> prior to the
        /// time if the zman is between Alos and Tzais.
        /// </summary>
        /// <returns> the Date representing the moment 15 days after the molad. </returns>
        /// <seealso cref= #getSofZmanKidushLevanaBetweenMoldos() </seealso>
        public virtual DateTime SofZmanKidushLevana15Days
        {
            get
            {
                DateTime molad = MoladAsDate;
                DateTime cal = new DateTime();
                cal = molad;
                cal.AddDays(15); // 15 days after the molad
                return cal;
            }
        }

        /// <summary>
        /// Returns the Daf Yomi (Bavli) for the date that the calendar is set to. See the
        /// <seealso cref="HebrewDateFormatter#formatDafYomiBavli(Daf)"/> for the ability to format the daf in Hebrew or transliterated
        /// masechta names.
        /// </summary>
        /// <returns> the daf as a <seealso cref="Daf"/> </returns>
        public virtual Daf DafYomiBavli
        {
            get
            {
                return YomiCalculator.getDafYomiBavli(this);
            }
        }

        /// <seealso cref= Object#equals(Object) </seealso>
        public virtual bool Equals(object @object)
        {
            if (this == @object)
            {
                return true;
            }
            if (!(@object is JewishCalendar))
            {
                return false;
            }
            JewishCalendar jewishCalendar = (JewishCalendar)@object;
            return AbsDate == jewishCalendar.AbsDate && InIsrael == jewishCalendar.InIsrael;
        }

        /// <seealso cref= Object#hashCode() </seealso>
        public virtual int GetHashCode()
        {
            int result = 17;
            result = 37 * result + this.GetType().GetHashCode(); // needed or this and subclasses will return identical hash
            result += 37 * result + AbsDate + (InIsrael ? 1 : 3);
            return result;
        }

        public object Clone()
        {
            return ObjectCopierExtensions.Clone(this);
        }
    }

}