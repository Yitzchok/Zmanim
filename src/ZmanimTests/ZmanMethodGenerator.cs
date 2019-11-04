using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Zmanim;
using ZmanimTests.TestGeneration.TestFormatters;
using ZmanimTests.TestGeneration.TestMethodGenerators;

namespace ZmanimTests
{
    public class ZmanMethodGenerator : BaseZmanimTests
    {
        [Test, Ignore("Generate Tests")]
        public void GenerateTestsFixtures()
        {
            var testFormatters = new List<ITestFormatter>
                                     {
                                         /*new DotNetTestFormatter{ClassName = "ZmanimTest"},
                                         new JavaTestFormatter{ClassName = "ZmanimTest"},*/
                                         new DotNetTestFormatter{ClassName = "ZmanimTest"},
                                         new DotNetTestFormatterWithMilliseconds{ClassName = "ZmanimTestWithMilliseconds"},
                                         new JavaTestFormatter{ClassName = "ZmanimTest"},
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
            File.WriteAllText("ZmanimTestWithMilliseconds.java", javaTests);
        }
    }
}