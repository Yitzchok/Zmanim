
using System;
using NUnit.Framework;
using Zmanim;
using Zmanim.TimeZone;
using Zmanim.TzDatebase;
using Zmanim.Utilities;

namespace ZmanimTests
{
    [TestFixture]
    public class ZmanimTestWithMilliseconds
    {
        
        //We can use these test when removing the depenency to Java (IKVM)
        //To make sure that the code stayes the same.

        private ComplexZmanimCalendar calendar;

        [SetUp]
        public void Setup()
        {
            String locationName = "Lakewood, NJ";
            double latitude = 40.09596; //Lakewood, NJ
            double longitude = -74.22213; //Lakewood, NJ
            double elevation = 0; //optional elevation
            ITimeZone timeZone = new OlsonTimeZone("America/New_York");
            GeoLocation location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            ComplexZmanimCalendar czc = new ComplexZmanimCalendar(new DateTime(2010, 4, 2), location);

            calendar = czc;
        }


        [Test]
        public void Check_GetPlagHamincha120MinutesZmanis()
        {
            var zman = calendar.GetPlagHamincha120MinutesZmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 43, 14, 308)
                ));
        }

        [Test]
        public void Check_GetPlagHamincha120Minutes()
        {
            var zman = calendar.GetPlagHamincha120Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 37, 38, 929)
                ));
        }

        [Test]
        public void Check_GetAlos60()
        {
            var zman = calendar.GetAlos60();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 39, 41, 832)
                ));
        }

        [Test]
        public void Check_GetAlos72Zmanis()
        {
            var zman = calendar.GetAlos72Zmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 23, 27, 645)
                ));
        }

        [Test]
        public void Check_GetAlos96()
        {
            var zman = calendar.GetAlos96();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 3, 41, 832)
                ));
        }

        [Test]
        public void Check_GetAlos90Zmanis()
        {
            var zman = calendar.GetAlos90Zmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 4, 24, 98)
                ));
        }

        [Test]
        public void Check_GetAlos96Zmanis()
        {
            var zman = calendar.GetAlos96Zmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 58, 2, 916)
                ));
        }

        [Test]
        public void Check_GetAlos90()
        {
            var zman = calendar.GetAlos90();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 9, 41, 832)
                ));
        }

        [Test]
        public void Check_GetAlos120()
        {
            var zman = calendar.GetAlos120();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 39, 41, 832)
                ));
        }

        [Test]
        public void Check_GetAlos120Zmanis()
        {
            var zman = calendar.GetAlos120Zmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 32, 38, 186)
                ));
        }

        [Test]
        public void Check_GetAlos26Degrees()
        {
            var zman = calendar.GetAlos26Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 19, 16, 963)
                ));
        }

        [Test]
        public void Check_GetAlos18Degrees()
        {
            var zman = calendar.GetAlos18Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 6, 31, 438)
                ));
        }

        [Test]
        public void Check_GetAlos19Point8Degrees()
        {
            var zman = calendar.GetAlos19Point8Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 4, 56, 13, 377)
                ));
        }

        [Test]
        public void Check_GetAlos16Point1Degrees()
        {
            var zman = calendar.GetAlos16Point1Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 17, 14, 493)
                ));
        }

        [Test]
        public void Check_GetMisheyakir11Point5Degrees()
        {
            var zman = calendar.GetMisheyakir11Point5Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 42, 39, 307)
                ));
        }

        [Test]
        public void Check_GetMisheyakir11Degrees()
        {
            var zman = calendar.GetMisheyakir11Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 45, 22, 774)
                ));
        }

        [Test]
        public void Check_GetMisheyakir10Point2Degrees()
        {
            var zman = calendar.GetMisheyakir10Point2Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 49, 43, 529)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaMGA19Point8Degrees()
        {
            var zman = calendar.GetSofZmanShmaMGA19Point8Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 58, 35, 187)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaMGA16Point1Degrees()
        {
            var zman = calendar.GetSofZmanShmaMGA16Point1Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 9, 5, 73)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaMGA18Degrees()
        {
            var zman = calendar.GetSofZmanShmaMGA18Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 3, 43, 867)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaMGA72Minutes()
        {
            var zman = calendar.GetSofZmanShmaMGA72Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 14, 17, 301)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaMGA72MinutesZmanis()
        {
            var zman = calendar.GetSofZmanShmaMGA72MinutesZmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 12, 10, 206)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaMGA90Minutes()
        {
            var zman = calendar.GetSofZmanShmaMGA90Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 5, 17, 301)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaMGA90MinutesZmanis()
        {
            var zman = calendar.GetSofZmanShmaMGA90MinutesZmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 2, 38, 432)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaMGA96Minutes()
        {
            var zman = calendar.GetSofZmanShmaMGA96Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 2, 17, 301)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaMGA96MinutesZmanis()
        {
            var zman = calendar.GetSofZmanShmaMGA96MinutesZmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 59, 27, 843)
                ));
        }

        [Test]
        public void Check_GetSofZmanShma3HoursBeforeChatzos()
        {
            var zman = calendar.GetSofZmanShma3HoursBeforeChatzos();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 0, 52, 770)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaMGA120Minutes()
        {
            var zman = calendar.GetSofZmanShmaMGA120Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 50, 17, 301)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaAlos16Point1ToSunset()
        {
            var zman = calendar.GetSofZmanShmaAlos16Point1ToSunset();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 48, 26, 796)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaAlos16Point1ToTzaisGeonim7Point083Degrees()
        {
            var zman = calendar.GetSofZmanShmaAlos16Point1ToTzaisGeonim7Point083Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 8, 56, 38, 160)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaKolEliyahu()
        {
            var zman = calendar.GetSofZmanShmaKolEliyahu();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 48, 17, 572)
                ));
        }

        [Test]
        public void Check_GetSofZmanTfilaMGA19Point8Degrees()
        {
            var zman = calendar.GetSofZmanTfilaMGA19Point8Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 19, 22, 457)
                ));
        }

        [Test]
        public void Check_GetSofZmanTfilaMGA16Point1Degrees()
        {
            var zman = calendar.GetSofZmanTfilaMGA16Point1Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 26, 21, 933)
                ));
        }

        [Test]
        public void Check_GetSofZmanTfilaMGA72Minutes()
        {
            var zman = calendar.GetSofZmanTfilaMGA72Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 29, 49, 124)
                ));
        }

        [Test]
        public void Check_GetSofZmanTfilaMGA72MinutesZmanis()
        {
            var zman = calendar.GetSofZmanTfilaMGA72MinutesZmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 28, 24, 393)
                ));
        }

        [Test]
        public void Check_GetSofZmanTfilaMGA90Minutes()
        {
            var zman = calendar.GetSofZmanTfilaMGA90Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 23, 49, 124)
                ));
        }

        [Test]
        public void Check_GetSofZmanTfilaMGA90MinutesZmanis()
        {
            var zman = calendar.GetSofZmanTfilaMGA90MinutesZmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 22, 3, 210)
                ));
        }

        [Test]
        public void Check_GetSofZmanTfilaMGA96Minutes()
        {
            var zman = calendar.GetSofZmanTfilaMGA96Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 21, 49, 124)
                ));
        }

        [Test]
        public void Check_GetSofZmanTfilaMGA96MinutesZmanis()
        {
            var zman = calendar.GetSofZmanTfilaMGA96MinutesZmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 19, 56, 152)
                ));
        }

        [Test]
        public void Check_GetSofZmanTfilaMGA120Minutes()
        {
            var zman = calendar.GetSofZmanTfilaMGA120Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 13, 49, 124)
                ));
        }

        [Test]
        public void Check_GetSofZmanTfila2HoursBeforeChatzos()
        {
            var zman = calendar.GetSofZmanTfila2HoursBeforeChatzos();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 11, 0, 52, 770)
                ));
        }

        [Test]
        public void Check_GetMinchaGedola30Minutes()
        {
            var zman = calendar.GetMinchaGedola30Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 30, 52, 770)
                ));
        }

        [Test]
        public void Check_GetMinchaGedola72Minutes()
        {
            var zman = calendar.GetMinchaGedola72Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 38, 38, 681)
                ));
        }

        [Test]
        public void Check_GetMinchaGedola16Point1Degrees()
        {
            var zman = calendar.GetMinchaGedola16Point1Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 39, 34, 83)
                ));
        }

        [Test]
        public void Check_GetMinchaGedolaGreaterThan30()
        {
            var zman = calendar.GetMinchaGedolaGreaterThan30();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 32, 38, 681)
                ));
        }

        [Test]
        public void Check_GetMinchaKetana16Point1Degrees()
        {
            var zman = calendar.GetMinchaKetana16Point1Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 17, 31, 24, 663)
                ));
        }

        [Test]
        public void Check_GetMinchaKetana72Minutes()
        {
            var zman = calendar.GetMinchaKetana72Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 17, 25, 14, 150)
                ));
        }

        [Test]
        public void Check_GetPlagHamincha60Minutes()
        {
            var zman = calendar.GetPlagHamincha60Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 50, 8, 929)
                ));
        }

        [Test]
        public void Check_GetPlagHamincha72Minutes()
        {
            var zman = calendar.GetPlagHamincha72Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 59, 38, 929)
                ));
        }

        [Test]
        public void Check_GetPlagHamincha90Minutes()
        {
            var zman = calendar.GetPlagHamincha90Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 13, 53, 929)
                ));
        }

        [Test]
        public void Check_GetPlagHamincha96Minutes()
        {
            var zman = calendar.GetPlagHamincha96Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 18, 38, 929)
                ));
        }

        [Test]
        public void Check_GetPlagHamincha96MinutesZmanis()
        {
            var zman = calendar.GetPlagHamincha96MinutesZmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 23, 7, 237)
                ));
        }

        [Test]
        public void Check_GetPlagHamincha90MinutesZmanis()
        {
            var zman = calendar.GetPlagHamincha90MinutesZmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 18, 5, 461)
                ));
        }

        [Test]
        public void Check_GetPlagHamincha72MinutesZmanis()
        {
            var zman = calendar.GetPlagHamincha72MinutesZmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 3, 0, 155)
                ));
        }

        [Test]
        public void Check_GetPlagHamincha16Point1Degrees()
        {
            var zman = calendar.GetPlagHamincha16Point1Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 8, 0, 738)
                ));
        }

        [Test]
        public void Check_GetPlagHamincha19Point8Degrees()
        {
            var zman = calendar.GetPlagHamincha19Point8Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 24, 41, 529)
                ));
        }

        [Test]
        public void Check_GetPlagHamincha26Degrees()
        {
            var zman = calendar.GetPlagHamincha26Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 54, 2, 131)
                ));
        }

        [Test]
        public void Check_GetPlagHamincha18Degrees()
        {
            var zman = calendar.GetPlagHamincha18Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 16, 30, 975)
                ));
        }

        [Test]
        public void Check_GetPlagAlosToSunset()
        {
            var zman = calendar.GetPlagAlosToSunset();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 17, 54, 3, 578)
                ));
        }

        [Test]
        public void Check_GetPlagAlos16Point1ToTzaisGeonim7Point083Degrees()
        {
            var zman = calendar.GetPlagAlos16Point1ToTzaisGeonim7Point083Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 23, 24, 299)
                ));
        }

        [Test]
        public void Check_GetBainHasmashosRT13Point24Degrees()
        {
            var zman = calendar.GetBainHasmashosRT13Point24Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 28, 42, 435)
                ));
        }

        [Test]
        public void Check_GetBainHasmashosRT58Point5Minutes()
        {
            var zman = calendar.GetBainHasmashosRT58Point5Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 20, 33, 710)
                ));
        }

        [Test]
        public void Check_GetBainHasmashosRT13Point5MinutesBefore7Point083Degrees()
        {
            var zman = calendar.GetBainHasmashosRT13Point5MinutesBefore7Point083Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 41, 19, 167)
                ));
        }

        [Test]
        public void Check_GetBainHasmashosRT2Stars()
        {
            var zman = calendar.GetBainHasmashosRT2Stars();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 50, 48, 280)
                ));
        }

        [Test]
        public void Check_GetTzaisGeonim5Point95Degrees()
        {
            var zman = calendar.GetTzaisGeonim5Point95Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 49, 12, 58)
                ));
        }

        [Test]
        public void Check_GetTzaisGeonim3Point65Degrees()
        {
            var zman = calendar.GetTzaisGeonim3Point65Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 36, 57, 453)
                ));
        }

        [Test]
        public void Check_GetTzaisGeonim3Point676Degrees()
        {
            var zman = calendar.GetTzaisGeonim3Point676Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 37, 5, 730)
                ));
        }

        [Test]
        public void Check_GetTzaisGeonim4Point61Degrees()
        {
            var zman = calendar.GetTzaisGeonim4Point61Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 42, 3, 478)
                ));
        }

        [Test]
        public void Check_GetTzaisGeonim4Point37Degrees()
        {
            var zman = calendar.GetTzaisGeonim4Point37Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 40, 46, 896)
                ));
        }

        [Test]
        public void Check_GetTzaisGeonim5Point88Degrees()
        {
            var zman = calendar.GetTzaisGeonim5Point88Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 48, 49, 625)
                ));
        }

        [Test]
        public void Check_GetTzaisGeonim4Point8Degrees()
        {
            var zman = calendar.GetTzaisGeonim4Point8Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 43, 4, 142)
                ));
        }

        [Test]
        public void Check_GetTzaisGeonim7Point083Degrees()
        {
            var zman = calendar.GetTzaisGeonim7Point083Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 54, 49, 167)
                ));
        }

        [Test]
        public void Check_GetTzaisGeonim8Point5Degrees()
        {
            var zman = calendar.GetTzaisGeonim8Point5Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 2, 52, 939)
                ));
        }

        [Test]
        public void Check_GetTzais60()
        {
            var zman = calendar.GetTzais60();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 22, 3, 710)
                ));
        }

        [Test]
        public void Check_GetTzaisAteretTorah()
        {
            var zman = calendar.GetTzaisAteretTorah();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 2, 3, 710)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaAteretTorah()
        {
            var zman = calendar.GetSofZmanShmaAteretTorah();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 3, 6, 660)
                ));
        }

        [Test]
        public void Check_GetSofZmanTfilahAteretTorah()
        {
            var zman = calendar.GetSofZmanTfilahAteretTorah();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 16, 19, 665)
                ));
        }

        [Test]
        public void Check_GetMinchaGedolaAteretTorah()
        {
            var zman = calendar.GetMinchaGedolaAteretTorah();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 19, 22, 177)
                ));
        }

        [Test]
        public void Check_GetMinchaKetanaAteretTorah()
        {
            var zman = calendar.GetMinchaKetanaAteretTorah();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 16, 59, 1, 192)
                ));
        }

        [Test]
        public void Check_GetPlagHaminchaAteretTorah()
        {
            var zman = calendar.GetPlagHaminchaAteretTorah();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 30, 32, 448)
                ));
        }

        [Test]
        public void Check_GetTzais72Zmanis()
        {
            var zman = calendar.GetTzais72Zmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 38, 17, 897)
                ));
        }

        [Test]
        public void Check_GetTzais90Zmanis()
        {
            var zman = calendar.GetTzais90Zmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 57, 21, 444)
                ));
        }

        [Test]
        public void Check_GetTzais96Zmanis()
        {
            var zman = calendar.GetTzais96Zmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 3, 42, 626)
                ));
        }

        [Test]
        public void Check_GetTzais90()
        {
            var zman = calendar.GetTzais90();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 52, 3, 710)
                ));
        }

        [Test]
        public void Check_GetTzais120()
        {
            var zman = calendar.GetTzais120();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 22, 3, 710)
                ));
        }

        [Test]
        public void Check_GetTzais120Zmanis()
        {
            var zman = calendar.GetTzais120Zmanis();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 29, 7, 356)
                ));
        }

        [Test]
        public void Check_GetTzais16Point1Degrees()
        {
            var zman = calendar.GetTzais16Point1Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 44, 36, 815)
                ));
        }

        [Test]
        public void Check_GetTzais26Degrees()
        {
            var zman = calendar.GetTzais26Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 42, 43, 663)
                ));
        }

        [Test]
        public void Check_GetTzais18Degrees()
        {
            var zman = calendar.GetTzais18Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 55, 21, 164)
                ));
        }

        [Test]
        public void Check_GetTzais19Point8Degrees()
        {
            var zman = calendar.GetTzais19Point8Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 21, 5, 40, 626)
                ));
        }

        [Test]
        public void Check_GetTzais96()
        {
            var zman = calendar.GetTzais96();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 58, 3, 710)
                ));
        }

        [Test]
        public void Check_GetFixedLocalChatzos()
        {
            var zman = calendar.GetFixedLocalChatzos();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 12, 56, 53, 311)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaFixedLocal()
        {
            var zman = calendar.GetSofZmanShmaFixedLocal();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 56, 53, 311)
                ));
        }

        [Test]
        public void Check_GetSofZmanTfilaFixedLocal()
        {
            var zman = calendar.GetSofZmanTfilaFixedLocal();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 56, 53, 311)
                ));
        }

        [Test]
        public void Check_getSofZmanAchilasChametzGRA()
        {
            var zman = calendar.getSofZmanAchilasChametzGRA();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 53, 49, 124)
                ));
        }

        [Test]
        public void Check_GetSofZmanAchilasChametzMGA72Minutes()
        {
            var zman = calendar.GetSofZmanAchilasChametzMGA72Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 29, 49, 124)
                ));
        }

        [Test]
        public void Check_GetSofZmanAchilasChametzMGA16Point1Degrees()
        {
            var zman = calendar.GetSofZmanAchilasChametzMGA16Point1Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 26, 21, 933)
                ));
        }

        [Test]
        public void Check_GetSofZmanBiurChametzGRA()
        {
            var zman = calendar.GetSofZmanBiurChametzGRA();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 11, 57, 20, 947)
                ));
        }

        [Test]
        public void Check_GetSofZmanBiurChametzMGA72Minutes()
        {
            var zman = calendar.GetSofZmanBiurChametzMGA72Minutes();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 11, 45, 20, 947)
                ));
        }

        [Test]
        public void Check_GetSofZmanBiurChametzMGA16Point1Degrees()
        {
            var zman = calendar.GetSofZmanBiurChametzMGA16Point1Degrees();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 11, 43, 38, 793)
                ));
        }

        [Test]
        public void Check_GetTzais()
        {
            var zman = calendar.GetTzais();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 2, 52, 939)
                ));
        }

        [Test]
        public void Check_GetAlosHashachar()
        {
            var zman = calendar.GetAlosHashachar();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 17, 14, 493)
                ));
        }

        [Test]
        public void Check_GetAlos72()
        {
            var zman = calendar.GetAlos72();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 27, 41, 832)
                ));
        }

        [Test]
        public void Check_GetChatzos()
        {
            var zman = calendar.GetChatzos();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 0, 52, 770)
                ));
        }

        [Test]
        public void Check_GetSolarMidnight()
        {
            var zman = calendar.GetSolarMidnight();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 3, 13, 0, 35, 50)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaGRA()
        {
            var zman = calendar.GetSofZmanShmaGRA();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 50, 17, 301)
                ));
        }

        [Test]
        public void Check_GetSofZmanShmaMGA()
        {
            var zman = calendar.GetSofZmanShmaMGA();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 9, 14, 17, 301)
                ));
        }

        [Test]
        public void Check_GetTzais72()
        {
            var zman = calendar.GetTzais72();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 34, 3, 710)
                ));
        }

        [Test]
        public void Check_GetCandleLighting()
        {
            var zman = calendar.GetCandleLighting();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 4, 3, 710)
                ));
        }

        [Test]
        public void Check_GetSofZmanTfilaGRA()
        {
            var zman = calendar.GetSofZmanTfilaGRA();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 53, 49, 124)
                ));
        }

        [Test]
        public void Check_GetSofZmanTfilaMGA()
        {
            var zman = calendar.GetSofZmanTfilaMGA();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 10, 29, 49, 124)
                ));
        }

        [Test]
        public void Check_GetMinchaGedola()
        {
            var zman = calendar.GetMinchaGedola();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 32, 38, 681)
                ));
        }

        [Test]
        public void Check_GetMinchaKetana()
        {
            var zman = calendar.GetMinchaKetana();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 16, 43, 14, 150)
                ));
        }

        [Test]
        public void Check_GetPlagHamincha()
        {
            var zman = calendar.GetPlagHamincha();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 18, 2, 38, 929)
                ));
        }

        [Test]
        public void Check_GetSunrise()
        {
            var zman = calendar.GetSunrise();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 6, 39, 41, 832)
                ));
        }

        [Test]
        public void Check_GetSeaLevelSunrise()
        {
            var zman = calendar.GetSeaLevelSunrise();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 6, 39, 41, 832)
                ));
        }

        [Test]
        public void Check_GetBeginCivilTwilight()
        {
            var zman = calendar.GetBeginCivilTwilight();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 6, 12, 18, 676)
                ));
        }

        [Test]
        public void Check_GetBeginNauticalTwilight()
        {
            var zman = calendar.GetBeginNauticalTwilight();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 39, 55, 441)
                ));
        }

        [Test]
        public void Check_GetBeginAstronomicalTwilight()
        {
            var zman = calendar.GetBeginAstronomicalTwilight();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 5, 6, 31, 438)
                ));
        }

        [Test]
        public void Check_GetSunset()
        {
            var zman = calendar.GetSunset();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 22, 3, 710)
                ));
        }

        [Test]
        public void Check_GetSeaLevelSunset()
        {
            var zman = calendar.GetSeaLevelSunset();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 22, 3, 710)
                ));
        }

        [Test]
        public void Check_GetEndCivilTwilight()
        {
            var zman = calendar.GetEndCivilTwilight();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 19, 49, 28, 84)
                ));
        }

        [Test]
        public void Check_GetEndNauticalTwilight()
        {
            var zman = calendar.GetEndNauticalTwilight();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 21, 53, 607)
                ));
        }

        [Test]
        public void Check_GetEndAstronomicalTwilight()
        {
            var zman = calendar.GetEndAstronomicalTwilight();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 20, 55, 21, 164)
                ));
        }

        [Test]
        public void Check_GetSunTransit()
        {
            var zman = calendar.GetSunTransit();

            Assert.That(zman, Is.EqualTo(
                    new DateTime(2010, 4, 2, 13, 0, 52, 770)
                ));
        }

        [Test]
        public void Check_GetShaahZmanis19Point8Degrees()
        {
            Assert.That(calendar.GetShaahZmanis19Point8Degrees(), Is.EqualTo(4847270));
        }

        [Test]
        public void Check_GetShaahZmanis18Degrees()
        {
            Assert.That(calendar.GetShaahZmanis18Degrees(), Is.EqualTo(4744143));
        }

        [Test]
        public void Check_GetShaahZmanis26Degrees()
        {
            Assert.That(calendar.GetShaahZmanis26Degrees(), Is.EqualTo(5217225));
        }

        [Test]
        public void Check_GetShaahZmanis16Point1Degrees()
        {
            Assert.That(calendar.GetShaahZmanis16Point1Degrees(), Is.EqualTo(4636860));
        }

        [Test]
        public void Check_GetShaahZmanis60Minutes()
        {
            Assert.That(calendar.GetShaahZmanis60Minutes(), Is.EqualTo(4411823));
        }

        [Test]
        public void Check_GetShaahZmanis72Minutes()
        {
            Assert.That(calendar.GetShaahZmanis72Minutes(), Is.EqualTo(4531823));
        }

        [Test]
        public void Check_GetShaahZmanis72MinutesZmanis()
        {
            Assert.That(calendar.GetShaahZmanis72MinutesZmanis(), Is.EqualTo(4574187));
        }

        [Test]
        public void Check_GetShaahZmanis90Minutes()
        {
            Assert.That(calendar.GetShaahZmanis90Minutes(), Is.EqualTo(4711823));
        }

        [Test]
        public void Check_GetShaahZmanis90MinutesZmanis()
        {
            Assert.That(calendar.GetShaahZmanis90MinutesZmanis(), Is.EqualTo(4764778));
        }

        [Test]
        public void Check_GetShaahZmanis96MinutesZmanis()
        {
            Assert.That(calendar.GetShaahZmanis96MinutesZmanis(), Is.EqualTo(4828309));
        }

        [Test]
        public void Check_GetShaahZmanisAteretTorah()
        {
            Assert.That(calendar.GetShaahZmanisAteretTorah(), Is.EqualTo(4393005));
        }

        [Test]
        public void Check_GetShaahZmanis96Minutes()
        {
            Assert.That(calendar.GetShaahZmanis96Minutes(), Is.EqualTo(4771823));
        }

        [Test]
        public void Check_GetShaahZmanis120Minutes()
        {
            Assert.That(calendar.GetShaahZmanis120Minutes(), Is.EqualTo(5011823));
        }

        [Test]
        public void Check_GetShaahZmanis120MinutesZmanis()
        {
            Assert.That(calendar.GetShaahZmanis120MinutesZmanis(), Is.EqualTo(5082430));
        }

        [Test]
        public void Check_GetShaahZmanisGra()
        {
            Assert.That(calendar.GetShaahZmanisGra(), Is.EqualTo(3811823));
        }

        [Test]
        public void Check_GetShaahZmanisMGA()
        {
            Assert.That(calendar.GetShaahZmanisMGA(), Is.EqualTo(4531823));
        }

        [Test]
        public void Check_GetTemporalHour()
        {
            Assert.That(calendar.GetTemporalHour(), Is.EqualTo(3811823));
        }

    }
}