using System;
using System.Collections.Generic;

namespace ZmanimTests.TestGeneration.TestFormatters
{
    public interface ITestFormatter
    {
        ITestFormatter SetClassName(string name);
        ITestFormatter AddTestMethod(string methodName, string testBody);
        /*ITestFormatter AddDateTestMethod(string methodName, Date date);*/
        ITestFormatter AddDateTimeTestMethod(string methodName, DateTime? date);
        ITestFormatter AddLongTestMethod(string methodName, long testResult);
        IList<string> TestMethods { get; set; }
        string BuildTestClass();
        string ClassName { get; set; }
    }
}