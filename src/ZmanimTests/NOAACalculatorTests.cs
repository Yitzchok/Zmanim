using System;
using NUnit.Framework;
using Zmanim;
using Zmanim.Calculator;
using Zmanim.TimeZone;
using Zmanim.TzDatebase;
using Zmanim.Utilities;

namespace ZmanimTests
{
    public class NOAACalculatorTests
    {
        [Test]
        public void TestSunset()
        {
            var calculator = new NOAACalculator();

            string locationName = "Lakewood, NJ";
            double latitude = 40.09596; //Lakewood, NJ
            double longitude = -74.22213; //Lakewood, NJ
            double elevation = 0; //optional elevation
            ITimeZone timeZone = new OlsonTimeZone("America/New_York");
            var location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            var calendar = new ComplexZmanimCalendar(new DateTime(2010, 4, 2), location);
            calendar.AstronomicalCalculator = calculator;

            var zman = calendar.GetSunrise();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 6, 39, 17, 482)
                ));
        }
    }
}