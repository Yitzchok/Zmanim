using System;
using System.Collections.Generic;
using java.util;
using net.sourceforge.zmanim;
using net.sourceforge.zmanim.util;
using NUnit.Framework;
using ZmanimTests.TestGeneration.TestFormatters;
using ZmanimTests.TestGeneration.TestMethodGenerators;
using TimeZone = java.util.TimeZone;

namespace ZmanimTests
{
    public class ZmanMethodGenerator
    {
        public ComplexZmanimCalendar GetCalendar()
        {
            String locationName = "Lakewood, NJ";
            double latitude = 40.09596; //Lakewood, NJ
            double longitude = -74.22213; //Lakewood, NJ
            double elevation = 0; //optional elevation
            TimeZone timeZone = TimeZone.getTimeZone("America/New_York");
            GeoLocation location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            ComplexZmanimCalendar czc = new ComplexZmanimCalendar(location);

            czc.setCalendar(new GregorianCalendar(2010, 3, 2));

            /*
            string locationName = "Brooklyn, NY";
            double latitude = 40.618851; //Brooklyn, NY
            double longitude = -73.985921; //Brooklyn, NY
            double elevation = 0; //optional elevation
            TimeZone timeZone = TimeZone.getTimeZone("America/New_York");
            var location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            var czc = new ComplexZmanimCalendar(location);
            czc.setCalendar(new GregorianCalendar(2010, 3, 2));
            */
            return czc;
        }

        [Test, Ignore]
        public void GenerateTestsFixtures()
        {
            var testFormatters = new List<ITestFormatter>
                                     {
                                         new DotNetTestFormatter{ClassName = "ZmanimTest"},
                                         new JavaTestFormatter{ClassName = "ZmanimTest"}
                                     };

            var testMethodGenerators = new List<ITestMethodGenerator> { new DateTestMethodGenerator() };

            Type type = typeof(ComplexZmanimCalendar);
            testMethodGenerators.ForEach(generator =>
                generator.Generate(type, GetCalendar, testFormatters));

            // Here are the outputed Test Fixtures.
            string dotNetTests = testFormatters[0].BuildTestClass();
            string javaTests = testFormatters[1].BuildTestClass();
        }
    }
}