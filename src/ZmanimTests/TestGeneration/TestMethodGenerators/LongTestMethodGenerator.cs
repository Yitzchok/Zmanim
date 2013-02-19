using System;
using System.Collections.Generic;
using System.Linq;
using ZmanimTests.TestGeneration.TestFormatters;

namespace ZmanimTests.TestGeneration.TestMethodGenerators
{
    public class LongTestMethodGenerator : ITestMethodGenerator
    {
        public void Generate(Type type, Func<object> typeObject, IEnumerable<ITestFormatter> testFormatters)
        {
            foreach (var method in type.GetMethods()
                .Where(m => m.ReturnType == typeof(long)
                            && m.Name.ToLowerInvariant().StartsWith("get")
                            && m.IsPublic
                            && !m.GetParameters().Any()))
            {
                var result = (long)method.Invoke(typeObject(), null);
                foreach (var formatter in testFormatters)
                    formatter.AddLongTestMethod(method.Name, result);
            }
        }
    }
}