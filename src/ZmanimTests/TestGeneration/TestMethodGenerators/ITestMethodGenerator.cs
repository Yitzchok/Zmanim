using System;
using System.Collections.Generic;
using ZmanimTests.TestGeneration.TestFormatters;

namespace ZmanimTests.TestGeneration.TestMethodGenerators
{
    public interface ITestMethodGenerator
    {
        void Generate(Type type, Func<object> typeObject, IEnumerable<ITestFormatter> testFormatters);
    }
}