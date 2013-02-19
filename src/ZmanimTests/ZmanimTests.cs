using System;
using NUnit.Framework;
using Zmanim;
using Zmanim.TimeZone;
using Zmanim.Utilities;
using Zmanim.TzDatebase;

namespace ZmanimTests
{
    [TestFixture]
    public class ZmanimTest
    {
        //We can use these test when removing the depenency to Java (IKVM)
        //To make sure that the code stayes the same.
        private ComplexZmanimCalendar calendar;

        [SetUp]
        public void Setup()
        {
            string locationName = "Lakewood, NJ";
            double latitude = 40.09596; //Lakewood, NJ
            double longitude = -74.22213; //Lakewood, NJ
            double elevation = 0; //optional elevation
            ITimeZone timeZone = new OlsonTimeZone("America/New_York");
            GeoLocation location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            ComplexZmanimCalendar czc = new ComplexZmanimCalendar(new DateTime(2010, 4, 2), location);
            calendar = czc;
        }


        [Test]
        public void Check_getPlagHamincha120MinutesZmanis()
        {
            var zman = calendar.GetPlagHamincha120MinutesZmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 43, 14)
                ));
        }

        [Test]
        public void Check_getPlagHamincha120Minutes()
        {
            var zman = calendar.GetPlagHamincha120Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 37, 38)
                ));
        }

        [Test]
        public void Check_getAlos60()
        {
            var zman = calendar.GetAlos60().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 39, 41)
                ));
        }

        [Test]
        public void Check_getAlos72Zmanis()
        {
            var zman = calendar.GetAlos72Zmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 23, 27)
                ));
        }

        [Test]
        public void Check_getAlos96()
        {
            var zman = calendar.GetAlos96().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 3, 41)
                ));
        }

        [Test]
        public void Check_getAlos90Zmanis()
        {
            var zman = calendar.GetAlos90Zmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 4, 24)
                ));
        }

        [Test]
        public void Check_getAlos96Zmanis()
        {
            var zman = calendar.GetAlos96Zmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 58, 2)
                ));
        }

        [Test]
        public void Check_getAlos90()
        {
            var zman = calendar.GetAlos90().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 9, 41)
                ));
        }

        [Test]
        public void Check_getAlos120()
        {
            var dateTime = calendar.GetAlos120();
            var zman = dateTime.RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 39, 41)
                ));
        }

        [Test]
        public void Check_getAlos120Zmanis()
        {
            var zman = calendar.GetAlos120Zmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 32, 38)
                ));
        }

        [Test]
        public void Check_getAlos26Degrees()
        {
            var zman = calendar.GetAlos26Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 19, 16)
                ));
        }

        [Test]
        public void Check_getAlos18Degrees()
        {
            var zman = calendar.GetAlos18Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 6, 31)
                ));
        }

        [Test]
        public void Check_getAlos19Point8Degrees()
        {
            var zman = calendar.GetAlos19Point8Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 56, 13)
                ));
        }

        [Test]
        public void Check_getAlos16Point1Degrees()
        {
            var zman = calendar.GetAlos16Point1Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 17, 14)
                ));
        }

        [Test]
        public void Check_getMisheyakir11Point5Degrees()
        {
            var zman = calendar.GetMisheyakir11Point5Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 42, 39)
                ));
        }

        [Test]
        public void Check_getMisheyakir11Degrees()
        {
            var zman = calendar.GetMisheyakir11Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 45, 22)
                ));
        }

        [Test]
        public void Check_getMisheyakir10Point2Degrees()
        {
            var zman = calendar.GetMisheyakir10Point2Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 49, 43)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA19Point8Degrees()
        {
            var zman = calendar.GetSofZmanShmaMGA19Point8Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 58, 35)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA16Point1Degrees()
        {
            var zman = calendar.GetSofZmanShmaMGA16Point1Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 9, 5)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA72Minutes()
        {
            var zman = calendar.GetSofZmanShmaMGA72Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 14, 17)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA72MinutesZmanis()
        {
            var zman = calendar.GetSofZmanShmaMGA72MinutesZmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 12, 10)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA90Minutes()
        {
            var zman = calendar.GetSofZmanShmaMGA90Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 5, 17)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA90MinutesZmanis()
        {
            var zman = calendar.GetSofZmanShmaMGA90MinutesZmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 2, 38)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA96Minutes()
        {
            var zman = calendar.GetSofZmanShmaMGA96Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 2, 17)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA96MinutesZmanis()
        {
            var zman = calendar.GetSofZmanShmaMGA96MinutesZmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 59, 27)
                ));
        }

        [Test]
        public void Check_getSofZmanShma3HoursBeforeChatzos()
        {
            var zman = calendar.GetSofZmanShma3HoursBeforeChatzos().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 0, 52)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA120Minutes()
        {
            var zman = calendar.GetSofZmanShmaMGA120Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 50, 17)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaAlos16Point1ToSunset()
        {
            var zman = calendar.GetSofZmanShmaAlos16Point1ToSunset().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 48, 26)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaAlos16Point1ToTzaisGeonim7Point083Degrees()
        {
            var zman = calendar.GetSofZmanShmaAlos16Point1ToTzaisGeonim7Point083Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 56, 38)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaKolEliyahu()
        {
            var zman = calendar.GetSofZmanShmaKolEliyahu().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 48, 17)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA19Point8Degrees()
        {
            var zman = calendar.GetSofZmanTfilaMGA19Point8Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 19, 22)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA16Point1Degrees()
        {
            var zman = calendar.GetSofZmanTfilaMGA16Point1Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 26, 21)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA72Minutes()
        {
            var zman = calendar.GetSofZmanTfilaMGA72Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 29, 49)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA72MinutesZmanis()
        {
            var zman = calendar.GetSofZmanTfilaMGA72MinutesZmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 28, 24)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA90Minutes()
        {
            var zman = calendar.GetSofZmanTfilaMGA90Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 23, 49)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA90MinutesZmanis()
        {
            var zman = calendar.GetSofZmanTfilaMGA90MinutesZmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 22, 3)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA96Minutes()
        {
            var zman = calendar.GetSofZmanTfilaMGA96Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 21, 49)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA96MinutesZmanis()
        {
            var zman = calendar.GetSofZmanTfilaMGA96MinutesZmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 19, 56)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA120Minutes()
        {
            var zman = calendar.GetSofZmanTfilaMGA120Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 13, 49)
                ));
        }

        [Test]
        public void Check_getSofZmanTfila2HoursBeforeChatzos()
        {
            var zman = calendar.GetSofZmanTfila2HoursBeforeChatzos().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 11, 0, 52)
                ));
        }

        [Test]
        public void Check_getMinchaGedola30Minutes()
        {
            var zman = calendar.GetMinchaGedola30Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 30, 52)
                ));
        }

        [Test]
        public void Check_getMinchaGedola72Minutes()
        {
            var zman = calendar.GetMinchaGedola72Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 38, 38)
                ));
        }

        [Test]
        public void Check_getMinchaGedola16Point1Degrees()
        {
            var zman = calendar.GetMinchaGedola16Point1Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 39, 34)
                ));
        }

        [Test]
        public void Check_getMinchaGedolaGreaterThan30()
        {
            var zman = calendar.GetMinchaGedolaGreaterThan30().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 32, 38)
                ));
        }

        [Test]
        public void Check_getMinchaKetana16Point1Degrees()
        {
            var zman = calendar.GetMinchaKetana16Point1Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 17, 31, 24)
                ));
        }

        [Test]
        public void Check_getMinchaKetana72Minutes()
        {
            var zman = calendar.GetMinchaKetana72Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 17, 25, 14)
                ));
        }

        [Test]
        public void Check_getPlagHamincha60Minutes()
        {
            var zman = calendar.GetPlagHamincha60Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 50, 8)
                ));
        }

        [Test]
        public void Check_getPlagHamincha72Minutes()
        {
            var zman = calendar.GetPlagHamincha72Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 59, 38)
                ));
        }

        [Test]
        public void Check_getPlagHamincha90Minutes()
        {
            var zman = calendar.GetPlagHamincha90Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 13, 53)
                ));
        }

        [Test]
        public void Check_getPlagHamincha96Minutes()
        {
            var zman = calendar.GetPlagHamincha96Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 18, 38)
                ));
        }

        [Test]
        public void Check_getPlagHamincha96MinutesZmanis()
        {
            var zman = calendar.GetPlagHamincha96MinutesZmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 23, 7)
                ));
        }

        [Test]
        public void Check_getPlagHamincha90MinutesZmanis()
        {
            var zman = calendar.GetPlagHamincha90MinutesZmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 18, 5)
                ));
        }

        [Test]
        public void Check_getPlagHamincha72MinutesZmanis()
        {
            var zman = calendar.GetPlagHamincha72MinutesZmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 3, 0)
                ));
        }

        [Test]
        public void Check_getPlagHamincha16Point1Degrees()
        {
            var zman = calendar.GetPlagHamincha16Point1Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 8, 0)
                ));
        }

        [Test]
        public void Check_getPlagHamincha19Point8Degrees()
        {
            var zman = calendar.GetPlagHamincha19Point8Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 24, 41)
                ));
        }

        [Test]
        public void Check_getPlagHamincha26Degrees()
        {
            var zman = calendar.GetPlagHamincha26Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 54, 2)
                ));
        }

        [Test]
        public void Check_getPlagHamincha18Degrees()
        {
            var zman = calendar.GetPlagHamincha18Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 16, 30)
                ));
        }

        [Test]
        public void Check_getPlagAlosToSunset()
        {
            var zman = calendar.GetPlagAlosToSunset().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 17, 54, 3)
                ));
        }

        [Test]
        public void Check_getPlagAlos16Point1ToTzaisGeonim7Point083Degrees()
        {
            var zman = calendar.GetPlagAlos16Point1ToTzaisGeonim7Point083Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 23, 24)
                ));
        }

        [Test]
        public void Check_getBainHasmashosRT13Point24Degrees()
        {
            var zman = calendar.GetBainHasmashosRT13Point24Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 27, 23)
                ));
        }

        [Test]
        public void Check_getBainHasmashosRT58Point5Minutes()
        {
            var zman = calendar.GetBainHasmashosRT58Point5Minutes().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 20, 33)
                ));
        }

        [Test]
        public void Check_getBainHasmashosRT13Point5MinutesBefore7Point083Degrees()
        {
            var zman = calendar.GetBainHasmashosRT13Point5MinutesBefore7Point083Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 41, 19)
                ));
        }

        [Test]
        public void Check_getBainHasmashosRT2Stars()
        {
            var zman = calendar.GetBainHasmashosRT2Stars().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 50, 48)
                ));
        }

        [Test]
        public void Check_getTzaisGeonim5Point95Degrees()
        {
            var zman = calendar.GetTzaisGeonim5Point95Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 49, 12)
                ));
        }

        [Test]
        public void Check_getTzaisGeonim3Point65Degrees()
        {
            var zman = calendar.GetTzaisGeonim3Point65Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 36, 57)
                ));
        }

        [Test]
        public void Check_getTzaisGeonim4Point61Degrees()
        {
            var zman = calendar.GetTzaisGeonim4Point61Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 42, 3)
                ));
        }

        [Test]
        public void Check_getTzaisGeonim4Point37Degrees()
        {
            var zman = calendar.GetTzaisGeonim4Point37Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 40, 46)
                ));
        }

        [Test]
        public void Check_getTzaisGeonim5Point88Degrees()
        {
            var zman = calendar.GetTzaisGeonim5Point88Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 48, 49)
                ));
        }

        [Test]
        public void Check_getTzaisGeonim4Point8Degrees()
        {
            var zman = calendar.GetTzaisGeonim4Point8Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 43, 4)
                ));
        }

        [Test]
        public void Check_getTzaisGeonim7Point083Degrees()
        {
            var zman = calendar.GetTzaisGeonim7Point083Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 54, 49)
                ));
        }

        [Test]
        public void Check_getTzaisGeonim8Point5Degrees()
        {
            var zman = calendar.GetTzaisGeonim8Point5Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 2, 52)
                ));
        }

        [Test]
        public void Check_getTzais60()
        {
            var zman = calendar.GetTzais60().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 22, 3)
                ));
        }

        [Test]
        public void Check_getTzaisAteretTorah()
        {
            var zman = calendar.GetTzaisAteretTorah().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 2, 3)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaAteretTorah()
        {
            var zman = calendar.GetSofZmanShmaAteretTorah().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 3, 6)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilahAteretTorah()
        {
            var zman = calendar.GetSofZmanTfilahAteretTorah().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 16, 19)
                ));
        }

        [Test]
        public void Check_getMinchaGedolaAteretTorah()
        {
            var zman = calendar.GetMinchaGedolaAteretTorah().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 19, 22)
                ));
        }

        [Test]
        public void Check_getMinchaKetanaAteretTorah()
        {
            var zman = calendar.GetMinchaKetanaAteretTorah().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 16, 59, 1)
                ));
        }

        [Test]
        public void Check_getPlagHaminchaAteretTorah()
        {
            var zman = calendar.GetPlagHaminchaAteretTorah().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 30, 32)
                ));
        }

        [Test]
        public void Check_getTzais72Zmanis()
        {
            var zman = calendar.GetTzais72Zmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 38, 17)
                ));
        }

        [Test]
        public void Check_getTzais90Zmanis()
        {
            var zman = calendar.GetTzais90Zmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 57, 21)
                ));
        }

        [Test]
        public void Check_getTzais96Zmanis()
        {
            var zman = calendar.GetTzais96Zmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 3, 42)
                ));
        }

        [Test]
        public void Check_getTzais90()
        {
            var zman = calendar.GetTzais90().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 52, 3)
                ));
        }

        [Test]
        public void Check_getTzais120()
        {
            var zman = calendar.GetTzais120().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 22, 3)
                ));
        }

        [Test]
        public void Check_getTzais120Zmanis()
        {
            var zman = calendar.GetTzais120Zmanis().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 29, 7)
                ));
        }

        [Test]
        public void Check_getTzais16Point1Degrees()
        {
            var zman = calendar.GetTzais16Point1Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 44, 36)
                ));
        }

        [Test]
        public void Check_getTzais26Degrees()
        {
            var zman = calendar.GetTzais26Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 42, 43)
                ));
        }

        [Test]
        public void Check_getTzais18Degrees()
        {
            var zman = calendar.GetTzais18Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 55, 21)
                ));
        }

        [Test]
        public void Check_getTzais19Point8Degrees()
        {
            var zman = calendar.GetTzais19Point8Degrees().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 5, 40)
                ));
        }

        [Test]
        public void Check_getTzais96()
        {
            var zman = calendar.GetTzais96().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 58, 3)
                ));
        }

        [Test]
        public void Check_getFixedLocalChatzos()
        {
            var zman = calendar.GetFixedLocalChatzos().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 12, 56, 53)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaFixedLocal()
        {
            var zman = calendar.GetSofZmanShmaFixedLocal().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 56, 53)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaFixedLocal()
        {
            var zman = calendar.GetSofZmanTfilaFixedLocal().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 56, 53)
                ));
        }

        [Test]
        public void Check_getTzais()
        {
            var zman = calendar.GetTzais().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 2, 52)
                ));
        }

        [Test]
        public void Check_getAlosHashachar()
        {
            var zman = calendar.GetAlosHashachar().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 17, 14)
                ));
        }

        [Test]
        public void Check_getAlos72()
        {
            var zman = calendar.GetAlos72().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 27, 41)
                ));
        }

        [Test]
        public void Check_getChatzos()
        {
            var zman = calendar.GetChatzos().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 0, 52)
                ));
        }

        [Test]
        public void Check_getSolarMidnight()
        {
            var zman = calendar.GetSolarMidnight().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 3, 1, 0, 4)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaGRA()
        {
            var zman = calendar.GetSofZmanShmaGRA().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 50, 17)
                ));
        }

        [Test]
        public void Check_getSofZmanShmaMGA()
        {
            var zman = calendar.GetSofZmanShmaMGA().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 14, 17)
                ));
        }

        [Test]
        public void Check_getTzais72()
        {
            var zman = calendar.GetTzais72().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 34, 3)
                ));
        }

        [Test]
        public void Check_getCandelLighting()
        {
            var zman = calendar.GetCandelLighting().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 4, 3)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaGRA()
        {
            var zman = calendar.GetSofZmanTfilaGRA().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 53, 49)
                ));
        }

        [Test]
        public void Check_getSofZmanTfilaMGA()
        {
            var zman = calendar.GetSofZmanTfilaMGA().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 29, 49)
                ));
        }

        [Test]
        public void Check_getMinchaGedola()
        {
            var zman = calendar.GetMinchaGedola().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 32, 38)
                ));
        }

        [Test]
        public void Check_getMinchaKetana()
        {
            var zman = calendar.GetMinchaKetana().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 16, 43, 14)
                ));
        }

        [Test]
        public void Check_getPlagHamincha()
        {
            var zman = calendar.GetPlagHamincha().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 2, 38)
                ));
        }

        [Test]
        public void Check_getSunrise()
        {
            var zman = calendar.GetSunrise().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 6, 39, 41)
                ));
        }

        [Test]
        public void Check_getSeaLevelSunrise()
        {
            var zman = calendar.GetSeaLevelSunrise().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 6, 39, 41)
                ));
        }

        [Test]
        public void Check_getBeginCivilTwilight()
        {
            var zman = calendar.GetBeginCivilTwilight().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 6, 12, 18)
                ));
        }

        [Test]
        public void Check_getBeginNauticalTwilight()
        {
            var zman = calendar.GetBeginNauticalTwilight().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 39, 55)
                ));
        }

        [Test]
        public void Check_getBeginAstronomicalTwilight()
        {
            var zman = calendar.GetBeginAstronomicalTwilight().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 6, 31)
                ));
        }

        [Test]
        public void Check_getSunset()
        {
            var zman = calendar.GetSunset().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 22, 3)
                ));
        }

        [Test]
        public void Check_getSeaLevelSunset()
        {
            var zman = calendar.GetSeaLevelSunset().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 22, 3)
                ));
        }

        [Test]
        public void Check_getEndCivilTwilight()
        {
            var zman = calendar.GetEndCivilTwilight().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 49, 28)
                ));
        }

        [Test]
        public void Check_getEndNauticalTwilight()
        {
            var zman = calendar.GetEndNauticalTwilight().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 21, 53)
                ));
        }

        [Test]
        public void Check_getEndAstronomicalTwilight()
        {
            var zman = calendar.GetEndAstronomicalTwilight().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 55, 21)
                ));
        }

        [Test]
        public void Check_getSunTransit()
        {
            var zman = calendar.GetSunTransit().RemoveMilliseconds();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 0, 52)
                ));
        }

        [Test]
        public void Check_getShaahZmanis19Point8Degrees()
        {
            Assert.That(calendar.GetShaahZmanis19Point8Degrees(), Is.EqualTo(4847270));
        }

        [Test]
        public void Check_getShaahZmanis18Degrees()
        {
            Assert.That(calendar.GetShaahZmanis18Degrees(), Is.EqualTo(4744143));
        }

        [Test]
        public void Check_getShaahZmanis26Degrees()
        {
            Assert.That(calendar.GetShaahZmanis26Degrees(), Is.EqualTo(5217225));
        }

        [Test]
        public void Check_getShaahZmanis16Point1Degrees()
        {
            Assert.That(calendar.GetShaahZmanis16Point1Degrees(), Is.EqualTo(4636860));
        }

        [Test]
        public void Check_getShaahZmanis60Minutes()
        {
            Assert.That(calendar.GetShaahZmanis60Minutes(), Is.EqualTo(4411823));
        }

        [Test]
        public void Check_getShaahZmanis72Minutes()
        {
            Assert.That(calendar.GetShaahZmanis72Minutes(), Is.EqualTo(4531823));
        }

        [Test]
        public void Check_getShaahZmanis72MinutesZmanis()
        {
            Assert.That(calendar.GetShaahZmanis72MinutesZmanis(), Is.EqualTo(4574187));
        }

        [Test]
        public void Check_getShaahZmanis90Minutes()
        {
            Assert.That(calendar.GetShaahZmanis90Minutes(), Is.EqualTo(4711823));
        }

        [Test]
        public void Check_getShaahZmanis90MinutesZmanis()
        {
            Assert.That(calendar.GetShaahZmanis90MinutesZmanis(), Is.EqualTo(4764778));
        }

        [Test]
        public void Check_getShaahZmanis96MinutesZmanis()
        {
            Assert.That(calendar.GetShaahZmanis96MinutesZmanis(), Is.EqualTo(4828309));
        }

        [Test]
        public void Check_getShaahZmanisAteretTorah()
        {
            Assert.That(calendar.GetShaahZmanisAteretTorah(), Is.EqualTo(4393005));
        }

        [Test]
        public void Check_getShaahZmanis96Minutes()
        {
            Assert.That(calendar.GetShaahZmanis96Minutes(), Is.EqualTo(4771823));
        }

        [Test]
        public void Check_getShaahZmanis120Minutes()
        {
            Assert.That(calendar.GetShaahZmanis120Minutes(), Is.EqualTo(5011823));
        }

        [Test]
        public void Check_getShaahZmanis120MinutesZmanis()
        {
            Assert.That(calendar.GetShaahZmanis120MinutesZmanis(), Is.EqualTo(5082430));
        }

        [Test]
        public void Check_getShaahZmanisGra()
        {
            Assert.That(calendar.GetShaahZmanisGra(), Is.EqualTo(3811823));
        }

        [Test]
        public void Check_getShaahZmanisMGA()
        {
            Assert.That(calendar.GetShaahZmanisMGA(), Is.EqualTo(4531823));
        }

        [Test]
        public void Check_getTemporalHour()
        {
            Assert.That(calendar.GetTemporalHour(), Is.EqualTo(3811823));
        }

    }
}