using System;
using System.Globalization;

namespace Zmanim.JewishCalendar
{
	/// <summary>
	/// Jewish calendar. Extends HebrewCalendar to provide things like Jewish info (holidays etc.) and jewish months
	/// </summary>
	public class JewishCalendar: System.Globalization.HebrewCalendar
	{
		
		/// <summary>
		/// Normalizes the months as 1-13 (1-12, with 13 being adar bet)
		/// This is different than the native HebrewCalendar month index 
		/// which changes the index of several months depending on whether or not it is a leap year
		/// see: https://msdn.microsoft.com/en-us/library/system.globalization.hebrewcalendar(v=vs.110).aspx
		/// </summary>
		public enum JewishMonth
		{
			/// <summary>
			/// To represent "no month"
			/// </summary>
			NONE = -1,
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

		public enum JewishYearType {
			/// <summary>
			/// A short year where both <seealso cref="#CHESHVAN"/> and <seealso cref="#KISLEV"/> are 29 days.
			/// </summary>
			/// <seealso cref= #getCheshvanKislevKviah() </seealso>
			/// <seealso cref= HebrewDateFormatter#getFormattedKviah(int) </seealso>
			CHASERIM = 0,

			/// <summary>
			/// An ordered year where <seealso cref="#CHESHVAN"/> is 29 days and <seealso cref="#KISLEV"/> is 30 days.
			/// </summary>
			/// <seealso cref= #getCheshvanKislevKviah() </seealso>
			/// <seealso cref= HebrewDateFormatter#getFormattedKviah(int) </seealso>
			KESIDRAN,

			/// <summary>
			/// A long year where both <seealso cref="#CHESHVAN"/> and <seealso cref="#KISLEV"/> are 30 days.
			/// </summary>
			/// <seealso cref= #getCheshvanKislevKviah() </seealso>
			/// <seealso cref= HebrewDateFormatter#getFormattedKviah(int) </seealso>
			SHELAIMIM
		}

		/// <summary>
		/// Natives the month to jewish month.
		/// see https://msdn.microsoft.com/en-us/library/system.globalization.hebrewcalendar(v=vs.110).aspx
		/// </summary>
		/// <returns>The month to jewish month.</returns>
		/// <param name="nativeMonth">Native month.</param>
		/// <param name="leapYear">If set to <c>true</c> leap year.</param>
		public JewishMonth NativeMonthToJewishMonth(int nativeMonth, bool leapYear) {

			switch (nativeMonth) {
			case 1:
				return JewishMonth.TISHREI;
			case 2:
				return JewishMonth.CHESHVAN;
			case 3:
				return JewishMonth.KISLEV;
			case 4:
				return JewishMonth.TEVES;
			case 5:
				return JewishMonth.SHEVAT;
			}

			if (leapYear) {
				switch (nativeMonth) {
				case 6:
					return JewishMonth.ADAR;
				case 7:
					return JewishMonth.ADAR_II;
				case 8:
					return JewishMonth.NISSAN;
				case 9:
					return JewishMonth.IYAR;
				case 10:
					return JewishMonth.SIVAN;
				case 11:
					return JewishMonth.TAMMUZ;
				case 12:
					return JewishMonth.AV;
				case 13:
					return JewishMonth.ELUL;
				}
			} else {
				switch (nativeMonth) {
				case 6:
					return JewishMonth.ADAR;
				case 7:
					return JewishMonth.NISSAN;
				case 8:
					return JewishMonth.IYAR;
				case 9:
					return JewishMonth.SIVAN;
				case 10:
					return JewishMonth.TAMMUZ;
				case 11:
					return JewishMonth.AV;
				case 12:
					return JewishMonth.ELUL;
				}
			}

			return JewishMonth.NONE;
		}


		/// <summary>
		/// Jewishs the month to native month.
		/// see https://msdn.microsoft.com/en-us/library/system.globalization.hebrewcalendar(v=vs.110).aspx
		/// </summary>
		/// <returns>The month to native month.</returns>
		/// <param name="jewishMonth">Jewish month.</param>
		/// <param name="leapYear">If set to <c>true</c> leap year.</param>
		public int JewishMonthToNativeMonth(JewishMonth jewishMonth, bool leapYear) {

			switch (jewishMonth) {
			case JewishMonth.TISHREI:
				return 1;
			case JewishMonth.CHESHVAN:
				return 2;
			case JewishMonth.KISLEV:
				return 3;
			case JewishMonth.TEVES:
				return 4;
			case JewishMonth.SHEVAT:
				return 5;
			}

			if (leapYear) {
				switch (jewishMonth) {
				case JewishMonth.ADAR:
					return 6;
				case JewishMonth.ADAR_II:
					return 7;
				case JewishMonth.NISSAN:
					return 8;
				case JewishMonth.IYAR:
					return 9;
				case JewishMonth.SIVAN:
					return 10;
				case JewishMonth.TAMMUZ:
					return 11;
				case JewishMonth.AV:
					return 12;
				case JewishMonth.ELUL:
					return 13;
				}
			} else {
				switch (jewishMonth) {
				case JewishMonth.ADAR:
					return 6;
				case JewishMonth.NISSAN:
					return 7;
				case JewishMonth.IYAR:
					return 8;
				case JewishMonth.SIVAN:
					return 9;
				case JewishMonth.TAMMUZ:
					return 10;
				case JewishMonth.AV:
					return 11;
				case JewishMonth.ELUL:
					return 12;
				}
			}

			return -1;
		}

		public int GetJewishDayOfWeekSundayIsOne(DateTime dt) {
			DayOfWeek nativeDayOfWeek = GetDayOfWeek (dt);

			switch (nativeDayOfWeek) {
			case DayOfWeek.Sunday:
				return 1;
			case DayOfWeek.Monday:
				return 2;
			case DayOfWeek.Tuesday:
				return 3;
			case DayOfWeek.Wednesday:
				return 4;
			case DayOfWeek.Thursday:
				return 5;
			case DayOfWeek.Friday:
				return 6;
			case DayOfWeek.Saturday:
				return 7;
			}

			return -1;

		}

		/// <summary>
		/// Determines whether this instance is leap year from date time the specified dt.
		/// </summary>
		/// <returns><c>true</c> if this instance is leap year from date time the specified dt; otherwise, <c>false</c>.</returns>
		/// <param name="dt">Dt.</param>
		public bool IsLeapYearFromDateTime(DateTime dt) {
			return IsLeapYear (GetYear (dt));
		}

		/// <summary>
		/// Gets the jewish month.
		/// </summary>
		/// <returns>The jewish month.</returns>
		/// <param name="dt">Dt.</param>
		public JewishMonth GetJewishMonth(DateTime dt) {
			return NativeMonthToJewishMonth (GetMonth (dt), IsLeapYearFromDateTime(dt));
		}

		/// <summary>
		/// Gets the jewish date time.
		/// </summary>
		/// <returns>The jewish date time.</returns>
		/// <param name="year">Year.</param>
		/// <param name="month">Month.</param>
		/// <param name="day">Day.</param>
		public DateTime GetJewishDateTime(int year, JewishMonth month, int day) {
			return new DateTime(year, JewishMonthToNativeMonth(month, IsLeapYear (year)), day, this);

		}

		/// <summary>
		/// Months the is 29 days / short
		/// </summary>
		/// <returns><c>true</c>, if is29 days was monthed, <c>false</c> otherwise.</returns>
		/// <param name="dt">Dt.</param>
		/// <param name="month">Month.</param>
		public bool MonthIs29Days(DateTime dt, JewishMonth month) {
			return GetJewishDaysInMonth(dt, month) == 29 ? true : false;
		}


		/// <summary>
		/// Gets the jewish days in month.
		/// </summary>
		/// <returns>The jewish days in month.</returns>
		/// <param name="dt">Dt.</param>
		/// <param name="month">Month.</param>
		public int GetJewishDaysInMonth(DateTime dt, JewishMonth month) {
			return GetJewishDaysInMonth (GetYear (dt), month);
		}


		/// <summary>
		/// Gets the jewish days in month.
		/// </summary>
		/// <returns>The jewish days in month.</returns>
		/// <param name="year">Year.</param>
		/// <param name="month">Month.</param>
		public int GetJewishDaysInMonth(int year, JewishMonth month) {
			int nativeMonth = JewishMonthToNativeMonth (month, IsLeapYear(year));
			return GetDaysInMonth (year, nativeMonth);
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

		public JewishHoliday GetJewishHolidayAfterMashiach(DateTime dt, bool inIsrael)
		{
			if (!inIsrael) {
				throw new Exception ("You are supposed to be in Israel by now");
			} else {
				return JewishHoliday.PURIM;
			}
		}

		public JewishHoliday GetJewishHoliday(DateTime dt, bool inIsrael, bool UseModernHolidays)
		{
			JewishMonth hebrewMonth = GetJewishMonth (dt);
			int dayOfMonth = GetDayOfMonth (dt);
			int dayOfWeek = GetJewishDayOfWeekSundayIsOne (dt);

			switch (hebrewMonth) {
			case JewishMonth.NISSAN:
			if (dayOfMonth == 14) {
				return JewishHoliday.EREV_PESACH;
			} else if (dayOfMonth == 15 || dayOfMonth == 21
				|| (!inIsrael && (dayOfMonth == 16 || dayOfMonth == 22))) {
					return JewishHoliday.PESACH;
			} else if (dayOfMonth >= 17 && dayOfMonth <= 20
				|| (dayOfMonth == 16 && inIsrael)) {
					return JewishHoliday.CHOL_HAMOED_PESACH;
			}
				if (UseModernHolidays
				&& ((dayOfMonth == 26 && dayOfWeek == 5)
					|| (dayOfMonth == 28 && dayOfWeek == 2)
					|| (dayOfMonth == 27 && dayOfWeek != 1 && dayOfWeek != 6))) {
					return JewishHoliday.YOM_HASHOAH;
			}
			break;
			case JewishMonth.IYAR:
				if (UseModernHolidays
				&& ((dayOfMonth == 4 && dayOfWeek == 3)
					|| ((dayOfMonth == 3 || dayOfMonth == 2) && dayOfWeek == 4) || (dayOfMonth == 5 && dayOfWeek == 2))) {
					return JewishHoliday.YOM_HAZIKARON;
			}
			// if 5 Iyar falls on Wed Yom Haatzmaut is that day. If it fal1s on Friday or Shabbos it is moved back to
			// Thursday. If it falls on Monday it is moved to Tuesday
				if (UseModernHolidays
				&& ((dayOfMonth == 5 && dayOfWeek == 4)
					|| ((dayOfMonth == 4 || dayOfMonth == 3) && dayOfWeek == 5) || (dayOfMonth == 6 && dayOfWeek == 3))) {
					return JewishHoliday.YOM_HAATZMAUT;
			}
			if (dayOfMonth == 14) {
					return JewishHoliday.PESACH_SHENI;
			}
				if (UseModernHolidays && dayOfMonth == 28) {
					return JewishHoliday.YOM_YERUSHALAYIM;
			}
			break;
			case JewishMonth.SIVAN:
			if (dayOfMonth == 5) {
					return JewishHoliday.EREV_SHAVUOS;
			} else if (dayOfMonth == 6 || (dayOfMonth == 7 && !inIsrael)) {
					return JewishHoliday.SHAVUOS;
			}
			break;
			case JewishMonth.TAMMUZ:
			// push off the fast day if it falls on Shabbos
			if ((dayOfMonth == 17 && dayOfWeek != 7)
				|| (dayOfMonth == 18 && dayOfWeek == 1)) {
					return JewishHoliday.SEVENTEEN_OF_TAMMUZ;
			}
			break;
			case JewishMonth.AV:
			// if Tisha B'av falls on Shabbos, push off until Sunday
			if ((dayOfWeek == 1 && dayOfMonth == 10)
				|| (dayOfWeek != 7 && dayOfMonth == 9)) {
					return JewishHoliday.TISHA_BEAV;
			} else if (dayOfMonth == 15) {
					return JewishHoliday.TU_BEAV;
			}
			break;
			case JewishMonth.ELUL:
			if (dayOfMonth == 29) {
					return JewishHoliday.EREV_ROSH_HASHANA;
			}
			break;
			case JewishMonth.TISHREI:
			if (dayOfMonth == 1 || dayOfMonth == 2) {
					return JewishHoliday.ROSH_HASHANA;
			} else if ((dayOfMonth == 3 && dayOfWeek != 7)
				|| (dayOfMonth == 4 && dayOfWeek == 1)) {
				// push off Tzom Gedalia if it falls on Shabbos
					return JewishHoliday.FAST_OF_GEDALYAH;
			} else if (dayOfMonth == 9) {
					return JewishHoliday.EREV_YOM_KIPPUR;
			} else if (dayOfMonth == 10) {
					return JewishHoliday.YOM_KIPPUR;
			} else if (dayOfMonth == 14) {
					return JewishHoliday.EREV_SUCCOS;
			}
			if (dayOfMonth == 15 || (dayOfMonth == 16 && !inIsrael)) {
					return JewishHoliday.SUCCOS;
			}
			if (dayOfMonth >= 17 && dayOfMonth <= 20 || (dayOfMonth == 16 && inIsrael)) {
					return JewishHoliday.CHOL_HAMOED_SUCCOS;
			}
			if (dayOfMonth == 21) {
					return JewishHoliday.HOSHANA_RABBA;
			}
			if (dayOfMonth == 22) {
					return JewishHoliday.SHEMINI_ATZERES;
			}
			if (dayOfMonth == 23 && !inIsrael) {
					return JewishHoliday.SIMCHAS_TORAH;
			}
			break;
			case JewishMonth.KISLEV: // no yomtov in CHESHVAN
			// if (dayOfMonth == 24) {
			// return EREV_CHANUKAH;
			// } else
			if (dayOfMonth >= 25) {
					return JewishHoliday.CHANUKAH;
			}
			break;
			case JewishMonth.TEVES:
			if (dayOfMonth == 1 || dayOfMonth == 2
					|| (dayOfMonth == 3 && MonthIs29Days(dt, JewishMonth.KISLEV))) {
					return JewishHoliday.CHANUKAH;
			} else if (dayOfMonth == 10) {
					return JewishHoliday.TENTH_OF_TEVES;
			}
			break;
			case JewishMonth.SHEVAT:
			if (dayOfMonth == 15) {
					return JewishHoliday.TU_BESHVAT;
			}
			break;
			case JewishMonth.ADAR:
				if (!IsLeapYearFromDateTime(dt)) {
				// if 13th Adar falls on Friday or Shabbos, push back to Thursday
				if (((dayOfMonth == 11 || dayOfMonth == 12) && dayOfWeek == 5)
					|| (dayOfMonth == 13 && !(dayOfWeek == 6 || dayOfWeek == 7))) {
						return JewishHoliday.FAST_OF_ESTHER;
				}
				if (dayOfMonth == 14) {
						return JewishHoliday.PURIM;
					} else if (dayOfMonth == 15 && inIsrael) {
						return JewishHoliday.SHUSHAN_PURIM;
				}
			} else { // else if a leap year
				if (dayOfMonth == 14) {
						return JewishHoliday.PURIM_KATAN;
				}
			}
			break;
			case JewishMonth.ADAR_II:
			// if 13th Adar falls on Friday or Shabbos, push back to Thursday
			if (((dayOfMonth == 11 || dayOfMonth == 12) && dayOfWeek == 5)
				|| (dayOfMonth == 13 && !(dayOfWeek == 6 || dayOfWeek == 7))) {
					return JewishHoliday.FAST_OF_ESTHER;
			}
			if (dayOfMonth == 14) {
					return JewishHoliday.PURIM;
				} else if (dayOfMonth == 15 && inIsrael) {
					return JewishHoliday.SHUSHAN_PURIM;
			}
			break;
		}
		// if we get to this stage, then there are no holidays for the given date return -1
			return JewishHoliday.NONE;
	}

	/**
	 * Returns true if the current day is Yom Tov. The method returns false for Chanukah, Erev Yom Tov (with the
	 * exception of Hoshana Rabba and Erev the second days of Pesach) and fast days.
	 * 
	 * @return true if the current day is a Yom Tov
	 * @see #isErevYomTov()
	 * @see #isTaanis()
	 */
		public bool IsYomTov(DateTime dt, bool inIsrael) {
			

			JewishHoliday holidayIndex = GetJewishHoliday (dt, inIsrael);

			if ((IsErevYomTov(dt, inIsrael) && (holidayIndex != JewishHoliday.HOSHANA_RABBA && (holidayIndex == JewishHoliday.CHOL_HAMOED_PESACH && GetDayOfMonth (dt) != 20)))
				|| holidayIndex == JewishHoliday.CHANUKAH || (IsTaanis(dt, inIsrael) && holidayIndex != JewishHoliday.YOM_KIPPUR)) {
			return false;
		}
			return holidayIndex != JewishHoliday.NONE;
	}

	/**
	 * Returns true if the Yom Tov day has a melacha (work)  prohibition. This method will return false for a non Yom Tov day, even if it is Shabbos.
	 * 
	 * @return if the Yom Tov day has a melacha (work)  prohibition.
	 */
		public bool IsYomTovAssurBemelacha(DateTime dt, bool inIsrael){
			JewishHoliday holidayIndex = GetJewishHoliday (dt, inIsrael);
			return holidayIndex == JewishHoliday.PESACH || holidayIndex == JewishHoliday.SHAVUOS || holidayIndex == JewishHoliday.SUCCOS || holidayIndex == JewishHoliday.SHEMINI_ATZERES || 
				holidayIndex == JewishHoliday.SIMCHAS_TORAH || holidayIndex == JewishHoliday.ROSH_HASHANA  || holidayIndex == JewishHoliday.YOM_KIPPUR;
	}

	/**
	 * Returns true if the current day is Chol Hamoed of Pesach or Succos.
	 * 
	 * @return true if the current day is Chol Hamoed of Pesach or Succos
	 * @see #isYomTov()
	 * @see #CHOL_HAMOED_PESACH
	 * @see #CHOL_HAMOED_SUCCOS
	 */
		public bool IsCholHamoed(DateTime dt, bool inIsrael) {
			JewishHoliday holidayIndex = GetJewishHoliday (dt, inIsrael);
			return holidayIndex == JewishHoliday.CHOL_HAMOED_PESACH || holidayIndex == JewishHoliday.CHOL_HAMOED_SUCCOS;
	}

	/**
	 * Returns true if the current day is erev Yom Tov. The method returns true for Erev - Pesach (first and last days),
	 * Shavuos, Rosh Hashana, Yom Kippur and Succos and Hoshana Rabba.
	 * 
	 * @return true if the current day is Erev - Pesach, Shavuos, Rosh Hashana, Yom Kippur and Succos
	 * @see #isYomTov()
	 */
		public bool IsErevYomTov(DateTime dt, bool inIsrael) {
			JewishHoliday holidayIndex = GetJewishHoliday (dt, inIsrael);
			return holidayIndex == JewishHoliday.EREV_PESACH || holidayIndex == JewishHoliday.EREV_SHAVUOS || holidayIndex == JewishHoliday.EREV_ROSH_HASHANA
				|| holidayIndex == JewishHoliday.EREV_YOM_KIPPUR || holidayIndex == JewishHoliday.EREV_SUCCOS || holidayIndex == JewishHoliday.HOSHANA_RABBA
				|| (holidayIndex == JewishHoliday.CHOL_HAMOED_PESACH && GetDayOfMonth (dt) == 20);
	}

	/**
	 * Returns true if the current day is Erev Rosh Chodesh. Returns false for Erev Rosh Hashana
	 * 
	 * @return true if the current day is Erev Rosh Chodesh. Returns false for Erev Rosh Hashana
	 * @see #isRoshChodesh()
	 */
		public bool IsErevRoshChodesh(DateTime dt) {
		// Erev Rosh Hashana is not Erev Rosh Chodesh.
			return (GetDayOfMonth (dt) == 29 && GetJewishMonth(dt) != JewishMonth.ELUL);
	}

	/**
	 * Return true if the day is a Taanis (fast day). Return true for 17 of Tammuz, Tisha B'Av, Yom Kippur, Fast of
	 * Gedalyah, 10 of Teves and the Fast of Esther
	 * 
	 * @return true if today is a fast day
	 */
		public bool IsTaanis(DateTime dt, bool inIsrael) {
			JewishHoliday holidayIndex = GetJewishHoliday (dt, inIsrael);
			return holidayIndex == JewishHoliday.SEVENTEEN_OF_TAMMUZ || holidayIndex == JewishHoliday.TISHA_BEAV || holidayIndex == JewishHoliday.YOM_KIPPUR
				|| holidayIndex == JewishHoliday.FAST_OF_GEDALYAH || holidayIndex == JewishHoliday.TENTH_OF_TEVES || holidayIndex == JewishHoliday.FAST_OF_ESTHER;
	}

	/**
	 * Returns the day of Chanukah or -1 if it is not Chanukah.
	 * 
	 * @return the day of Chanukah or -1 if it is not Chanukah.
	 */
		public int GetDayOfChanukah(DateTime dt) {
			if (IsChanukah(dt)) {
				if (GetJewishMonth(dt) == JewishMonth.KISLEV) {
					return GetDayOfMonth (dt) - 24;
			} else { // teves
					return MonthIs29Days(dt, JewishMonth.KISLEV) ? GetDayOfMonth (dt) + 5 : GetDayOfMonth (dt) + 6;
			}
		} else {
			return -1;
		}
	}

		public bool IsChanukah(DateTime dt) {
			//israel settings don't matter, but would rather catch it with compiler elsewhere
			return GetJewishHoliday (dt, true) == JewishHoliday.CHANUKAH;
	}

	/**
	 * Returns if the day is Rosh Chodesh. Rosh Hashana will return false
	 * 
	 * @return true if it is Rosh Chodesh. Rosh Hashana will return false
	 */
		public bool IsRoshChodesh(DateTime dt) {
		// Rosh Hashana is not rosh chodesh. Elul never has 30 days
			return (GetDayOfMonth (dt) == 1 && GetJewishMonth(dt) != JewishMonth.TISHREI) || GetDayOfMonth (dt) == 30;
	}

	/**
	 * Returns the int value of the Omer day or -1 if the day is not in the omer
	 * 
	 * @return The Omer count as an int or -1 if it is not a day of the Omer.
	 */
		public int GetDayOfOmer(DateTime dt) {
		int omer = -1; // not a day of the Omer

		// if Nissan and second day of Pesach and on
			if (GetJewishMonth(dt) == JewishMonth.NISSAN && GetDayOfMonth (dt) >= 16) {
				omer = GetDayOfMonth (dt) - 15;
			// if Iyar
			} else if (GetJewishMonth(dt) == JewishMonth.IYAR) {
				omer = GetDayOfMonth (dt) + 15;
			// if Sivan and before Shavuos
			} else if (GetJewishMonth(dt) == JewishMonth.SIVAN && GetDayOfMonth (dt) < 6) {
				omer = GetDayOfMonth (dt) + 44;
		}
		return omer;
	}

	

	/**
	 * Returns the Daf Yomi (Bavli) for the date that the calendar is set to. See the
	 * {@link HebrewDateFormatter#formatDafYomiBavli(Daf)} for the ability to format the daf in Hebrew or transliterated
	 * masechta names.
	 * 
	 * @return the daf as a {@link Daf}
	 */
		public Daf getDafYomiBavli(DateTime dt) {
			return YomiCalculator.getDafYomiBavli(dt);
	}

		public JewishYearType GetJewishYearType(DateTime dt) {
			JewishYearType jType = JewishYearType.SHELAIMIM;

			if (MonthIs29Days (dt, JewishMonth.CHESHVAN)) {
				jType = JewishYearType.KESIDRAN;
				if (MonthIs29Days (dt, JewishMonth.KISLEV)) {
					jType = JewishYearType.CHASERIM;
				}
			}

			return jType;

		}

	
}
}