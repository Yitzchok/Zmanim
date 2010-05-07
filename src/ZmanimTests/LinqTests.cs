using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using Zmanim;
using Zmanim.TimeZone;
using Zmanim.Utilities;

namespace ZmanimTests
{
    [TestFixture]
    public class LinqTests
    {
        [Test]
        public void Can_select_over_linq()
        {
            var location = new GeoLocation("Lakewood, NJ", 40.09596, -74.22213, 0, new WindowsTimeZone(TimeZoneInfo.Local));

            var days = from day in GetDaysInHebrewYear(DateTime.Now, location)
                       let sunrise = day.GetSunrise()
                       where sunrise.Hour >= 5 && sunrise.Minute > 45
                       select sunrise;


            var itemCount = days.Count();
        }

        public IEnumerable<ComplexZmanimCalendar> GetDaysInHebrewMonth(DateTime yearAndMonth, GeoLocation location)
        {
            Calendar calendar = new HebrewCalendar();
            var daysInMonth = calendar.GetDaysInMonth(calendar.GetYear(yearAndMonth), calendar.GetMonth(yearAndMonth));

            for (int i = 0; i < daysInMonth; i++)
            {
                var zmanimCalendar = new ComplexZmanimCalendar(location);
                zmanimCalendar.Calendar.Date = new DateTime(yearAndMonth.Year, yearAndMonth.Month, i + 1);
                yield return zmanimCalendar;
            }
        }


        public IEnumerable<ComplexZmanimCalendar> GetDaysInHebrewYear(DateTime year, GeoLocation location)
        {
            Calendar calendar = new HebrewCalendar();
            var currentYear = calendar.GetYear(year);
            var amountOfMonths = calendar.GetMonthsInYear(currentYear);

            for (int i = 0; i < amountOfMonths; i++)
            {
                var currentMonth = i + 1;
                var daysInMonth = calendar.GetDaysInMonth(currentYear, currentMonth);

                for (int dayOfMonth = 0; dayOfMonth < daysInMonth; dayOfMonth++)
                {
                    var zmanimCalendar = new ComplexZmanimCalendar(location);
                    zmanimCalendar.Calendar.Date = new DateTime(currentYear, currentMonth, dayOfMonth + 1, calendar);
                    yield return zmanimCalendar;
                }
            }
        }
    }
}