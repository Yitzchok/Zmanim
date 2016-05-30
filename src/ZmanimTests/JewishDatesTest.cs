using System;
using System.Globalization;
using NUnit.Framework;
using Zmanim;

namespace ZmanimTests
{
    [TestFixture]
    public class JewishDatesTest
    {
        JewishCalendar jewishCalendar;
        GregorianCalendar gregorianCalendar;

        [SetUp]
        public void Setup()
        {
			jewishCalendar = new JewishCalendar();
            gregorianCalendar = new GregorianCalendar();
        }

        [Test]
        public void Jewish_leap_year_test()
        {
			Assert.That(jewishCalendar.IsLeapYear(5769), Is.False);
			Assert.That(jewishCalendar.IsLeapYear(5770), Is.False);
			Assert.That(jewishCalendar.IsLeapYear(5771), Is.True);
        }

        [Test]
        public void Can_convert_Gregorian_date_to_Jewish_Date_Native()
        {
            var dateTime = new DateTime(2010, 4, 15, gregorianCalendar);
			Assert.That(jewishCalendar.GetYear(dateTime), Is.EqualTo(5770));
			Assert.That(jewishCalendar.GetMonth(dateTime), Is.EqualTo(8));
			Assert.That(jewishCalendar.GetDayOfMonth(dateTime), Is.EqualTo(1));
        }

		[Test]
		public void Can_convert_Month_Systems()
		{

			//leap year
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (1, true), Is.EqualTo(JewishCalendar.JewishMonth.TISHREI));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (2, true), Is.EqualTo(JewishCalendar.JewishMonth.CHESHVAN));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (3, true), Is.EqualTo(JewishCalendar.JewishMonth.KISLEV));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (4, true), Is.EqualTo(JewishCalendar.JewishMonth.TEVES));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (5, true), Is.EqualTo(JewishCalendar.JewishMonth.SHEVAT));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (6, true), Is.EqualTo(JewishCalendar.JewishMonth.ADAR));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (7, true), Is.EqualTo(JewishCalendar.JewishMonth.ADAR_II));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (8, true), Is.EqualTo(JewishCalendar.JewishMonth.NISSAN));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (9, true), Is.EqualTo(JewishCalendar.JewishMonth.IYAR));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (10, true), Is.EqualTo(JewishCalendar.JewishMonth.SIVAN));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (11, true), Is.EqualTo(JewishCalendar.JewishMonth.TAMMUZ));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (12, true), Is.EqualTo(JewishCalendar.JewishMonth.AV));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (13, true), Is.EqualTo(JewishCalendar.JewishMonth.ELUL));

			//regular year
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (1, false), Is.EqualTo(JewishCalendar.JewishMonth.TISHREI));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (2, false), Is.EqualTo(JewishCalendar.JewishMonth.CHESHVAN));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (3, false), Is.EqualTo(JewishCalendar.JewishMonth.KISLEV));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (4, false), Is.EqualTo(JewishCalendar.JewishMonth.TEVES));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (5, false), Is.EqualTo(JewishCalendar.JewishMonth.SHEVAT));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (6, false), Is.EqualTo(JewishCalendar.JewishMonth.ADAR));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (7, false), Is.EqualTo(JewishCalendar.JewishMonth.NISSAN));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (8, false), Is.EqualTo(JewishCalendar.JewishMonth.IYAR));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (9, false), Is.EqualTo(JewishCalendar.JewishMonth.SIVAN));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (10, false), Is.EqualTo(JewishCalendar.JewishMonth.TAMMUZ));
			Assert.That (jewishCalendar.NativeMonthToJewishMonth (11, false), Is.EqualTo(JewishCalendar.JewishMonth.AV));




			//leap year
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.NISSAN, true), Is.EqualTo(8));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.IYAR, true), Is.EqualTo(9));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.SIVAN, true), Is.EqualTo(10));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.TAMMUZ, true), Is.EqualTo(11));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.AV, true), Is.EqualTo(12));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.ELUL, true), Is.EqualTo(13));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.TISHREI, true), Is.EqualTo(1));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.CHESHVAN, true), Is.EqualTo(2));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.KISLEV, true), Is.EqualTo(3));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.TEVES, true), Is.EqualTo(4));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.SHEVAT, true), Is.EqualTo(5));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.ADAR, true), Is.EqualTo(6));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.ADAR_II, true), Is.EqualTo(7));


			//regular year
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.NISSAN, false), Is.EqualTo(7));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.IYAR, false), Is.EqualTo(8));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.SIVAN, false), Is.EqualTo(9));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.TAMMUZ, false), Is.EqualTo(10));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.AV, false), Is.EqualTo(11));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.ELUL, false), Is.EqualTo(12));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.TISHREI, false), Is.EqualTo(1));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.CHESHVAN, false), Is.EqualTo(2));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.KISLEV, false), Is.EqualTo(3));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.TEVES, false), Is.EqualTo(4));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.SHEVAT, false), Is.EqualTo(5));
			Assert.That (jewishCalendar.JewishMonthToNativeMonth(JewishCalendar.JewishMonth.ADAR, false), Is.EqualTo(6));

		}

		[Test]
		public void Can_convert_Gregorian_date_to_Jewish_Jewish_Month()
		{
			DateTime dateTime;


			//Leap year
			dateTime = new DateTime(2016, 4, 8, gregorianCalendar);
			Assert.That(jewishCalendar.GetJewishMonth(dateTime), Is.EqualTo(JewishCalendar.JewishMonth.ADAR_II));


			dateTime = new DateTime(2016, 4, 9, gregorianCalendar);
			Assert.That(jewishCalendar.GetJewishMonth(dateTime), Is.EqualTo(JewishCalendar.JewishMonth.NISSAN));

			dateTime = new DateTime(2015, 9, 14, gregorianCalendar);
			Assert.That(jewishCalendar.GetJewishMonth(dateTime), Is.EqualTo(JewishCalendar.JewishMonth.TISHREI));

			dateTime = new DateTime(2016, 6, 12, gregorianCalendar);
			Assert.That(jewishCalendar.GetJewishMonth(dateTime), Is.EqualTo(JewishCalendar.JewishMonth.SIVAN));


			//Regular year
			dateTime = new DateTime(2015, 3, 20, gregorianCalendar);
			Assert.That(jewishCalendar.GetJewishMonth(dateTime), Is.EqualTo(JewishCalendar.JewishMonth.ADAR));

			dateTime = new DateTime(2015, 3, 21, gregorianCalendar);
			Assert.That(jewishCalendar.GetJewishMonth(dateTime), Is.EqualTo(JewishCalendar.JewishMonth.NISSAN));

		}

		[Test]
		public void Can_convert_JewishDate_To_Gregorian()
		{

			DateTime hebrewDateTime, gregorianDateTime;

			//Leap year
			hebrewDateTime = jewishCalendar.GetJewishDateTime(5776, JewishCalendar.JewishMonth.ADAR_II, 29);
			gregorianDateTime = new DateTime(2016, 4, 8, gregorianCalendar);
			Assert.That(hebrewDateTime, Is.EqualTo(gregorianDateTime));
				
			hebrewDateTime = jewishCalendar.GetJewishDateTime(5776, JewishCalendar.JewishMonth.NISSAN, 1);
			gregorianDateTime = new DateTime(2016, 4, 9, gregorianCalendar);
			Assert.That(hebrewDateTime, Is.EqualTo(gregorianDateTime));

			hebrewDateTime = jewishCalendar.GetJewishDateTime(5776, JewishCalendar.JewishMonth.TISHREI, 1);
			gregorianDateTime = new DateTime(2015, 9, 14, gregorianCalendar);
			Assert.That(hebrewDateTime, Is.EqualTo(gregorianDateTime));



			//Regular year
			hebrewDateTime = jewishCalendar.GetJewishDateTime(5775, JewishCalendar.JewishMonth.ADAR, 29);
			gregorianDateTime = new DateTime(2015, 3, 20, gregorianCalendar);
			Assert.That(hebrewDateTime, Is.EqualTo(gregorianDateTime));
						
			hebrewDateTime = jewishCalendar.GetJewishDateTime(5775, JewishCalendar.JewishMonth.NISSAN, 1);
			gregorianDateTime = new DateTime(2015, 3, 21, gregorianCalendar);
			Assert.That(hebrewDateTime, Is.EqualTo(gregorianDateTime));
		}

		[Test]
		public void Can_Get_Holidays_From_Gregorian()
		{
			DateTime dateTime;

			//In Israel
			dateTime = new DateTime(2015, 9, 14, gregorianCalendar);
			Assert.That(jewishCalendar.GetJewishHoliday(dateTime, true), Is.EqualTo(JewishCalendar.JewishHoliday.ROSH_HASHANA));

			dateTime = new DateTime(2016, 6, 12, gregorianCalendar);
			Assert.That(jewishCalendar.GetJewishHoliday(dateTime, true), Is.EqualTo(JewishCalendar.JewishHoliday.SHAVUOS));

			//Outside Israel
			dateTime = new DateTime(2016, 6, 13, gregorianCalendar);
			Assert.That(jewishCalendar.GetJewishHoliday(dateTime, false), Is.EqualTo(JewishCalendar.JewishHoliday.SHAVUOS));

		}

		[Test]
		public void Can_Get_Short_Month_From_Gregorian()
		{
			DateTime dateTime;


			dateTime = new DateTime(2016, 12, 1, gregorianCalendar);
			Assert.That(jewishCalendar.MonthIs29Days(dateTime, JewishCalendar.JewishMonth.KISLEV), Is.EqualTo(true));

			dateTime = new DateTime(2015, 12, 1, gregorianCalendar);
			Assert.That(jewishCalendar.MonthIs29Days(dateTime, JewishCalendar.JewishMonth.KISLEV), Is.EqualTo(false));
		}

		[Test]
		public void Can_Get_Formatter()
		{
			DateTime dateTime;

			Zmanim.HebrewDateFormatter formatter = new Zmanim.HebrewDateFormatter ();

			Assert.That (formatter.getFormattedKviah (5729), Is.EqualTo ("בשה"));
			Assert.That (formatter.getFormattedKviah (5771), Is.EqualTo ("השג"));


		}
	
    }
}