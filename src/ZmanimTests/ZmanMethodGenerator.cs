using System;
using System.Collections.Generic;
using System.Text;
using java.util;
using net.sourceforge.zmanim;
using net.sourceforge.zmanim.util;
using NUnit.Framework;
using System.Linq;
using TimeZone = java.util.TimeZone;

namespace ZmanimTests
{
    public class ZmanMethodGenerator
    {
        public ComplexZmanimCalendar GetCalendar()
        {
            string locationName = "Brooklyn, NY";
            double latitude = 40.618851; //Brooklyn, NY
            double longitude = -73.985921; //Brooklyn, NY
            double elevation = 0; //optional elevation
            TimeZone timeZone = TimeZone.getTimeZone("America/New_York");
            var location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            var czc = new ComplexZmanimCalendar(location);
            czc.setCalendar(new GregorianCalendar(2010, 3, 2));
            return czc;
        }

        [Test, Ignore]
        public void GenerateMethods()
        {
            //IList<string> methods = new List<string>();
            StringBuilder sb = new StringBuilder();
            Type type = typeof(ComplexZmanimCalendar);
            var calendar = new GregorianCalendar();

            foreach (var method in type.GetMethods()
                .Where(m => m.ReturnType == typeof(Date)
                    && m.Name.StartsWith("get")
                    && m.IsPublic == true
                    && m.GetParameters().Count() == 0))
            {
                Date date = (Date)method.Invoke(GetCalendar(), null);
                calendar.setTime(date);


                sb.AppendFormat(@"
        [Test]
        public void Check_{0}()
        {{
            var zman = calendar.{0}().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime({1}, {2}, {3}, {4}, {5}, {6})
                ));
        }}
                ", method.Name,
                    calendar.get(Calendar.YEAR),
                    calendar.get(Calendar.MONTH) + 1,
                    calendar.get(Calendar.DAY_OF_MONTH),
                    calendar.get(Calendar.HOUR_OF_DAY),
                    calendar.get(Calendar.MINUTE),
                    calendar.get(Calendar.SECOND)
                    /*,calender.get(Calendar.MILLISECOND)*/
                );

            }

            string finalTestMethods = sb.ToString();
        }
    }
}