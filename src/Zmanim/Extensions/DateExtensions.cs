using System;
using java.util;

namespace Zmanim.Extensions
{
    public static class DateExtensions
    {
        public static DateTime ToDateTimeWithMilliseconds(this Date javaDate)
        {
            var calender = new GregorianCalendar();
            calender.setTime(javaDate);


            //return DateTime.Parse(javaDate.toGMTString()).ToLocalTime();
            return new DateTime(calender.get(Calendar.YEAR), calender.get(Calendar.MONTH) + 1,
                calender.get(Calendar.DAY_OF_MONTH), calender.get(Calendar.HOUR),
                calender.get(Calendar.MINUTE),
                calender.get(Calendar.SECOND),
                calender.get(Calendar.MILLISECOND));
        }

        public static DateTime ToDateTime(this Date javaDate)
        {
            var calender = new GregorianCalendar();
            calender.setTime(javaDate);


            //return DateTime.Parse(javaDate.toGMTString()).ToLocalTime();
            return new DateTime(calender.get(Calendar.YEAR), calender.get(Calendar.MONTH) + 1,
                calender.get(Calendar.DAY_OF_MONTH), calender.get(Calendar.HOUR),
                calender.get(Calendar.MINUTE),
                calender.get(Calendar.SECOND)
                );
        }
    }
}