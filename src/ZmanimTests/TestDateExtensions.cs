using System;

namespace ZmanimTests
{
    public static class TestDateExtensions
    {
        public static DateTime RemoveMilliseconds(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }
    }
}