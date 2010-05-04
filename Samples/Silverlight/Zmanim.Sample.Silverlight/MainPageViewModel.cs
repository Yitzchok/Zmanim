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
            set
            {
                ZmanimCalendar.Calendar.Date = value;
                this.RaisePropertyChanged(x => x.Date);
                this.RaisePropertyChanged(x => x.Zmanim);
            }
        }

        public double Latitude
        {
            get { return ZmanimCalendar.GeoLocation.Latitude; }
            set
            {
                ZmanimCalendar.GeoLocation.Latitude = value;
                this.RaisePropertyChanged(x => x.Latitude);
                this.RaisePropertyChanged(x => x.Zmanim);
            }
        }

        public double Longitude
        {
            get { return ZmanimCalendar.GeoLocation.Longitude; }
            set
            {
                ZmanimCalendar.GeoLocation.Longitude = value;
                this.RaisePropertyChanged(x => x.Longitude);
                this.RaisePropertyChanged(x => x.Zmanim);
            }
        }

        public double Elevation
        {
            get { return ZmanimCalendar.GeoLocation.Elevation; }
            set
            {
                ZmanimCalendar.GeoLocation.Elevation = value;
                this.RaisePropertyChanged(x => x.Elevation);
                this.RaisePropertyChanged(x => x.Zmanim);
            }
        }

        public string LocationName { get { return ZmanimCalendar.GeoLocation.LocationName; } }
        public Zmanim Zmanim { get; set; }
    }
}