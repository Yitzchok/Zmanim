using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Zmanim;

namespace ZmanimTests
{
    public class ZmanMiscTests : BaseZmanimTests
    {
        [Test]
        public void Check_getFixedLocalChatzosDST()
        {
            var zman = GetCalendar().GetFixedLocalChatzos();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 12, 56, 53, 311)
                ));
        }

        [Test]
        public void Check_getFixedLocalChatzosEST()
        {
            Assert.That(GetFixedLocalChatzos(new DateTime(2010, 1, 1)), Is.EqualTo(new DateTime(2010, 1, 1, 11, 56, 53, 312)));
            Assert.That(GetFixedLocalChatzos(new DateTime(2010, 1, 1, 1, 20, 0)), Is.EqualTo(new DateTime(2010, 1, 1, 11, 56, 53, 312)));
            Assert.That(GetFixedLocalChatzos(new DateTime(2010, 1, 8, 2, 0, 10)), Is.EqualTo(new DateTime(2010, 1, 8, 11, 56, 53, 312)));
            Assert.That(GetFixedLocalChatzos(new DateTime(2010, 1, 25, 3, 30, 5)), Is.EqualTo(new DateTime(2010, 1, 25, 11, 56, 53, 312)));
            Assert.That(GetFixedLocalChatzos(new DateTime(2010, 2, 1, 8, 15, 0)), Is.EqualTo(new DateTime(2010, 2, 1, 11, 56, 53, 312)));
            Assert.That(GetFixedLocalChatzos(new DateTime(2010, 3, 5, 10, 50, 55)), Is.EqualTo(new DateTime(2010, 3, 5, 11, 56, 53, 312)));
        }

        private DateTime? GetFixedLocalChatzos(DateTime dateTime)
        {
            var calendar = GetCalendar();
            calendar.DateWithLocation = new DateWithLocation(dateTime, calendar.DateWithLocation.Location);
            return calendar.GetFixedLocalChatzos();
        }

        [Test, Ignore("Helpers")]
        public void ZmanimCalendarToXml()
        {
            string s = GetCalendar().ToString();
        }

        [Test, Ignore("Helpers")]
        public void TestsMethodsToCsv()
        {
            StringBuilder csvStringBuilder = new StringBuilder("MethodName,DateCalculated");
            Type type = typeof(ComplexZmanimCalendar);

            foreach (var method in type.GetMethods()
                .Where(m => m.ReturnType == typeof(DateTime)
                            && m.Name.ToLowerInvariant().StartsWith("get")
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