using System;

namespace ZmanimTests
{
    public static class TestDateExtensions
    {
        public static DateTime RemoveMilliseconds(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public static DateTime RemoveMilliseconds(this DateTime? dateTime)
        {
            return new DateTime(dateTime.Value.Year, dateTime.Value.Month, dateTime.Value.Day, dateTime.Value.Hour, dateTime.Value.Minute, dateTime.Value.Second);
        }
    }
}