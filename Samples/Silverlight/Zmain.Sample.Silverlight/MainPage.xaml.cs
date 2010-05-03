using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Zmanim;
using Zmanim.TimeZone;
using Zmanim.Utilities;

namespace Zmain.Sample.Silverlight
{
    public partial class MainPage : UserControl
    {
        readonly MainPageViewModel mainPageViewModel;
        public MainPage()
        {
            var zman = GetZmanim();
            mainPageViewModel = new MainPageViewModel("Lakewood, NJ", zman.getSunrise(), zman.getSunset());
            this.DataContext = mainPageViewModel;
            InitializeComponent();
        }

        private ComplexZmanimCalendar GetZmanim()
        {
            string locationName = "Lakewood, NJ";
            double latitude = 40.09596; //Lakewood, NJ
            double longitude = -74.22213; //Lakewood, NJ
            double elevation = 0; //optional elevation
            var timeZone = new WindowsTimeZone();
            var location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            return new ComplexZmanimCalendar(location);
        }
    }

    public class MainPageViewModel
    {
        public MainPageViewModel(string locationName, DateTime sunrise, DateTime sunset)
        {
            LocationName = locationName;
            Sunrise = sunrise;
            Sunset = sunset;
        }

        public MainPageViewModel() { }
        public string LocationName { get; set; }
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
    }
}
