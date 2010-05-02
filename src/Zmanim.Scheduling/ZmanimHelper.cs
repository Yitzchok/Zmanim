using System;
using Zmanim.TimeZone;
using Zmanim.Utilities;

namespace Zmanim.Scheduling
{
    public static class ZmanimHelper
    {
        public static bool IsShabbos(this ComplexZmanimCalendar calendar)
        {
            bool isShabbos = false;

            var timeUtc = calendar.getCalendar().Date.ToUniversalTime();

            if (timeUtc.DayOfWeek == DayOfWeek.Friday)
                isShabbos = timeUtc > calendar.getCandelLighting().ToUniversalTime();
            if (timeUtc.DayOfWeek == DayOfWeek.Saturday)
                isShabbos = timeUtc <= calendar.getTzais().ToUniversalTime();

            return isShabbos;
        }

        public static bool IsShabbos(this DateTime date, Location location)
        {
            return IsShabbos(GetCalendar(location, date));
        }

        public static DateTime GetZman(this Location location, DateTime date, Func<ComplexZmanimCalendar, DateTime> zman)
        {
            return zman.Invoke(GetCalendar(location, date)).ToUniversalTime();
        }

        public static ComplexZmanimCalendar GetCalendar(this Location location, DateTime date)
        {
            var complexZmanimCalendar = GetCalendar(location);
            complexZmanimCalendar.setCalendar(new TimeZoneDateTime(new DateTime(date.Date.Year, date.Date.Month, date.Date.Day)));

            return complexZmanimCalendar;
        }

        public static ComplexZmanimCalendar SetCalendarDate(this ComplexZmanimCalendar calendar, DateTime date)
        {
            calendar.setCalendar(new TimeZoneDateTime(new DateTime(date.Date.Year, date.Date.Month, date.Date.Day)));
            return calendar;
        }

        public static ComplexZmanimCalendar GetCalendar(this Location location)
        {
            var complexZmanimCalendar =
                new ComplexZmanimCalendar(new GeoLocation(location.LocationName, location.Latitude, location.Longitude,
                                                          location.Elevation,
                                                          new OlsonTimeZone(location.TimeZone)));

            return complexZmanimCalendar;
        }
    }
}