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
            get { return ZmanimCalendar.DateWithLocation.Date; }
            set
            {
                ZmanimCalendar.DateWithLocation.Date = value;
                this.RaisePropertyChanged(x => x.Date);
                this.RaisePropertyChanged(x => x.Zmanim);
            }
        }

        public double Latitude
        {
            get { return ZmanimCalendar.DateWithLocation.Location.Latitude; }
            set
            {
                ZmanimCalendar.DateWithLocation.Location.Latitude = value;
                this.RaisePropertyChanged(x => x.Latitude);
                this.RaisePropertyChanged(x => x.Zmanim);
            }
        }

        public double Longitude
        {
            get { return ZmanimCalendar.DateWithLocation.Location.Longitude; }
            set
            {
                ZmanimCalendar.DateWithLocation.Location.Longitude = value;
                this.RaisePropertyChanged(x => x.Longitude);
                this.RaisePropertyChanged(x => x.Zmanim);
            }
        }

        public double Elevation
        {
            get { return ZmanimCalendar.DateWithLocation.Location.Elevation; }
            set
            {
                ZmanimCalendar.DateWithLocation.Location.Elevation = value;
                this.RaisePropertyChanged(x => x.Elevation);
                this.RaisePropertyChanged(x => x.Zmanim);
            }
        }

        public string LocationName { get { return ZmanimCalendar.DateWithLocation.Location.LocationName; } }
        public Zmanim Zmanim { get; set; }
    }
}