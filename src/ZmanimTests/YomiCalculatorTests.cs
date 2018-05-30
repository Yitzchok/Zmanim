using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Zmanim.JewishCalendar;

namespace ZmanimTests
{
    [TestFixture]
    public class YomiCalculatorTests
    {
        [TestCase(2017, 6, 30, Result = "Bava Basra:159")]
        [TestCase(2017, 7, 1, Result = "Bava Basra:160")]
        [TestCase(2017, 9, 30, Result = "Sanhedrin:76")]
        [TestCase(2017, 10, 1, Result = "Sanhedrin:77")]
        [TestCase(2017, 10, 17, Result = "Sanhedrin:93")]
        [TestCase(2018, 5, 29, Result = "Zevachim:46")]
        [TestCase(2019, 9, 19, Result = "Meilah:2")]
        [TestCase(2019, 10, 9, Result = "Meilah Kinnim:22")]
        [TestCase(2019, 10, 10, Result = "Kinnim:23")]
        [TestCase(2019, 10, 20, Result = "Tamid:33")]
        [TestCase(2019, 10, 21, Result = "Midos:34")]
        [TestCase(2019, 10, 25, Result = "Niddah:2")]
        public string Can_DafYomi_Returns_The_Correct_Daf(int year, int month, int day)
        {
            var yomi = YomiCalculator.GetDafYomiBavli(new DateTime(year, month, day));

            return $"{yomi.MasechtaTransliterated}:{yomi.Page}";
        }
    }
}
