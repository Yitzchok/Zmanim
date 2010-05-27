using System;
using NUnit.Framework;
using Zmanim;
using Zmanim.TimeZone;
using Zmanim.Utilities;

namespace ZmanimTests
{
    [TestFixture]
    public class ZmanimUnavailableLocationTest
    {
        [Test]
        public void Returns_DateTime_MinValue_when_trying_to_get_a_zman_for_a_location_that_cant_be_calculated()
        {
            var calendar = new ComplexZmanimCalendar(
                new DateTime(2010, 5, 27),
                new GeoLocation("Gateshead, England", 54.9593729, -1.6018252, 0, new WindowsTimeZone(TimeZoneInfo.Utc))
                );

            Assert.That(calendar.GetAlos16Point1Degrees(), Is.Null);
        }
    }
}