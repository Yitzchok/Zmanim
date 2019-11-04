using System;
using NUnit.Framework;
using Zmanim;
using Zmanim.TimeZone;
using Zmanim.TzDatebase;
using Zmanim.Utilities;

namespace ZmanimTests
{
    [TestFixture]
    public abstract class BaseZmanimTests
    {
        public ComplexZmanimCalendar GetCalendar()
        {
            String locationName = "Lakewood, NJ";
            double latitude = 40.09596; //Lakewood, NJ
            double longitude = -74.22213; //Lakewood, NJ
            double elevation = 0; //optional elevation
            ITimeZone timeZone = new OlsonTimeZone("America/New_York");
            GeoLocation location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            ComplexZmanimCalendar czc = new ComplexZmanimCalendar(new DateTime(2010, 4, 2), location);
            return czc;
        }
    }
}