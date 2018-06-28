using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Zmanim;
using Zmanim.TimeZone;
using Zmanim.Utilities;

namespace ZmanimTests
{
    public class ZmanMiscTests : ZmanMethodGenerator
    {
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

        [Test]
        public void Check_getFixedLocalChatzosDST()
        {
            var zman = GetCalendar().GetFixedLocalChatzos();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 12, 56, 53, 311)
                ));
        }

        [Test]
        public void Check_getFixedLocalChatzosEST()
        {
            Assert.That(GetFixedLocalChatzos(new DateTime(2010, 1, 1)), Is.EqualTo(new DateTime(2010, 1, 1, 11, 56, 53, 312)));
            Assert.That(GetFixedLocalChatzos(new DateTime(2010, 1, 1, 1, 20, 0)), Is.EqualTo(new DateTime(2010, 1, 1, 11, 56, 53, 312)));
            Assert.That(GetFixedLocalChatzos(new DateTime(2010, 1, 8, 2, 0, 10)), Is.EqualTo(new DateTime(2010, 1, 8, 11, 56, 53, 312)));
            Assert.That(GetFixedLocalChatzos(new DateTime(2010, 1, 25, 3, 30, 5)), Is.EqualTo(new DateTime(2010, 1, 25, 11, 56, 53, 312)));
            Assert.That(GetFixedLocalChatzos(new DateTime(2010, 2, 1, 8, 15, 0)), Is.EqualTo(new DateTime(2010, 2, 1, 11, 56, 53, 312)));
            Assert.That(GetFixedLocalChatzos(new DateTime(2010, 3, 5, 10, 50, 55)), Is.EqualTo(new DateTime(2010, 3, 5, 11, 56, 53, 312)));
        }

        private DateTime? GetFixedLocalChatzos(DateTime dateTime)
        {
            var calendar = GetCalendar();
            calendar.DateWithLocation = new DateWithLocation(dateTime, calendar.DateWithLocation.Location);
            return calendar.GetFixedLocalChatzos();
        }

        [Test, Ignore("Helpers")]
        public void ZmanimCalendarToXml()
        {
            string s = GetCalendar().ToString();
        }

        [Test, Ignore("Helpers")]
        public void TestsMethodsToCsv()
        {
            StringBuilder csvStringBuilder = new StringBuilder("MethodName,DateCalculated");
            Type type = typeof(ComplexZmanimCalendar);

            foreach (var method in type.GetMethods()
                .Where(m => m.ReturnType == typeof(DateTime)
                            && m.Name.ToLowerInvariant().StartsWith("get")
                            && m.IsPublic == true
                            && m.GetParameters().Count() == 0))
            {
                var date = (DateTime)method.Invoke(GetCalendar(), null);

                csvStringBuilder.AppendFormat("{0},{1}{2}", method.Name, date, Environment.NewLine);

            }

            string csvTestResults = csvStringBuilder.ToString();
        }
    }
}