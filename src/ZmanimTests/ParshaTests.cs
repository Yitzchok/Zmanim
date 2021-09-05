using NUnit.Framework;
using Zmanim.JewishCalendar;
using System;

namespace ZmanimTests
{
    [TestFixture]
    class ParshaTests
    {
        [TestCase(2021, 1, 9, ExpectedResult = JewishCalendar.Parsha.SHEMOS, TestName = "Non-leap year")]
        [TestCase(2019, 5, 25, ExpectedResult = JewishCalendar.Parsha.BEHAR, TestName = "Leap year")]
        [TestCase(2029, 1, 6, ExpectedResult = JewishCalendar.Parsha.SHEMOS, TestName = "Non-leap year Long Kislev")]
        public JewishCalendar.Parsha OutsideIsrael(int year, int month, int day)
        {
            var jewishCalendar = new JewishCalendar();

            return jewishCalendar.GetParshah(new DateTime(year, month, day));
        }

        [TestCase(2021, 1, 9, ExpectedResult = JewishCalendar.Parsha.SHEMOS, TestName = "Non-leap year")]
        [TestCase(2019, 5, 25, ExpectedResult = JewishCalendar.Parsha.BECHUKOSAI, TestName = "Leap year")]
        public JewishCalendar.Parsha Israel(int year, int month, int day)
        {
            var jewishCalendar = new JewishCalendar();

            return jewishCalendar.GetParshah(new DateTime(year, month, day), true);
        }
    }
}
