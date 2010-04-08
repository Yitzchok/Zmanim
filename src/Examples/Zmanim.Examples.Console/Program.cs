using System;
using net.sourceforge.zmanim;
using net.sourceforge.zmanim.util;

namespace Zmanim.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            string locationName = "Lakewood, NJ";
            double latitude = 40.09596; //Lakewood, NJ
            double longitude = -74.22213; //Lakewood, NJ
            double elevation = 0; //optional elevation
            java.util.TimeZone timeZone = java.util.TimeZone.getTimeZone("America/New_York");
            GeoLocation location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            ComplexZmanimCalendar zc = new ComplexZmanimCalendar(location);

            Console.WriteLine("Today's Zmanim for " + locationName);
            Console.WriteLine("Sunrise: " + zc.getSunrise()); //output sunrise
            Console.WriteLine("Sof Zman Shema MGA: " + zc.getSofZmanShmaMGA()); //output Sof Zman Shema MGA
            Console.WriteLine("Sof Zman Shema GRA: " + zc.getSofZmanShmaGRA()); //output Sof Zman Shema GRA
            Console.WriteLine("Sunset: " + zc.getSunset()); //output sunset

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
    }
}
