using NUnit.Framework;
using Zmanim.Calculator;

namespace ZmanimTests
{
    [TestFixture]
    public class ZmanimCalculatorZmanimTest : ZmanimTest
    {
        protected override AstronomicalCalculator GetCalculator()
        {
            return new ZmanimCalculator();
        }
    }
}