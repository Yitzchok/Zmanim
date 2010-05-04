using System.Windows.Controls;
using Zmanim.TimeZone;
using Zmanim.Utilities;

namespace Zmanim.Sample.Silverlight
{
    public partial class MainPage : UserControl
    {
        readonly MainPageViewModel mainPageViewModel;
        public MainPage()
        {
            var zman = GetZmanim();
            mainPageViewModel = new MainPageViewModel(GetZmanim());
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
}
