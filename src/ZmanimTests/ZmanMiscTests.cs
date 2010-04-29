using System;
using System.Linq;
using System.Text;
using net.sourceforge.zmanim;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace ZmanimTests
{
    public class ZmanMiscTests : ZmanMethodGenerator
    {
        [Test]
        public void Check_getFixedLocalChatzosDST()
        {
            var zman = GetCalendar().getFixedLocalChatzos();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 12, 56, 53, 311)
                ));
        }

        [Test]
        public void Check_getFixedLocalChatzosEST()
        {
            var complexZmanimCalendar = GetCalendar();
            complexZmanimCalendar.setCalendar(new TimeZoneDateTime(new DateTime(2010, 1, 1)));
            var zman = complexZmanimCalendar.getFixedLocalChatzos();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 1, 1, 11, 56, 53, 312)
                ));
        }

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