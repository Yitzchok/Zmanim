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
#if ikvm
        [Test]
        public void TestTimeZone()
        {
            var javaTimeZone = new JavaTimeZone("America/New_York");
            var olsonTimeZone = new OlsonTimeZone("America/New_York");
            //var pdTimeZone = TzTimeZone.GetTimeZone("America/New_York");

            var timeFromEpoch = DateTime.Now - new DateTime(1970, 1, 1);
            Assert.AreEqual(javaTimeZone.getDSTSavings(), olsonTimeZone.getDSTSavings());
            Assert.AreEqual(javaTimeZone.inDaylightTime(DateTime.Now), olsonTimeZone.inDaylightTime(DateTime.Now));
            Assert.AreEqual(javaTimeZone.UtcOffset(DateTime.Now), olsonTimeZone.UtcOffset(DateTime.Now));
            Assert.AreEqual(javaTimeZone.getOffset(timeFromEpoch.Milliseconds), olsonTimeZone.getOffset(timeFromEpoch.Milliseconds));
            Assert.AreEqual(javaTimeZone.getRawOffset(), olsonTimeZone.getRawOffset());
        }
#endif
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