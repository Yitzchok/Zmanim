using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using NDesk.Options;
using net.sourceforge.zmanim;
using net.sourceforge.zmanim.util;

namespace Zmanim.Cli
{
    public class Program
    {
        private static ZmanimOptions options;
        private static OptionSet p;

        private static void Main(string[] args)
        {
            options = new ZmanimOptions();

            p = new OptionSet
                    {
                        {"d|date=", "Set date. <mm/dd/yyyy>", x => options.Date = DateTime.ParseExact(x, "MM/dd/yyyy", CultureInfo.InvariantCulture)},
                        {"lat|latitude=", "Set location's latitude", (double x) => options.Latitude = x},
                        {"lon|longitude=", "Set location's longitude", (double x) => options.Longitude = x},
                        {"e|elevation=", "Set location's elevation; Positive only", (double x) => options.Elevation = x},
                        {"tz|timezone=", "Set location's TimeZone", x => options.TimeZone = x},
                        {"tf|timeformat=", "Set the way the application formats a DateTime object.", x => options.DateTimeFormat = x},
                        {"h|?|help", "Shows this help message", v => ShowHelp()}
                    };

            List<string> extraArgs;
            try
            {
                extraArgs = p.Parse(args);
            }
            catch (OptionException e)
            {
                ShowHelp();
                return;
            }

            var timeZone = java.util.TimeZone.getTimeZone(options.TimeZone);
            var location =
                new GeoLocation(string.Empty, options.Latitude, options.Longitude, options.Elevation, timeZone);
            var czc = new ComplexZmanimCalendar(location);

            czc.setCalendar(new DefaultCalendar { Date = new DateTime(options.Date.Year, options.Date.Month, options.Date.Day) });

            var methods = GetDateTimeAndLongMethods();

            foreach (var first in
                extraArgs.Select(extraArg =>
                                 methods.Where(
                                     f => f.Name.Remove(0, 3).ToLowerInvariant() == extraArg.ToLowerInvariant()).First())
                )
            {
                object invoke = first.Invoke(czc, null);

                if (extraArgs.Count > 1)
                    Console.Write(first.Name.Remove(0, 3) + ": ");

                if (invoke.GetType() == typeof(DateTime))
                {
                    var time = (DateTime)invoke;
                    Console.Write(time.ToString(options.DateTimeFormat));
                }
                else if (invoke.GetType() == typeof(long))
                {
                    Console.WriteLine((long)invoke);
                }

                if (extraArgs.Count > 1)
                    Console.WriteLine();
            }
            //Console.Read();
        }

        private static IEnumerable<MethodInfo> GetDateTimeAndLongMethods()
        {
            return typeof(ComplexZmanimCalendar).GetMethods()
                .Where(m => (m.ReturnType == typeof(DateTime) || m.ReturnType == typeof(long))
                            && m.Name.StartsWith("get")
                            && m.IsPublic
                            && m.GetParameters().Count() == 0);
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Usage: Zmanim Cli");
            p.WriteOptionDescriptions(Console.Out);

            Console.WriteLine("Example:");
            Console.WriteLine("sunrise");
            Console.WriteLine("sunrise sunset alos60");
            Console.WriteLine("--latitude=31.7780 --longitude=35.235149 --elevation=600 --timezone=Israel Sunrise");

            Console.Write("Press any key to exit.");
            Console.Read();

            Environment.Exit(0);
        }
    }
}