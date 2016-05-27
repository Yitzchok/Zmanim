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
		public void Can_convert_Gregorian_date_to_Jewish_Normalized_Month()
		{
			DateTime dateTime;

			//Leap year
			dateTime = new DateTime(2016, 4, 8, gregorianCalendar);
			Assert.That(jewishCalendar.GetNormalizedMonth(dateTime), Is.EqualTo(JewishCalendar.HebrewMonth.ADAR_II));

			dateTime = new DateTime(2016, 4, 9, gregorianCalendar);
			Assert.That(jewishCalendar.GetNormalizedMonth(dateTime), Is.EqualTo(JewishCalendar.HebrewMonth.NISSAN));

			dateTime = new DateTime(2015, 9, 14, gregorianCalendar);
			Assert.That(jewishCalendar.GetNormalizedMonth(dateTime), Is.EqualTo(JewishCalendar.HebrewMonth.TISHREI));

			//Why is this failing?!
			dateTime = new DateTime(2016, 6, 12, gregorianCalendar);
			Console.Write (jewishCalendar.GetDayOfMonth (dateTime) + " of " + jewishCalendar.GetNormalizedMonth (dateTime));
			Assert.That(jewishCalendar.GetNormalizedMonth(dateTime), Is.EqualTo(JewishCalendar.HebrewMonth.SIVAN));


			//Regular year
			dateTime = new DateTime(2015, 3, 20, gregorianCalendar);
			Assert.That(jewishCalendar.GetNormalizedMonth(dateTime), Is.EqualTo(JewishCalendar.HebrewMonth.ADAR));

			dateTime = new DateTime(2015, 3, 21, gregorianCalendar);
			Assert.That(jewishCalendar.GetNormalizedMonth(dateTime), Is.EqualTo(JewishCalendar.HebrewMonth.NISSAN));
		}

		[Test]
		public void Can_convert_HebrewDate_To_Gregorian()
		{

			DateTime hebrewDateTime, gregorianDateTime;

			//Leap year
			hebrewDateTime = jewishCalendar.GetHebrewDateTime(5776, JewishCalendar.HebrewMonth.ADAR_II, 29);
			gregorianDateTime = new DateTime(2016, 4, 8, gregorianCalendar);
			Assert.That(hebrewDateTime, Is.EqualTo(gregorianDateTime));
				
			hebrewDateTime = jewishCalendar.GetHebrewDateTime(5776, JewishCalendar.HebrewMonth.NISSAN, 1);
			gregorianDateTime = new DateTime(2016, 4, 9, gregorianCalendar);
			Assert.That(hebrewDateTime, Is.EqualTo(gregorianDateTime));

			hebrewDateTime = jewishCalendar.GetHebrewDateTime(5776, JewishCalendar.HebrewMonth.TISHREI, 1);
			gregorianDateTime = new DateTime(2015, 9, 14, gregorianCalendar);
			Assert.That(hebrewDateTime, Is.EqualTo(gregorianDateTime));



			//Regular year
			hebrewDateTime = jewishCalendar.GetHebrewDateTime(5775, JewishCalendar.HebrewMonth.ADAR, 29);
			gregorianDateTime = new DateTime(2015, 3, 20, gregorianCalendar);
			Assert.That(hebrewDateTime, Is.EqualTo(gregorianDateTime));
						
			hebrewDateTime = jewishCalendar.GetHebrewDateTime(5775, JewishCalendar.HebrewMonth.NISSAN, 1);
			gregorianDateTime = new DateTime(2015, 3, 21, gregorianCalendar);
			Assert.That(hebrewDateTime, Is.EqualTo(gregorianDateTime));
		}

		[Test]
		public void Can_Get_Holidays_From_Gregorian()
		{
			DateTime dateTime;

			//Rosh hashana
			dateTime = new DateTime(2015, 9, 14, gregorianCalendar);
			Assert.That(jewishCalendar.GetJewishHoliday(dateTime, true), Is.EqualTo(JewishCalendar.JewishHoliday.ROSH_HASHANA));

			//Why is this failing?!
			dateTime = new DateTime(2016, 6, 12, gregorianCalendar);
			Console.Write (jewishCalendar.GetDayOfMonth (dateTime) + " of " + jewishCalendar.GetNormalizedMonth (dateTime));
			Assert.That(jewishCalendar.GetJewishHoliday(dateTime, true), Is.EqualTo(JewishCalendar.JewishHoliday.SHAVUOS));

		}


		[Test]
		public void Can_Get_Short_Kislev_From_Gregorian()
		{
			DateTime dateTime;


			dateTime = new DateTime(2016, 12, 1, gregorianCalendar);
			Assert.That(jewishCalendar.IsKislevShort(dateTime), Is.EqualTo(true));

			dateTime = new DateTime(2015, 12, 1, gregorianCalendar);
			Assert.That(jewishCalendar.IsKislevShort(dateTime), Is.EqualTo(false));
		}
    }
}