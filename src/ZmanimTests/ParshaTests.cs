using NUnit.Framework;
using Zmanim.JewishCalendar;
using System;

namespace ZmanimTests
{
    [TestFixture]
    class ParshaTests
    {
        [Test]
        public void Test_Parsha_Outside_Israel_Non_Leap_Year()
        {
            var jewishCalendar = new JewishCalendar();

            Assert.AreEqual(JewishCalendar.Parsha.SHEMOS, jewishCalendar.GetParshah(new DateTime(2021,1,9)));
        }

        [Test]
        public void Test_Parsha_Outside_Israel_Leap_Year()
        {
            var jewishCalendar = new JewishCalendar();

            Assert.AreEqual(JewishCalendar.Parsha.BEHAR, jewishCalendar.GetParshah(new DateTime(2019, 5, 25)));
        }

        [Test]
        public void Test_Parsha_In_Israel_Non_Leap_Year()
        {
            var jewishCalendar = new JewishCalendar();

            Assert.AreEqual(JewishCalendar.Parsha.SHEMOS, jewishCalendar.GetParshah(new DateTime(2021, 1, 9), true));
        }

        [Test]
        public void Test_Parsha_In_Israel_Leap_Year()
        {
            var jewishCalendar = new JewishCalendar();

            Assert.AreEqual(JewishCalendar.Parsha.BECHUKOSAI, jewishCalendar.GetParshah(new DateTime(2019, 5, 25), true));
        }
    }
}
