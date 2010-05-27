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
        public void Will_return_null_when_trying_to_get_the_zman()
        {
            var calendar = new ComplexZmanimCalendar(
                new DateTime(2010, 5, 27),
                new GeoLocation("Gateshead, England", 54.9593729, -1.6018252, 0, new WindowsTimeZone(TimeZoneInfo.Utc))
                );

            Assert.That(calendar.GetAlos16Point1Degrees(), Is.Null);
        }
    }
}