using System;
using System.Globalization;
using NUnit.Framework;

namespace ZmanimTests
{
    [TestFixture]
    public class JewishDatesTest
    {
        HebrewCalendar hebrewCalendar;
        GregorianCalendar gregorianCalendar;

        [SetUp]
        public void Setup()
        {
            hebrewCalendar = new HebrewCalendar();
            gregorianCalendar = new GregorianCalendar();
        }

        [Test]
        public void Jewish_leap_year_test()
        {
            Assert.That(hebrewCalendar.IsLeapYear(5769), Is.False);
            Assert.That(hebrewCalendar.IsLeapYear(5770), Is.False);
            Assert.That(hebrewCalendar.IsLeapYear(5771), Is.True);
        }

        [Test]
        public void Can_convert_Gregorian_date_to_Jewish_Date()
        {
            var dateTime = new DateTime(2010, 4, 15, gregorianCalendar);
            Assert.That(hebrewCalendar.GetYear(dateTime), Is.EqualTo(5770));
            Assert.That(hebrewCalendar.GetMonth(dateTime), Is.EqualTo(8));
            Assert.That(hebrewCalendar.GetDayOfMonth(dateTime), Is.EqualTo(1));
        }
    }
}