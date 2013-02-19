using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Zmanim;
using Zmanim.TimeZone;
using Zmanim.TzDatebase;
using Zmanim.Utilities;
using ZmanimTests.TestGeneration.TestFormatters;
using ZmanimTests.TestGeneration.TestMethodGenerators;

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
            ITimeZone timeZone = new OlsonTimeZone("America/New_York");
            GeoLocation location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            ComplexZmanimCalendar czc = new ComplexZmanimCalendar(new DateTime(2010, 4, 2), location);

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
                                         /*new DotNetTestFormatter{ClassName = "ZmanimTest"},
                                         new JavaTestFormatter{ClassName = "ZmanimTest"},*/
                                         new DotNetTestFormatter{ClassName = "ZmanimTest"},
                                         new DotNetTestFormatterWithMilliseconds{ClassName = "ZmanimTestWithMilliseconds"},
                                         new JavaTestFormatterWithMilliseconds{ClassName = "ZmanimTestWithMilliseconds"}
                                     };

            var testMethodGenerators = new List<ITestMethodGenerator>
                                           {
                                               new DateTimeTestMethodGenerator(),
                                               //new DateTestMethodGenerator(),
                                               new LongTestMethodGenerator()
                                           };

            Type type = typeof(ComplexZmanimCalendar);
            testMethodGenerators.ForEach(generator =>
                generator.Generate(type, GetCalendar, testFormatters));

            // Here are the outputed Test Fixtures.
            string dotNetTests = testFormatters[0].BuildTestClass();
            string dotNetMilliTests = testFormatters[1].BuildTestClass();
            string javaTests = testFormatters[2].BuildTestClass();

            File.WriteAllText("ZmanimTests.cs", dotNetTests);
            File.WriteAllText("ZmanimTestWithMilliseconds.cs", dotNetMilliTests);
            File.WriteAllText("ZmanimTest.java", javaTests);
        }
    }
}