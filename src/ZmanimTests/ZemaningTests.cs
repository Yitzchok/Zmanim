using java.util;
using net.sourceforge.zmanim;
using net.sourceforge.zmanim.util;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace ZmanimTests
{
    [TestFixture]
    public class ZemaningTests
    {
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
        public void Can_get_sunrise_time()
        {
            Date sunrise = calendar.getSunrise();

            Assert.That(sunrise.toString(), Is.EqualTo("Fri Apr 02 06:38:24 EDT 2010"));
        }

        [Test]
        public void Can_get_sunset_time()
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