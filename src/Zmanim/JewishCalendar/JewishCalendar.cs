using System;
using System.Globalization;

namespace Zmanim
{
	/// <summary>
	/// Jewish calendar. Extends HebrewCalendar to provide things like Jewish info (holidays etc.) and normalized months
	/// </summary>
	public class JewishCalendar: HebrewCalendar
	{
		/// <summary>
		/// Normalizes the months as 1-13 (1-12, with 13 being adar bet)
		/// This is different than the native HebrewCalendar month index 
		/// which changes the index of several months depending on whether or not it is a leap year
		/// see: https://msdn.microsoft.com/en-us/library/system.globalization.hebrewcalendar(v=vs.110).aspx
		/// </summary>
		public enum HebrewMonth
		{
			/// <summary>
			/// Value of the month field indicating Nissan, the first numeric month of the year in the Jewish calendar. With the
			/// year starting at <seealso cref="TISHREI"/>, it would actually be the 7th (or 8th in a {@link #IsLeapYearFromDateTime leap
			/// year}) month of the year.
			/// </summary>
			NISSAN = 1,
			/// <summary>
			/// Value of the month field indicating Iyar, the second numeric month of the year in the Jewish calendar. With the
			/// year starting at <seealso cref="TISHREI"/>, it would actually be the 8th (or 9th in a {@link #IsLeapYearFromDateTime leap
			/// year}) month of the year.
			/// </summary>
			IYAR,
			/// <summary>
			/// Value of the month field indicating Sivan, the third numeric month of the year in the Jewish calendar. With the
			/// year starting at <seealso cref="TISHREI"/>, it would actually be the 9th (or 10th in a {@link #IsLeapYearFromDateTime leap
			/// year}) month of the year.
			/// </summary>
			SIVAN,
			/// <summary>
			/// Value of the month field indicating Tammuz, the fourth numeric month of the year in the Jewish calendar. With the
			/// year starting at <seealso cref="TISHREI"/>, it would actually be the 10th (or 11th in a {@link #IsLeapYearFromDateTime leap
			/// year}) month of the year.
			/// </summary>
			TAMMUZ,
			/// <summary>
			/// Value of the month field indicating Av, the fifth numeric month of the year in the Jewish calendar. With the year
			/// starting at <seealso cref="TISHREI"/>, it would actually be the 11th (or 12th in a <seealso cref="IsLeapYearFromDateTime"/>)
			/// month of the year.
			/// </summary>
			AV,
			/// <summary>
			/// Value of the month field indicating Elul, the sixth numeric month of the year in the Jewish calendar. With the
			/// year starting at <seealso cref="TISHREI"/>, it would actually be the 12th (or 13th in a {@link #IsLeapYearFromDateTime leap
			/// year}) month of the year.
			/// </summary>
			ELUL,
			/// <summary>
			/// Value of the month field indicating Tishrei, the seventh numeric month of the year in the Jewish calendar. With
			/// the year starting at this month, it would actually be the 1st month of the year.
			/// </summary>
			TISHREI,
			/// <summary>
			/// Value of the month field indicating Cheshvan/marcheshvan, the eighth numeric month of the year in the Jewish
			/// calendar. With the year starting at <seealso cref="TISHREI"/>, it would actually be the 2nd month of the year.
			/// </summary>
			CHESHVAN,
			/// <summary>
			/// Value of the month field indicating Kislev, the ninth numeric month of the year in the Jewish calendar. With the
			/// year starting at <seealso cref="TISHREI"/>, it would actually be the 3rd month of the year.
			/// </summary>
			KISLEV,
			/// <summary>
			/// Value of the month field indicating Teves, the tenth numeric month of the year in the Jewish calendar. With the
			/// year starting at <seealso cref="TISHREI"/>, it would actually be the 4th month of the year.
			/// </summary>
			TEVES,
			/// <summary>
			/// Value of the month field indicating Shevat, the eleventh numeric month of the year in the Jewish calendar. With
			/// the year starting at <seealso cref="TISHREI"/>, it would actually be the 5th month of the year.
			/// </summary>
			SHEVAT,
			/// <summary>
			/// Value of the month field indicating Adar (or Adar I in a <seealso cref="IsLeapYearFromDateTime"/>), the twelfth
			/// numeric month of the year in the Jewish calendar. With the year starting at <seealso cref="TISHREI"/>, it would actually
			/// be the 6th month of the year.
			/// </summary>
			ADAR,
			/// <summary>
			/// Value of the month field indicating Adar II, the leap (intercalary or embolismic) thirteenth (Undecimber) numeric
			/// month of the year added in Jewish <seealso cref="IsLeapYearFromDateTime"/>). The leap years are years 3, 6, 8, 11,
			/// 14, 17 and 19 of a 19 year cycle. With the year starting at <seealso cref="TISHREI"/>, it would actually be the 7th month
			/// of the year.
			/// </summary>
			ADAR_II
		}

		/// <summary>
		/// Jewish holiday.
		/// </summary>
		public enum JewishHoliday {
			NONE = -1,
			EREV_PESACH = 0,
			PESACH = 1,
			CHOL_HAMOED_PESACH = 2,
			PESACH_SHENI = 3,
			EREV_SHAVUOS = 4,
			SHAVUOS = 5,
			SEVENTEEN_OF_TAMMUZ = 6,
			TISHA_BEAV = 7,
			TU_BEAV = 8,
			EREV_ROSH_HASHANA = 9,
			ROSH_HASHANA = 10,
			FAST_OF_GEDALYAH = 11,
			EREV_YOM_KIPPUR = 12,
			YOM_KIPPUR = 13,
			EREV_SUCCOS = 14,
			SUCCOS = 15,
			CHOL_HAMOED_SUCCOS = 16,
			HOSHANA_RABBA = 17,
			SHEMINI_ATZERES = 18,
			SIMCHAS_TORAH = 19,
			// public static final int EREV_CHANUKAH = 20,// probably remove this
			CHANUKAH = 21,
			TENTH_OF_TEVES = 22,
			TU_BESHVAT = 23,
			FAST_OF_ESTHER = 24,
			PURIM = 25,
			SHUSHAN_PURIM = 26,
			PURIM_KATAN = 27,
			ROSH_CHODESH = 28,
			YOM_HASHOAH = 29,
			YOM_HAZIKARON = 30,
			YOM_HAATZMAUT = 31,
			YOM_YERUSHALAYIM = 32
		}

		/// <summary>
		/// see https://msdn.microsoft.com/en-us/library/system.globalization.hebrewcalendar(v=vs.110).aspx
		/// </summary>
		private static HebrewMonth[] internalToNormalizedMonthsRegularYear = { 	HebrewMonth.TISHREI, 
															HebrewMonth.CHESHVAN,
			HebrewMonth.KISLEV,
			HebrewMonth.TEVES,
			HebrewMonth.SHEVAT,
			HebrewMonth.ADAR,
			HebrewMonth.NISSAN,
			HebrewMonth.IYAR,
			HebrewMonth.TAMMUZ,
			HebrewMonth.AV,
			HebrewMonth.ELUL,

		};

		private static HebrewMonth[] internalToNormalizedMonthsLeapYear = { 	HebrewMonth.TISHREI, 
			HebrewMonth.CHESHVAN,
			HebrewMonth.KISLEV,
			HebrewMonth.TEVES,
			HebrewMonth.SHEVAT,
			HebrewMonth.ADAR,
			HebrewMonth.ADAR_II,
			HebrewMonth.NISSAN,
			HebrewMonth.IYAR,
			HebrewMonth.TAMMUZ,
			HebrewMonth.AV,
			HebrewMonth.ELUL,

		};

		/// <summary>
		/// also see https://msdn.microsoft.com/en-us/library/system.globalization.hebrewcalendar(v=vs.110).aspx
		/// pay particular attention to the fact that there are always 14 indexes, and different ones are skipped depending on leap year
		/// </summary>
		private static HebrewMonth[] normalizedToInternalMonthsRegularYear = { 	HebrewMonth.TISHREI, 
			HebrewMonth.CHESHVAN,
			HebrewMonth.KISLEV,
			HebrewMonth.TEVES,
			HebrewMonth.SHEVAT,
			HebrewMonth.ADAR,
			HebrewMonth.NISSAN,
			HebrewMonth.IYAR,
			HebrewMonth.TAMMUZ,
			HebrewMonth.AV,
			HebrewMonth.ELUL,
			HebrewMonth.ELUL, 
			HebrewMonth.ELUL
		};
			
		private static HebrewMonth[] normalizedToInternalMonthsLeapYear = { 	
			HebrewMonth.CHESHVAN,
			HebrewMonth.KISLEV,
			HebrewMonth.TEVES,
			HebrewMonth.SHEVAT,
			HebrewMonth.ADAR,
			HebrewMonth.ADAR_II,
			HebrewMonth.NISSAN,
			HebrewMonth.IYAR,
			HebrewMonth.TAMMUZ,
			HebrewMonth.AV,
			HebrewMonth.ELUL,
			HebrewMonth.ELUL,
			HebrewMonth.TISHREI, 

		};


		/// <summary>
		/// Determines whether this instance is leap year from date time the specified dt.
		/// </summary>
		/// <returns><c>true</c> if this instance is leap year from date time the specified dt; otherwise, <c>false</c>.</returns>
		/// <param name="dt">Dt.</param>
		public bool IsLeapYearFromDateTime(DateTime dt) {
			return IsLeapYear (GetYear (dt));
		}

		/// <summary>
		/// Gets the normalized month.
		/// </summary>
		/// <returns>The normalized month.</returns>
		/// <param name="dt">Dt.</param>
		public HebrewMonth GetNormalizedMonth(DateTime dt) {
			int index = GetMonth (dt)-1;

			if(IsLeapYearFromDateTime(dt)) {
				return internalToNormalizedMonthsLeapYear [index];
			} else {
				return internalToNormalizedMonthsRegularYear [index];
			}
		}

		/// <summary>
		/// Gets the hebrew date time.
		/// </summary>
		/// <returns>The hebrew date time.</returns>
		/// <param name="year">Year.</param>
		/// <param name="month">Month.</param>
		/// <param name="day">Day.</param>
		public DateTime GetHebrewDateTime(int year, HebrewMonth month, int day) {
			DateTime dt;

			int index = (int)month-1;


			if (IsLeapYear (year)) {
				dt = new DateTime(year, (int)normalizedToInternalMonthsLeapYear [index], day, this);
			} else {
				dt = new DateTime(year, (int)normalizedToInternalMonthsRegularYear [index], day, this);
			}
			return dt;
		}


		/// <summary>
		/// Gets the jewish holiday, sets "use modern holidays" to true
		/// </summary>
		/// <returns>The jewish holiday.</returns>
		/// <param name="dt">Dt.</param>
		/// <param name="inIsrael">If set to <c>true</c> in israel.</param>
		public JewishHoliday GetJewishHoliday(DateTime dt, bool inIsrael) {
			return GetJewishHoliday (dt, inIsrael, true);
		}

		/// <summary>
		/// Gets the jewish holiday.
		/// </summary>
		/// <returns>The jewish holiday.</returns>
		/// <param name="dt">Dt.</param>
		/// <param name="inIsrael">If set to <c>true</c> in israel.</param>
		/// <param name="UseModernHolidays">If set to <c>true</c> use modern holidays.</param>
		public JewishHoliday GetJewishHoliday(DateTime dt, bool inIsrael, bool UseModernHolidays)
		{

			HebrewMonth hebrewMonth = GetNormalizedMonth (dt);
			int dayOfMonth = GetDayOfMonth (dt);
			int dayOfWeek = (int)GetDayOfWeek (dt);

			// check by month (starts from Nissan)
			switch (hebrewMonth)
				{
			case HebrewMonth.NISSAN:
					if (dayOfMonth == 14)
					{
					return JewishHoliday.EREV_PESACH;
					}
					else if (dayOfMonth == 15 || dayOfMonth == 21 || (!inIsrael && (dayOfMonth == 16 || dayOfMonth == 22)))
					{
					return JewishHoliday.PESACH;
					}
					else if (dayOfMonth >= 17 && dayOfMonth <= 20 || (dayOfMonth == 16 && inIsrael))
					{
					return JewishHoliday.CHOL_HAMOED_PESACH;
					}
					if (UseModernHolidays && ((dayOfMonth == 26 && dayOfWeek == 5) || (dayOfMonth == 28 && dayOfWeek == 1) || (dayOfMonth == 27 && dayOfWeek == 3) || (dayOfMonth == 27 && dayOfWeek == 5)))
					{
					return JewishHoliday.YOM_HASHOAH;
					}
					break;
			case HebrewMonth.IYAR:
					if (UseModernHolidays && ((dayOfMonth == 4 && dayOfWeek == 3) || ((dayOfMonth == 3 || dayOfMonth == 2) && dayOfWeek == 4) || (dayOfMonth == 5 && dayOfWeek == 2)))
					{
					return JewishHoliday.YOM_HAZIKARON;
					}
					// if 5 Iyar falls on Wed Yom Haatzmaut is that day. If it fal1s on Friday or Shabbos it is moved back to
					// Thursday. If it falls on Monday it is moved to Tuesday
					if (UseModernHolidays && ((dayOfMonth == 5 && dayOfWeek == 4) || ((dayOfMonth == 4 || dayOfMonth == 3) && dayOfWeek == 5) || (dayOfMonth == 6 && dayOfWeek == 3)))
					{
					return JewishHoliday.YOM_HAATZMAUT;
					}
					if (dayOfMonth == 14)
					{
					return JewishHoliday.PESACH_SHENI;
					}
					if (UseModernHolidays && dayOfMonth == 28)
					{
					return JewishHoliday.YOM_YERUSHALAYIM;
					}
					break;
			case HebrewMonth.SIVAN:
					if (dayOfMonth == 5)
					{
					return JewishHoliday.EREV_SHAVUOS;
					}
					else if (dayOfMonth == 6 || (dayOfMonth == 7 && !inIsrael))
					{
					return JewishHoliday.SHAVUOS;
					}
					break;
			case HebrewMonth.TAMMUZ:
					// push off the fast day if it falls on Shabbos
					if ((dayOfMonth == 17 && dayOfWeek != 7) || (dayOfMonth == 18 && dayOfWeek == 1))
					{
					return JewishHoliday.SEVENTEEN_OF_TAMMUZ;
					}
					break;
			case HebrewMonth.AV:
					// if Tisha B'av falls on Shabbos, push off until Sunday
					if ((dayOfWeek == 1 && dayOfMonth == 10) || (dayOfWeek != 7 && dayOfMonth == 9))
					{
					return JewishHoliday.TISHA_BEAV;
					}
					else if (dayOfMonth == 15)
					{
					return JewishHoliday.TU_BEAV;
					}
					break;
			case HebrewMonth.ELUL:
					if (dayOfMonth == 29)
					{
					return JewishHoliday.EREV_ROSH_HASHANA;
					}
					break;
			case HebrewMonth.TISHREI:
					if (dayOfMonth == 1 || dayOfMonth == 2)
					{
					return JewishHoliday.ROSH_HASHANA;
					}
					else if ((dayOfMonth == 3 && dayOfWeek != 7) || (dayOfMonth == 4 && dayOfWeek == 1))
					{
						// push off Tzom Gedalia if it falls on Shabbos
					return JewishHoliday.FAST_OF_GEDALYAH;
					}
					else if (dayOfMonth == 9)
					{
					return JewishHoliday.EREV_YOM_KIPPUR;
					}
					else if (dayOfMonth == 10)
					{
					return JewishHoliday.YOM_KIPPUR;
					}
					else if (dayOfMonth == 14)
					{
					return JewishHoliday.EREV_SUCCOS;
					}
					if (dayOfMonth == 15 || (dayOfMonth == 16 && !inIsrael))
					{
					return JewishHoliday.SUCCOS;
					}
					if (dayOfMonth >= 17 && dayOfMonth <= 20 || (dayOfMonth == 16 && inIsrael))
					{
					return JewishHoliday.CHOL_HAMOED_SUCCOS;
					}
					if (dayOfMonth == 21)
					{
					return JewishHoliday.HOSHANA_RABBA;
					}
					if (dayOfMonth == 22)
					{
					return JewishHoliday.SHEMINI_ATZERES;
					}
					if (dayOfMonth == 23 && !inIsrael)
					{
					return JewishHoliday.SIMCHAS_TORAH;
					}
					break;
			case HebrewMonth.KISLEV: // no yomtov in CHESHVAN
					// if (getdayOfMonth() == 24) {
					// return EREV_CHANUKAH;
					// } else
					if (dayOfMonth >= 25)
					{
					return JewishHoliday.CHANUKAH;
					}
					break;
			case HebrewMonth.TEVES:
				if (dayOfMonth == 1 || dayOfMonth == 2 || (dayOfMonth == 3 && IsKislevShort(dt)))
					{
					return JewishHoliday.CHANUKAH;
					}
					else if (dayOfMonth == 10)
					{
					return JewishHoliday.TENTH_OF_TEVES;
					}
					break;
			case HebrewMonth.SHEVAT:
					if (dayOfMonth == 15)
					{
					return JewishHoliday.TU_BESHVAT;
					}
					break;
			case HebrewMonth.ADAR:
				if (!IsLeapYearFromDateTime(dt))
					{
						// if 13th Adar falls on Friday or Shabbos, push back to Thursday
						if (((dayOfMonth == 11 || dayOfMonth == 12) && dayOfWeek == 5) || (dayOfMonth == 13 && !(dayOfWeek == 6 || dayOfWeek == 7)))
						{
						return JewishHoliday.FAST_OF_ESTHER;
						}
						if (dayOfMonth == 14)
						{
						return JewishHoliday.PURIM;
						}
						else if (dayOfMonth == 15)
						{
						return JewishHoliday.SHUSHAN_PURIM;
						}
					} // else if a leap year
					else
					{
						if (dayOfMonth == 14)
						{
						return JewishHoliday.PURIM_KATAN;
						}
					}
					break;
			case HebrewMonth.ADAR_II:
					// if 13th Adar falls on Friday or Shabbos, push back to Thursday
					if (((dayOfMonth == 11 || dayOfMonth == 12) && dayOfWeek == 5) || (dayOfMonth == 13 && !(dayOfWeek == 6 || dayOfWeek == 7)))
					{
					return JewishHoliday.FAST_OF_ESTHER;
					}
					if (dayOfMonth == 14)
					{
					return JewishHoliday.PURIM;
					}
					else if (dayOfMonth == 15)
					{
					return JewishHoliday.SHUSHAN_PURIM;
					}
					break;
				}
				// if we get to this stage, then there are no holidays for the given date return -1
			return JewishHoliday.NONE;

		}

		/// <summary>
		/// Returns true if the current day is Yom Tov. The method returns false for Chanukah, Erev Yom tov and fast days.
		/// </summary>
		/// <returns> true if the current day is a Yom Tov </returns>
		/// <seealso cref= #isErevYomTov() </seealso>
		/// <seealso cref= #isTaanis() </seealso>
		public bool IsYomTov(DateTime dt, bool inIsrael)
		{
			
			JewishHoliday jewishHoliday = GetJewishHoliday(dt, inIsrael, false);
			if (IsErevYomTov(dt, inIsrael) || jewishHoliday == JewishHoliday.CHANUKAH || (IsTaanis(dt, inIsrael) && jewishHoliday != JewishHoliday.YOM_KIPPUR))
				{
					return false;
				}
			return jewishHoliday != JewishHoliday.NONE;

		}

		/// <summary>
		/// Determines whether this instance is kislev short the specified dt.
		/// </summary>
		/// <returns><c>true</c> if this instance is kislev short the specified dt; otherwise, <c>false</c>.</returns>
		/// <param name="dt">Dt.</param>
		public bool IsKislevShort(DateTime dt) {
			return GetDaysInMonth (GetYear (dt), GetMonth (dt)) == 30 ? false : true;
		}

		/// <summary>
		/// Determines whether this instance is erev yom tov the specified dt inIsrael.
		/// </summary>
		/// <returns><c>true</c> if this instance is erev yom tov the specified dt inIsrael; otherwise, <c>false</c>.</returns>
		/// <param name="dt">Dt.</param>
		/// <param name="inIsrael">If set to <c>true</c> in israel.</param>
		public bool IsErevYomTov(DateTime dt, bool inIsrael)
		{
			JewishHoliday jewishHoliday = GetJewishHoliday(dt, inIsrael, false);
			return jewishHoliday == JewishHoliday.EREV_PESACH || jewishHoliday == JewishHoliday.EREV_SHAVUOS || jewishHoliday == JewishHoliday.EREV_ROSH_HASHANA || jewishHoliday == JewishHoliday.EREV_YOM_KIPPUR || jewishHoliday == JewishHoliday.EREV_SUCCOS;

		}


		public bool IsTaanis(DateTime dt, bool inIsrael)
		{
			JewishHoliday jewishHoliday = GetJewishHoliday(dt, inIsrael, false);
			return jewishHoliday == JewishHoliday.SEVENTEEN_OF_TAMMUZ || jewishHoliday == JewishHoliday.TISHA_BEAV || jewishHoliday == JewishHoliday.YOM_KIPPUR || jewishHoliday == JewishHoliday.FAST_OF_GEDALYAH || jewishHoliday == JewishHoliday.TENTH_OF_TEVES || jewishHoliday == JewishHoliday.FAST_OF_ESTHER;
		
		}
	}
}

