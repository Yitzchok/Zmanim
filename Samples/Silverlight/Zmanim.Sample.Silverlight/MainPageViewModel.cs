using System;
using Zmanim.Sample.Silverlight.Observable;
using Zmanim.Utilities;

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
            get { return ZmanimCalendar.GeoLocation.getLatitude(); }
            set
            {
                ZmanimCalendar.GeoLocation.setLatitude(value);
                this.RaisePropertyChanged(x => x.Latitude);
                this.RaisePropertyChanged(x => x.Zmanim);
            }
        }

        public double Longitude
        {
            get { return ZmanimCalendar.GeoLocation.getLongitude(); }
            set
            {
                ZmanimCalendar.GeoLocation.setLongitude(value);
                this.RaisePropertyChanged(x => x.Longitude);
                this.RaisePropertyChanged(x => x.Zmanim);
            }
        }

        public double Elevation
        {
            get { return ZmanimCalendar.GeoLocation.getElevation(); }
            set
            {
                ZmanimCalendar.GeoLocation.setElevation(value);
                this.RaisePropertyChanged(x => x.Elevation);
                this.RaisePropertyChanged(x => x.Zmanim);
            }
        }

        public string LocationName { get { return ZmanimCalendar.GeoLocation.getLocationName(); } }
        public Zmanim Zmanim { get; set; }
    }
}