using System;
using System.Collections.Generic;
using System.Linq;
using ZmanimTests.TestGeneration.TestFormatters;

namespace ZmanimTests.TestGeneration.TestMethodGenerators
{
    public class DateTimeTestMethodGenerator : ITestMethodGenerator
    {
        public void Generate(Type type, Func<object> typeObject, IEnumerable<ITestFormatter> testFormatters)
        {
            foreach (var method in type.GetMethods()
                   .Where(m => m.ReturnType == typeof(DateTime)
                               && m.Name.StartsWith("get")
                               && m.IsPublic
                               && m.GetParameters().Count() == 0))
            {
                var date = (DateTime)method.Invoke(typeObject(), null);
                foreach (var formatter in testFormatters)
                    formatter.AddDateTimeTestMethod(method.Name, date);
            }
        }
    }
}