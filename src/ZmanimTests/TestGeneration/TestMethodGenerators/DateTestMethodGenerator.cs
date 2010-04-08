using System;
using System.Collections.Generic;
using System.Linq;
using java.util;
using ZmanimTests.TestGeneration.TestFormatters;

namespace ZmanimTests.TestGeneration.TestMethodGenerators
{
    public class DateTestMethodGenerator : ITestMethodGenerator
    {
        public void Generate(Type type, Func<object> typeObject, IEnumerable<ITestFormatter> testFormatters)
        {
            foreach (var method in type.GetMethods()
                .Where(m => m.ReturnType == typeof(Date)
                            && m.Name.StartsWith("get")
                            && m.IsPublic
                            && m.GetParameters().Count() == 0))
            {
                Date date = (Date)method.Invoke(typeObject(), null);
                foreach (var formatter in testFormatters)
                    formatter.AddDateTestMethod(method.Name, date);
            }
        }
    }
}