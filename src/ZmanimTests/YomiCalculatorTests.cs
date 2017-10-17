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
        public string Can_DafYomi_Returns_The_Correct_Daf(int year, int month, int day)
        {
            var yomi = YomiCalculator.GetDafYomiBavli(new DateTime(year, month, day));

            return $"{yomi.MasechtaTransliterated}:{yomi.Page}";
        }
    }
}
