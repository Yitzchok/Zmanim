using System;
using System.Collections.Generic;
using System.Text;

namespace ZmanimTests.TestGeneration.TestFormatters
{
    public class DotNetTestFormatter : ITestFormatter
    {
        public DotNetTestFormatter()
        {
            TestMethods = new List<string>();
            TestMethods.Add(@"
        //We can use these test when removing the depenency to Java (IKVM)
        //To make sure that the code stayes the same.

        private ComplexZmanimCalendar calendar;

        [SetUp]
        public void Setup()
        {
            String locationName = ""Lakewood, NJ"";
            double latitude = 40.09596; //Lakewood, NJ
            double longitude = -74.22213; //Lakewood, NJ
            double elevation = 0; //optional elevation
            ITimeZone timeZone = new OlsonTimeZone(""America/New_York"");
            GeoLocation location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            ComplexZmanimCalendar czc = new ComplexZmanimCalendar(location);

            czc.Calendar = new TimeZoneDateTime(new DateTime(2010, 4, 2));
            calendar = czc;
        }
");
        }

        public string ClassName { get; set; }
        public ITestFormatter SetClassName(string name)
        {
            ClassName = name;
            return this;
        }

        public ITestFormatter AddTestMethod(string methodName, string testBody)
        {
            TestMethods.Add(string.Format(@"
        [Test]
        public void Check_{0}()
        {{
            {1}
        }}", methodName, testBody));

            return this;
        }

        /*
        public ITestFormatter AddDateTestMethod(string methodName, Date date)
        {
            var calendar = new GregorianCalendar();
            calendar.setTime(date);

            AddTestMethod(methodName,
                string.Format(
                @"var zman = calendar.{0}().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime({1}, {2}, {3}, {4}, {5}, {6})
                ));",
                    methodName,
                    calendar.Get(Calendar.YEAR),
                    calendar.Get(Calendar.MONTH) + 1,
                    calendar.Get(Calendar.DAY_OF_MONTH),
                    calendar.Get(Calendar.HOUR_OF_DAY),
                    calendar.Get(Calendar.MINUTE),
                    calendar.Get(Calendar.SECOND),
                    calendar.Get(Calendar.MILLISECOND)
                    ));
            return this;
        }
        */

        public ITestFormatter AddDateTimeTestMethod(string methodName, DateTime date)
        {
            AddTestMethod(methodName,
                 string.Format(
                 @"var zman = calendar.{0}();

            Assert.That(zman, Is.EqualTo(
                    new DateTime({1}, {2}, {3}, {4}, {5}, {6})
                ));",
                     methodName,
                     date.Year,
                     date.Month - 1,
                     date.Day,
                     date.Hour,
                     date.Minute,
                     date.Second,
                     date.Millisecond
                     ));
            return this;
        }

        public ITestFormatter AddLongTestMethod(string methodName, long testResult)
        {
            AddTestMethod(methodName,
                string.Format(@"Assert.That(calendar.{0}(), Is.EqualTo({1}));",
                    methodName, testResult));

            return this;
        }

        public IList<string> TestMethods { get; set; }

        public string BuildTestClass()
        {
            var sb = new StringBuilder();

            foreach (var testMethod in TestMethods)
                sb.AppendLine(testMethod);

            return string.Format(@"
using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Zmanim;
using Zmanim.TimeZone;
using Zmanim.TzDatebase;
using Zmanim.Utilities;

namespace ZmanimTests
{{
    [TestFixture]
    public class {0}
    {{
        {1}
    }}
}}", ClassName, sb);
        }
    }
}