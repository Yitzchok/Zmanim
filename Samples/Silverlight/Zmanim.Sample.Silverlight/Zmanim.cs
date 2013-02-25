using System;

namespace Zmanim.Sample.Silverlight
{
    public class Zmanim
    {
        public Zmanim(ComplexZmanimCalendar zmanimCalendar)
        {
            ZmanimCalendar = zmanimCalendar;
        }

        public ComplexZmanimCalendar ZmanimCalendar { get; set; }
        public DateTime? AlosHashachar { get { return ZmanimCalendar.GetAlosHashachar(); } }
        public DateTime? Alos72 { get { return ZmanimCalendar.GetAlos72(); } }
        public DateTime? Sunrise { get { return ZmanimCalendar.GetSunrise(); } }
        public DateTime? Sunset { get { return ZmanimCalendar.GetSunset(); } }
        public DateTime? CandleLighting { get { return ZmanimCalendar.GetCandleLighting(); } }
        public DateTime? Tzais { get { return ZmanimCalendar.GetTzais(); } }

    }
}