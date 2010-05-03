using Zmanim.TimeZone;
using Zmanim.Utilities;

namespace Zmanim.Samples.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string locationName = "Lakewood, NJ";
            double latitude = 40.09596; //Lakewood, NJ
            double longitude = -74.22213; //Lakewood, NJ
            double elevation = 0; //optional elevation
            ITimeZone timeZone = new OlsonTimeZone("America/New_York");
            GeoLocation location = new GeoLocation(locationName, latitude, longitude, elevation, timeZone);
            ComplexZmanimCalendar zc = new ComplexZmanimCalendar(location);

            System.Console.WriteLine("Today's Zmanim for " + locationName);
            System.Console.WriteLine("Sunrise: " + zc.getSunrise()); //output sunrise
            System.Console.WriteLine("Sof Zman Shema MGA: " + zc.getSofZmanShmaMGA()); //output Sof Zman Shema MGA
            System.Console.WriteLine("Sof Zman Shema GRA: " + zc.getSofZmanShmaGRA()); //output Sof Zman Shema GRA
            System.Console.WriteLine("Sunset: " + zc.getSunset()); //output sunset

            System.Console.WriteLine("Press enter to exit.");
            System.Console.ReadLine();
        }
    }
}
