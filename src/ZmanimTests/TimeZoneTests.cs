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
    }
}