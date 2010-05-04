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
        public DateTime AlosHashachar { get { return ZmanimCalendar.getAlosHashachar(); } }
        public DateTime Alos72 { get { return ZmanimCalendar.getAlos72(); } }
        public DateTime Sunrise { get { return ZmanimCalendar.getSunrise(); } }
        public DateTime Sunset { get { return ZmanimCalendar.getSunset(); } }
        public DateTime CandelLighting { get { return ZmanimCalendar.getCandelLighting(); } }
        public DateTime Tzais { get { return ZmanimCalendar.getTzais(); } }

    }
}