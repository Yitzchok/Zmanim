using System;
using java.util;
using net.sourceforge.zmanim;
using net.sourceforge.zmanim.util;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Zmanim.Extensions;
using TimeZone = java.util.TimeZone;

namespace ZmanimTests
{
    [TestFixture]
    public class ZemaningTests
    {
        //We can use these test when removing the depenency to Java (IKVM)
        //To make sure that the code stayes the same.

        private ComplexZmanimCalendar calendar;

        [SetUp]
        public void Setup()
        {
            string locationName = "Brooklyn, NY";
            double latitude = 40.618851; //Brooklyn, NY
            double longitude = -73.985921; //Brooklyn, NY
            double elevation = 0; //optional elevation
            TimeZone timeZone = TimeZone.getTimeZone("America/New_York");
            var location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            var czc = new ComplexZmanimCalendar(location);
            czc.setCalendar(new GregorianCalendar(2010, 3, 2));
            calendar = czc;
        }

        [Test]
        public void Check_getAlos120()
        {
            var zman = calendar.getAlos120().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 38, 24)
                ));
        }

        [Test]
        public void Check_getAlos120Zmanis()
        {
            var zman = calendar.getAlos120Zmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 31, 13)
                ));
        }

        [Test]
        public void Check_getAlos16Point1Degrees()
        {
            var zman = calendar.getAlos16Point1Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 15, 13)
                ));
        }

        [Test]
        public void Check_getAlos18Degrees()
        {
            var zman = calendar.getAlos18Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 4, 24)
                ));
        }

        [Test]
        public void Check_getAlos19Point8Degrees()
        {
            var zman = calendar.getAlos19Point8Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 53, 59)
                ));
        }

        [Test]
        public void Check_getAlos26Degrees()
        {
            var zman = calendar.getAlos26Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 16, 34)
                ));
        }

        [Test]
        public void Check_getAlos60()
        {
            var zman = calendar.getAlos60().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 38, 24)
                ));
        }

        [Test]
        public void Check_getAlos72Zmanis()
        {
            var zman = calendar.getAlos72Zmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 22, 6)
                ));
        }

        [Test]
        public void Check_getAlos90()
        {
            var zman = calendar.getAlos90().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 8, 24)
                ));
        }

        [Test]
        public void Check_getAlos90Zmanis()
        {
            var zman = calendar.getAlos90Zmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 3, 1)
                ));
        }

        [Test]
        public void Check_getAlos96()
        {
            var zman = calendar.getAlos96().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 2, 24)
                ));
        }

        [Test]
        public void Check_getAlos96Zmanis()
        {
            var zman = calendar.getAlos96Zmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 56, 40)
                ));
        }

        [Test]
        public void Check_getBainHasmashosRT13Degrees()
        {
            var zman = calendar.getBainHasmashosRT13Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 27, 21)
                ));
        }

        [Test]
        public void Check_getBainHasmashosRT13Point5MinutesBefore7Point083Degrees()
        {
            var zman = calendar.getBainHasmashosRT13Point5MinutesBefore7Point083Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 40, 59)
                ));
        }

        [Test]
        public void Check_getBainHasmashosRT2Stars()
        {
            var zman = calendar.getBainHasmashosRT2Stars().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 50, 28)
                ));
        }

        [Test]
        public void Check_getBainHasmashosRT58Point5Minutes()
        {
            var zman = calendar.getBainHasmashosRT58Point5Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 19, 58)
                ));
        }

        [Test]
        public void Check_getFixedLocalChatzos()
        {
            var zman = calendar.getFixedLocalChatzos().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 12, 55, 56)
                ));
        }

        [Test]
        public void Check_getMinchaGedola16Point1Degrees()
        {
            var zman = calendar.getMinchaGedola16Point1Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 38, 43)
                ));
        }

        [Test]
        public void Check_getMinchaGedola30Minutes()
        {
            var zman = calendar.getMinchaGedola30Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 29, 56)
                ));
        }

        [Test]
        public void Check_getMinchaGedola72Minutes()
        {
            var zman = calendar.getMinchaGedola72Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 37, 44)
                ));
        }

        [Test]
        public void Check_getMinchaGedolaAteretTorah()
        {
            var zman = calendar.getMinchaGedolaAteretTorah().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 18, 25)
                ));
        }

        [Test]
        public void Check_getMinchaGedolaGreaterThan30()
        {
            var zman = calendar.getMinchaGedolaGreaterThan30().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 31, 44)
                ));
        }

        [Test]
        public void Check_getMinchaKetana16Point1Degrees()
        {
            var zman = calendar.getMinchaKetana16Point1Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 17, 31, 5)
                ));
        }

        [Test]
        public void Check_getMinchaKetana72Minutes()
        {
            var zman = calendar.getMinchaKetana72Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 17, 24, 30)
                ));
        }

        [Test]
        public void Check_getMinchaKetanaAteretTorah()
        {
            var zman = calendar.getMinchaKetanaAteretTorah().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 16, 58, 16)
                ));
        }

        [Test]
        public void Check_getMisheyakir10Point2Degrees()
        {
            var zman = calendar.getMisheyakir10Point2Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 48, 1)
                ));
        }

        [Test]
        public void Check_getMisheyakir11Degrees()
        {
            var zman = calendar.getMisheyakir11Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 43, 38)
                ));
        }

        [Test]
        public void Check_getMisheyakir11Point5Degrees()
        {
            var zman = calendar.getMisheyakir11Point5Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 40, 53)
                ));
        }

        [Test]
        public void Check_getPlagAlos16Point1ToTzaisGeonim7Point083Degrees()
        {
            var zman = calendar.getPlagAlos16Point1ToTzaisGeonim7Point083Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 22, 54)
                ));
        }

        [Test]
        public void Check_getPlagAlosToSunset()
        {
            var zman = calendar.getPlagAlosToSunset().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 17, 53, 19)
                ));
        }

        [Test]
        public void Check_getPlagHamincha120Minutes()
        {
            var zman = calendar.getPlagHamincha120Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 36, 59)
                ));
        }

        [Test]
        public void Check_getPlagHamincha120MinutesZmanis()
        {
            var zman = calendar.getPlagHamincha120MinutesZmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 42, 40)
                ));
        }

        [Test]
        public void Check_getPlagHamincha16Point1Degrees()
        {
            var zman = calendar.getPlagHamincha16Point1Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 7, 55)
                ));
        }

        [Test]
        public void Check_getPlagHamincha18Degrees()
        {
            var zman = calendar.getPlagHamincha18Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 16, 30)
                ));
        }

        [Test]
        public void Check_getPlagHamincha19Point8Degrees()
        {
            var zman = calendar.getPlagHamincha19Point8Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 24, 46)
                ));
        }

        [Test]
        public void Check_getPlagHamincha26Degrees()
        {
            var zman = calendar.getPlagHamincha26Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 54, 30)
                ));
        }

        [Test]
        public void Check_getPlagHamincha60Minutes()
        {
            var zman = calendar.getPlagHamincha60Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 49, 29)
                ));
        }

        [Test]
        public void Check_getPlagHamincha72Minutes()
        {
            var zman = calendar.getPlagHamincha72Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 58, 59)
                ));
        }

        [Test]
        public void Check_getPlagHamincha72MinutesZmanis()
        {
            var zman = calendar.getPlagHamincha72MinutesZmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 2, 23)
                ));
        }

        [Test]
        public void Check_getPlagHamincha90Minutes()
        {
            var zman = calendar.getPlagHamincha90Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 13, 14)
                ));
        }

        [Test]
        public void Check_getPlagHamincha90MinutesZmanis()
        {
            var zman = calendar.getPlagHamincha90MinutesZmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 17, 29)
                ));
        }

        [Test]
        public void Check_getPlagHamincha96Minutes()
        {
            var zman = calendar.getPlagHamincha96Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 17, 59)
                ));
        }

        [Test]
        public void Check_getPlagHamincha96MinutesZmanis()
        {
            var zman = calendar.getPlagHamincha96MinutesZmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 22, 31)
                ));
        }

        [Test]
        public void Check_getPlagHaminchaAteretTorah()
        {
            var zman = calendar.getPlagHaminchaAteretTorah().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 29, 52)
                ));
        }

        [Test]
        public void Check_getSofZmanShma3HoursBeforeChatzos()
        {
            var zman = calendar.getSofZmanShma3HoursBeforeChatzos().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 59, 56)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaAlos16Point1ToSunset()
        {
            var zman = calendar.getSofZmanShmaAlos16Point1ToSunset().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 46, 47)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaAlos16Point1ToTzaisGeonim7Point083Degrees()
        {
            var zman = calendar.getSofZmanShmaAlos16Point1ToTzaisGeonim7Point083Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 55, 2)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaAteretTorah()
        {
            var zman = calendar.getSofZmanShmaAteretTorah().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 1, 56)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaFixedLocal()
        {
            var zman = calendar.getSofZmanShmaFixedLocal().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 55, 56)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaKolEliyahu()
        {
            var zman = calendar.getSofZmanShmaKolEliyahu().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 47, 10)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA120Minutes()
        {
            var zman = calendar.getSofZmanShmaMGA120Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 49, 10)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA16Point1Degrees()
        {
            var zman = calendar.getSofZmanShmaMGA16Point1Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 7, 36)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA19Point8Degrees()
        {
            var zman = calendar.getSofZmanShmaMGA19Point8Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 57, 0)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA72Minutes()
        {
            var zman = calendar.getSofZmanShmaMGA72Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 13, 10)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA72MinutesZmanis()
        {
            var zman = calendar.getSofZmanShmaMGA72MinutesZmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 11, 1)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA90Minutes()
        {
            var zman = calendar.getSofZmanShmaMGA90Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 4, 10)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA90MinutesZmanis()
        {
            var zman = calendar.getSofZmanShmaMGA90MinutesZmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 1, 29)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA96Minutes()
        {
            var zman = calendar.getSofZmanShmaMGA96Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 1, 10)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA96MinutesZmanis()
        {
            var zman = calendar.getSofZmanShmaMGA96MinutesZmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 58, 18)
                ));
        }

        [Test]
        public void Check_getSofZmanTfila2HoursBeforeChatzos()
        {
            var zman = calendar.getSofZmanTfila2HoursBeforeChatzos().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 59, 56)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaFixedLocal()
        {
            var zman = calendar.getSofZmanTfilaFixedLocal().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 55, 56)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilahAteretTorah()
        {
            var zman = calendar.getSofZmanTfilahAteretTorah().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 15, 13)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA120Minutes()
        {
            var zman = calendar.getSofZmanTfilaMGA120Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 12, 45)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA16Point1Degrees()
        {
            var zman = calendar.getSofZmanTfilaMGA16Point1Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 25, 4)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA19Point8Degrees()
        {
            var zman = calendar.getSofZmanTfilaMGA19Point8Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 18, 0)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA72Minutes()
        {
            var zman = calendar.getSofZmanTfilaMGA72Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 28, 45)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA72MinutesZmanis()
        {
            var zman = calendar.getSofZmanTfilaMGA72MinutesZmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 27, 19)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA90Minutes()
        {
            var zman = calendar.getSofZmanTfilaMGA90Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 22, 45)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA90MinutesZmanis()
        {
            var zman = calendar.getSofZmanTfilaMGA90MinutesZmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 20, 58)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA96Minutes()
        {
            var zman = calendar.getSofZmanTfilaMGA96Minutes().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 20, 45)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA96MinutesZmanis()
        {
            var zman = calendar.getSofZmanTfilaMGA96MinutesZmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 18, 50)
                ));
        }

        [Test]
        public void Check_getTzais120()
        {
            var zman = calendar.getTzais120().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 21, 28)
                ));
        }

        [Test]
        public void Check_getTzais120Zmanis()
        {
            var zman = calendar.getTzais120Zmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 28, 38)
                ));
        }

        [Test]
        public void Check_getTzais16Point1Degrees()
        {
            var zman = calendar.getTzais16Point1Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 44, 44)
                ));
        }

        [Test]
        public void Check_getTzais18Degrees()
        {
            var zman = calendar.getTzais18Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 55, 35)
                ));
        }

        [Test]
        public void Check_getTzais19Point8Degrees()
        {
            var zman = calendar.getTzais19Point8Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 6, 2)
                ));
        }

        [Test]
        public void Check_getTzais26Degrees()
        {
            var zman = calendar.getTzais26Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 43, 33)
                ));
        }

        [Test]
        public void Check_getTzais60()
        {
            var zman = calendar.getTzais60().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 21, 28)
                ));
        }

        [Test]
        public void Check_getTzais72Zmanis()
        {
            var zman = calendar.getTzais72Zmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 37, 46)
                ));
        }

        [Test]
        public void Check_getTzais90()
        {
            var zman = calendar.getTzais90().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 51, 28)
                ));
        }

        [Test]
        public void Check_getTzais90Zmanis()
        {
            var zman = calendar.getTzais90Zmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 56, 51)
                ));
        }

        [Test]
        public void Check_getTzais96()
        {
            var zman = calendar.getTzais96().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 57, 28)
                ));
        }

        [Test]
        public void Check_getTzais96Zmanis()
        {
            var zman = calendar.getTzais96Zmanis().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 3, 12)
                ));
        }

        [Test]
        public void Check_getTzaisAteretTorah()
        {
            var zman = calendar.getTzaisAteretTorah().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 1, 28)
                ));
        }

        [Test]
        public void Check_getTzaisGeonim5Point95Degrees()
        {
            var zman = calendar.getTzaisGeonim5Point95Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 48, 49)
                ));
        }

        [Test]
        public void Check_getTzaisGeonim7Point083Degrees()
        {
            var zman = calendar.getTzaisGeonim7Point083Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 54, 29)
                ));
        }

        [Test]
        public void Check_getTzaisGeonim8Point5Degrees()
        {
            var zman = calendar.getTzaisGeonim8Point5Degrees().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 2, 37)
                ));
        }

        [Test]
        public void Check_getAlos72()
        {
            var zman = calendar.getAlos72().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 26, 24)
                ));
        }

        [Test]
        public void Check_getAlosHashachar()
        {
            var zman = calendar.getAlosHashachar().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 15, 13)
                ));
        }

        [Test]
        public void Check_getCandelLighting()
        {
            var zman = calendar.getCandelLighting().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 3, 28)
                ));
        }

        [Test]
        public void Check_getChatzos()
        {
            var zman = calendar.getChatzos().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 12, 59, 56)
                ));
        }

        [Test]
        public void Check_getMinchaGedola()
        {
            var zman = calendar.getMinchaGedola().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 31, 44)
                ));
        }

        [Test]
        public void Check_getMinchaKetana()
        {
            var zman = calendar.getMinchaKetana().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 16, 42, 30)
                ));
        }

        [Test]
        public void Check_getPlagHamincha()
        {
            var zman = calendar.getPlagHamincha().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 1, 59)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaGRA()
        {
            var zman = calendar.getSofZmanShmaGRA().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 49, 10)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA()
        {
            var zman = calendar.getSofZmanShmaMGA().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 13, 10)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaGRA()
        {
            var zman = calendar.getSofZmanTfilaGRA().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 52, 45)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA()
        {
            var zman = calendar.getSofZmanTfilaMGA().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 28, 45)
                ));
        }

        [Test]
        public void Check_getSolarMidnight()
        {
            var zman = calendar.getSolarMidnight().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 3, 5, 4, 46, 42)
                ));
        }

        [Test]
        public void Check_getTzais()
        {
            var zman = calendar.getTzais().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 2, 37)
                ));
        }

        [Test]
        public void Check_getTzais72()
        {
            var zman = calendar.getTzais72().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 33, 28)
                ));
        }

        [Test]
        public void Check_getBeginAstronomicalTwilight()
        {
            var zman = calendar.getBeginAstronomicalTwilight().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 4, 24)
                ));
        }

        [Test]
        public void Check_getBeginCivilTwilight()
        {
            var zman = calendar.getBeginCivilTwilight().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 6, 10, 48)
                ));
        }

        [Test]
        public void Check_getBeginNauticalTwilight()
        {
            var zman = calendar.getBeginNauticalTwilight().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 38, 7)
                ));
        }

        [Test]
        public void Check_getEndAstronomicalTwilight()
        {
            var zman = calendar.getEndAstronomicalTwilight().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 55, 35)
                ));
        }

        [Test]
        public void Check_getEndCivilTwilight()
        {
            var zman = calendar.getEndCivilTwilight().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 49, 6)
                ));
        }

        [Test]
        public void Check_getEndNauticalTwilight()
        {
            var zman = calendar.getEndNauticalTwilight().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 21, 48)
                ));
        }

        [Test]
        public void Check_getSeaLevelSunrise()
        {
            var zman = calendar.getSeaLevelSunrise().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 6, 38, 24)
                ));
        }

        [Test]
        public void Check_getSeaLevelSunset()
        {
            var zman = calendar.getSeaLevelSunset().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 21, 28)
                ));
        }

        [Test]
        public void Check_getSunrise()
        {
            var zman = calendar.getSunrise().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 6, 38, 24)
                ));
        }

        [Test]
        public void Check_getSunset()
        {
            var zman = calendar.getSunset().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 21, 28)
                ));
        }

        [Test]
        public void Check_getSunTransit()
        {
            var zman = calendar.getSunTransit().ToDateTime();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 12, 59, 56)
                ));
        }

    }
}