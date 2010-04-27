using System;
using System.Linq;
using System.Text;
using net.sourceforge.zmanim;
using NUnit.Framework;

namespace ZmanimTests
{
    public class ZmanMiscTests : ZmanMethodGenerator
    {
        [Test, Ignore]
        public void ZmanimCalendarToXml()
        {
            string s = GetCalendar().ToString();
        }

        [Test, Ignore]
        public void TestsMethodsToCsv()
        {
            StringBuilder csvStringBuilder = new StringBuilder("MethodName,DateCalculated");
            Type type = typeof(ComplexZmanimCalendar);

            foreach (var method in type.GetMethods()
                .Where(m => m.ReturnType == typeof(DateTime)
                            && m.Name.StartsWith("get")
                            && m.IsPublic == true
                            && m.GetParameters().Count() == 0))
            {
                var date = (DateTime)method.Invoke(GetCalendar(), null);

                csvStringBuilder.AppendFormat("{0},{1}{2}", method.Name, date, Environment.NewLine);

            }

            string csvTestResults = csvStringBuilder.ToString();
        }
    }
}