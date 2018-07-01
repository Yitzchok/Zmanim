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
        [TestCase(2017, 6, 30, ExpectedResult = "Bava Basra:159")]
        [TestCase(2017, 7, 1, ExpectedResult = "Bava Basra:160")]
        [TestCase(2017, 9, 30, ExpectedResult = "Sanhedrin:76")]
        [TestCase(2017, 10, 1, ExpectedResult = "Sanhedrin:77")]
        [TestCase(2017, 10, 17, ExpectedResult = "Sanhedrin:93")]
        [TestCase(2018, 5, 29, ExpectedResult = "Zevachim:46")]
        [TestCase(2019, 9, 19, ExpectedResult = "Meilah:2")]
        [TestCase(2019, 10, 9, ExpectedResult = "Meilah Kinnim:22")]
        [TestCase(2027, 3, 12, ExpectedResult = "Meilah Kinnim:22")]
        [TestCase(2190, 6, 27, ExpectedResult = "Meilah Kinnim:22")]
        [TestCase(2019, 10, 10, ExpectedResult = "Kinnim:23")]
        [TestCase(2019, 10, 12, ExpectedResult = "Kinnim Tamid:25")]
        [TestCase(2027, 3, 15, ExpectedResult = "Kinnim Tamid:25")]
        [TestCase(2019, 10, 13, ExpectedResult = "Tamid:26")]
        [TestCase(2019, 10, 20, ExpectedResult = "Tamid:33")]
        [TestCase(2019, 10, 21, ExpectedResult = "Midos:34")]
        [TestCase(2019, 10, 25, ExpectedResult = "Niddah:2")]
        public string Can_DafYomi_Returns_The_Correct_Daf(int year, int month, int day)
        {
            var daf = YomiCalculator.GetDafYomiBavli(new DateTime(year, month, day));

            return $"{daf.MasechtaTransliterated}{(daf.HasSecondaryMesechta ? " " + daf.SecondaryMasechtaTransliterated: "")}:{daf.Page}";
        }
    }
}
