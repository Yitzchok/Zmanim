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
            Date sunset = calendar.getSunset();

            Assert.That(sunset.toString(), Is.EqualTo("Fri Apr 02 19:21:28 EDT 2010"));
        }

        [Test]
        public void Can_get_CandelLighting_time()
        {
            Date candle = calendar.getCandelLighting();

            Assert.That(candle.toString(), Is.EqualTo("Fri Apr 02 19:03:28 EDT 2010"));
        }

        [Test]
        public void Can_get_time_for_shma_acording_to_the_magen_avrahom()
        {
            Date shma = calendar.getSofZmanShmaMGA();

            Assert.That(shma.toString(), Is.EqualTo("Fri Apr 02 09:13:10 EDT 2010"));
        }
    }
}