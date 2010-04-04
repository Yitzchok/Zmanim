using System;
using java.util;
using net.sourceforge.zmanim;
using net.sourceforge.zmanim.util;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Zmanim.Extensions;
using TimeZone = java.util.TimeZone;

namespace ZmanimTests
{
    [TestFixture]
    public class ZemaningTests
    {
        //We can use these test when removing the depenency to Java (IKVM)
        //To make sure that the code stayes the same.

        private ComplexZmanimCalendar calendar;

        [SetUp]
        public void Setup()
        {
            string locationName = "Brooklyn, NY";
            double latitude = 40.618851; //Brooklyn, NY
            double longitude = -73.985921; //Brooklyn, NY
            double elevation = 0; //optional elevation
            TimeZone timeZone = TimeZone.getTimeZone("America/New_York");
            var location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            var czc = new ComplexZmanimCalendar(location);
            czc.setCalendar(new GregorianCalendar(2010, 3, 2));
            calendar = czc;
        }

        [Test]
        public void Check_getSunrise()
        {
            var sunrise = calendar.getSunrise().ToDateTime();

            Assert.That(sunrise, Is.EqualTo(
                    new DateTime(2010, 4, 2, 6, 38, 24)
                ));
        }

        [Test]
        public void Check_getSunset()
        {
            var sunset = calendar.getSunset().ToDateTime();

            Assert.That(sunset, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 21, 28)
                ));
        }

        [Test]
        public void Check_getCandelLighting()
        {
            var candleLighting = calendar.getCandelLighting().ToDateTime();

            Assert.That(candleLighting, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 03, 28)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA()
        {
            var sofZmanShmaMGA = calendar.getSofZmanShmaMGA().ToDateTime();

            Assert.That(sofZmanShmaMGA, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 13, 10)
                ));
        }
    }
}