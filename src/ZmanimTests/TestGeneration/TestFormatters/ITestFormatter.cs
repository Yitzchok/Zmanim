using System.Collections.Generic;
using java.util;

namespace ZmanimTests.TestGeneration.TestFormatters
{
    public interface ITestFormatter
    {
        ITestFormatter SetClassName(string name);
        ITestFormatter AddTestMethod(string methodName, string testBody);
        ITestFormatter AddDateTestMethod(string methodName, Date date);
        IList<string> TestMethods { get; set; }
        string BuildTestClass();
        string ClassName { get; set; }
    }
}