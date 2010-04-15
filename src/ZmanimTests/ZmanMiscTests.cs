using System;
using System.Globalization;
using System.Linq;
using System.Text;
using java.util;
using net.sourceforge.zmanim;
using NUnit.Framework;
using GregorianCalendar = java.util.GregorianCalendar;

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
            var calendar = new GregorianCalendar();

            foreach (var method in type.GetMethods()
                .Where(m => m.ReturnType == typeof(Date)
                            && m.Name.StartsWith("get")
                            && m.IsPublic == true
                            && m.GetParameters().Count() == 0))
            {
                Date date = (Date)method.Invoke(GetCalendar(), null);
                calendar.setTime(date);

                csvStringBuilder.AppendFormat("{0},{1}{2}", method.Name, date.toString(), Environment.NewLine);

            }

            string csvTestResults = csvStringBuilder.ToString();
        }
    }
}