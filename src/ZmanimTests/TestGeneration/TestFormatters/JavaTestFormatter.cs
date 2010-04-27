using System;
using System.Collections.Generic;
using System.Text;

namespace ZmanimTests.TestGeneration.TestFormatters
{
    public class JavaTestFormatter : ITestFormatter
    {
        public JavaTestFormatter()
        {
            TestMethods = new List<string>();
            TestMethods.Add(@"
    private ComplexZmanimCalendar calendar;

    @Before
    public void Setup() {

        String locationName = ""Lakewood, NJ"";
        double latitude = 40.09596; //Lakewood, NJ
        double longitude = -74.22213; //Lakewood, NJ
        double elevation = 0; //optional elevation
        TimeZone timeZone = TimeZone.getTimeZone(""America/New_York"");
        GeoLocation location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
        ComplexZmanimCalendar czc = new ComplexZmanimCalendar(location);
        czc.setCalendar(new GregorianCalendar(2010, 3, 2));
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
        @Test
        public void Check_{0}(){{
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
                @"Date zman = calendar.{0}();
   Assert.assertEquals(new GregorianCalendar({1}, {2}, {3}, {4}, {5}, {6}).getTime().toString(), zman.toString());",
                    methodName,
                    calendar.get(Calendar.YEAR),
                    calendar.get(Calendar.MONTH),
                    calendar.get(Calendar.DAY_OF_MONTH),
                    calendar.get(Calendar.HOUR_OF_DAY),
                    calendar.get(Calendar.MINUTE),
                    calendar.get(Calendar.SECOND),
                    calendar.get(Calendar.MILLISECOND)
                    ));
            return this;
        }
        */

        public ITestFormatter AddDateTimeTestMethod(string methodName, DateTime date)
        {
            AddTestMethod(methodName,
                string.Format(
                @"Date zman = calendar.{0}();
   Assert.assertEquals(new GregorianCalendar({1}, {2}, {3}, {4}, {5}, {6}).getTime().toString(), zman.toString());",
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
                 string.Format(@"Assert.assertEquals({1}, calendar.{0}());",
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
import org.junit.*;

import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.TimeZone;

import net.sourceforge.zmanim.*;
import net.sourceforge.zmanim.util.*;

public class {0}{{
        {1}
}}
", ClassName, sb);
        }
    }
}