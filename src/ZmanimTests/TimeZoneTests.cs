using System;
using NUnit.Framework;
using Zmanim;
using Zmanim.TimeZone;
using Zmanim.TzDatebase;
using Zmanim.Utilities;

namespace ZmanimTests
{
    [TestFixture]
    public class TimeZoneTests
    {
        [Test]
        public void LocalTimeConversion_should_correctly_use_time_for_daylight_saving_day_of_transition()
        {
            ITimeZone timeZone = new OlsonTimeZone("America/New_York");
            var location = new GeoLocation("Lakewood, NJ", 40.09596, -74.22213, 0, timeZone);
            var czc = new ComplexZmanimCalendar(DateTime.Parse("11/3/2019"), location);

            var sunrise = czc.GetSunrise();

            Assert.That(sunrise.Value.Hour, Is.EqualTo(6));
        }

        [Test]
        public void Check_is_offset_timezone_working()
        {
            String locationName = "Lakewood, NJ";
            double latitude = 40.09596; //Lakewood, NJ
            double longitude = -74.22213; //Lakewood, NJ
            double elevation = 0; //optional elevation
            var timeZone = new OffsetTimeZone(new TimeSpan(0, 0, -14400));
            var location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            var czc = new ComplexZmanimCalendar(new DateTime(2010, 4, 2), location);

            var zman = czc.GetSunrise();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 6, 39, 41, 832)
                ));
        }
    }
}