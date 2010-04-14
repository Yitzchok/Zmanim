using System;

namespace Zmanim.Cli
{
    public class ZmanimOptions
    {
        public ZmanimOptions()
        {
            Date = DateTime.Now;
            Latitude = 40.09596;
            Longitude = -74.22213;
            TimeZone = "America/New_York";
        }

        public DateTime Date { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public string TimeZone { get; set; }
        public string MethodName { get; set; }
    }
}