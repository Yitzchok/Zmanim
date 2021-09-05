using System;
using NUnit.Framework;
using Zmanim;
using Zmanim.Calculator;
using Zmanim.TzDatebase;
using Zmanim.Utilities;

namespace ZmanimTests
{
    [TestFixture]
    public class TimeZoneTest
    {
        private ComplexZmanimCalendar calendar;

        [SetUp]
        public void Setup()
        {
            var location = new GeoLocation("Lakewood, NJ", 40.09596, -74.22213, 0, new OlsonTimeZone("America/New_York"));
            calendar = new ComplexZmanimCalendar(new DateTime(2010, 4, 2), location)
            {
                AstronomicalCalculator = new SunTimesCalculator()
            };
        }
        
        [Test]
        public void Check_GetSunrise()
        {
            var zman = calendar.GetSunrise()!.Value;

            Assert.That(zman.Kind, Is.EqualTo(DateTimeKind.Unspecified));
            Assert.That(zman.Hour, Is.EqualTo(6));
        }

        [Test]
        public void Check_GetSunset()
        {
            var zman = calendar.GetSunset()!.Value;

            Assert.That(zman.Kind, Is.EqualTo(DateTimeKind.Unspecified));
            Assert.That(zman.Hour, Is.EqualTo(19));
        }
    }
}