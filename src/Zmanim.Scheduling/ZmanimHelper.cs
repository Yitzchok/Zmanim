using System;
using java.util;
using net.sourceforge.zmanim;
using net.sourceforge.zmanim.util;
using Zmanim.Extensions;

namespace Zmanim.Scheduling
{
    public static class ZmanimHelper
    {
        public static bool IsShabbos(this ComplexZmanimCalendar calendar)
        {
            bool isShabbos = false;

            var timeUtc = calendar.getCalendar().getTime().ToDateTime().ToUniversalTime();

            if (timeUtc.DayOfWeek == DayOfWeek.Friday)
                isShabbos = timeUtc > calendar.getCandelLighting().ToDateTime().ToUniversalTime();
            if (timeUtc.DayOfWeek == DayOfWeek.Saturday)
                isShabbos = timeUtc <= calendar.getTzais().ToDateTime().ToUniversalTime();

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
            complexZmanimCalendar.setCalendar(new GregorianCalendar(date.Year, date.Month - 1, date.Day));

            return complexZmanimCalendar;
        }

        public static ComplexZmanimCalendar SetCalendarDate(this ComplexZmanimCalendar calendar, DateTime date)
        {
            calendar.setCalendar(new GregorianCalendar(date.Year, date.Month - 1, date.Day));

            return calendar;
        }

        public static ComplexZmanimCalendar GetCalendar(this Location location)
        {
            var complexZmanimCalendar =
                new ComplexZmanimCalendar(new GeoLocation(location.LocationName, location.Latitude, location.Longitude,
                                                          location.Elevation,
                                                          java.util.TimeZone.getTimeZone(location.TimeZone)));

            return complexZmanimCalendar;
        }
    }
}