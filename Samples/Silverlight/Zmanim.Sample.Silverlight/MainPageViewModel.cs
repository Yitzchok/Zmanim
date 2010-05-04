using System;
using Zmanim.Sample.Silverlight.Observable;

namespace Zmanim.Sample.Silverlight
{
    public class MainPageViewModel : ObservableBase
    {
        public ComplexZmanimCalendar ZmanimCalendar { get; set; }

        public MainPageViewModel(ComplexZmanimCalendar zmanimCalendar)
        {
            ZmanimCalendar = zmanimCalendar;
            Zmanim = new Zmanim(zmanimCalendar);
        }

        public DateTime Date
        {
            get { return ZmanimCalendar.Calendar.Date; }
            set { ZmanimCalendar.Calendar.Date = value; }
        }

        public string LocationName { get { return ZmanimCalendar.GeoLocation.getLocationName(); } }
        public Zmanim Zmanim { get; set; }
    }
}