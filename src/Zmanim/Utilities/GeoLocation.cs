// * Zmanim .NET API
// * Copyright (C) 2004-2010 Eliyahu Hershfeld
// *
// * Converted to C# by AdminJew
// *
// * This file is part of Zmanim .NET API.
// *
// * Zmanim .NET API is free software: you can redistribute it and/or modify
// * it under the terms of the GNU Lesser General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * Zmanim .NET API is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU Lesser General Public License for more details.
// *
// * You should have received a copy of the GNU Lesser General Public License
// * along with Zmanim.NET API.  If not, see <http://www.gnu.org/licenses/lgpl.html>.

using System;
using System.Text;
using Zmanim.Calculator;
using Zmanim.Extensions;
using Zmanim.TimeZone;

namespace Zmanim.Utilities
{
    /// <summary>
    ///   A class that contains location information such as latitude and longitude
    ///   required for astronomical calculations. The elevation field is not used by
    ///   most calculation engines and would be ignored if set. Check the documentation
    ///   for specific implementations of the <seealso cref = "AstronomicalCalculator" /> to see if
    ///   elevation is calculated as part o the algorithm.
    /// </summary>
    /// <author>Eliyahu Hershfeld</author>
    public class GeoLocation : IGeoLocation
    {
        /// <summary>
        ///   constant for milliseconds in a minute (60,000)
        /// </summary>
        private const long MINUTE_MILLIS = 60 * 1000;

        /// <summary>
        ///   constant for milliseconds in an hour (3,600,000)
        /// </summary>
        private const long HOUR_MILLIS = MINUTE_MILLIS * 60;

        private int DISTANCE;
        private int FINAL_BEARING = 2;
        private int INITIAL_BEARING = 1;
        private double elevation;

        private double latitude;
        private string locationName;
        private double longitude;
        private ITimeZone timeZone;

        ///<summary>
        ///  GeoLocation constructor with parameters for all required fields.
        ///</summary>
        ///<param name = "latitude">
        ///  the latitude in a double format such as 40.095965 for
        ///  Lakewood, NJ <br /> <b>Note: </b> For latitudes south of the
        ///  equator, a negative value should be used. </param>
        ///<param name = "longitude">
        ///  double the longitude in a double format such as -74.222130 for
        ///  Lakewood, NJ. <br /> <b>Note: </b> For longitudes east of the
        ///  <a href = "http://en.wikipedia.org/wiki/Prime_Meridian">Prime
        ///    Meridian </a> (Greenwich), a negative value should be used. </param>
        ///<param name = "timeZone">
        ///  the <c>TimeZone</c> for the location. </param>
        public GeoLocation(double latitude, double longitude, ITimeZone timeZone)
            : this(string.Empty, latitude, longitude, 0, timeZone)
        {
        }

        ///<summary>
        ///  GeoLocation constructor with parameters for all required fields.
        ///</summary>
        ///<param name = "name">
        ///  The location name for display use such as &quot;Lakewood,
        ///  NJ&quot; </param>
        ///<param name = "latitude">
        ///  the latitude in a double format such as 40.095965 for
        ///  Lakewood, NJ <br /> <b>Note: </b> For latitudes south of the
        ///  equator, a negative value should be used. </param>
        ///<param name = "longitude">
        ///  double the longitude in a double format such as -74.222130 for
        ///  Lakewood, NJ. <br /> <b>Note: </b> For longitudes east of the
        ///  <a href = "http://en.wikipedia.org/wiki/Prime_Meridian">Prime
        ///    Meridian </a> (Greenwich), a negative value should be used. </param>
        ///<param name = "timeZone">
        ///  the <c>TimeZone</c> for the location. </param>
        public GeoLocation(string name, double latitude, double longitude, ITimeZone timeZone)
            : this(name, latitude, longitude, 0, timeZone)
        {
        }


        ///<summary>
        ///  GeoLocation constructor with parameters for all required fields.
        ///</summary>
        ///<param name = "name">
        ///  The location name for display use such as &quot;Lakewood,
        ///  NJ&quot; </param>
        ///<param name = "latitude">
        ///  the latitude in a double format such as 40.095965 for
        ///  Lakewood, NJ <br /> <b>Note: </b> For latitudes south of the
        ///  equator, a negative value should be used. </param>
        ///<param name = "longitude">
        ///  double the longitude in a double format such as -74.222130 for
        ///  Lakewood, NJ. <br /> <b>Note: </b> For longitudes east of the
        ///  <a href = "http://en.wikipedia.org/wiki/Prime_Meridian">Prime
        ///    Meridian </a> (Greenwich), a negative value should be used. </param>
        ///<param name = "elevation">
        ///  the elevation above sea level in Meters. Elevation is not used
        ///  in most algorithms used for calculating sunrise and set. </param>
        ///<param name = "timeZone">
        ///  the <c>TimeZone</c> for the location. </param>
        public GeoLocation(string name, double latitude, double longitude, double elevation, ITimeZone timeZone)
        {
            LocationName = name;
            Latitude = latitude;
            Longitude = longitude;
            Elevation = elevation;
            TimeZone = timeZone;
        }


        ///<summary>
        ///  Default GeoLocation constructor will set location to the Prime Meridian
        ///  at Greenwich, England and a TimeZone of GMT. The longitude will be set to
        ///  0 and the latitude will be 51.4772 to match the location of the
        ///  <a href = "http://www.rog.nmm.ac.uk">Royal Observatory, Greenwich </a>. No
        ///  daylight savings time will be used.
        ///</summary>
        public GeoLocation()
        {
            LocationName = "Greenwich, England";
            Longitude = 0; // added for clarity
            Latitude = 51.4772;
            TimeZone = new OffsetTimeZone(0);
        }

        ///<summary>
        ///  Method to get the elevation in Meters.
        ///</summary>
        ///<value> Returns the elevation in Meters. </value>
        public virtual double Elevation
        {
            get { return elevation; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Elevation cannot be negative");
                }
                this.elevation = value;
            }
        }


        ///<summary>
        ///  Method to set the latitude.
        ///</summary>
        ///<value>
        ///  The degrees of latitude to set. The values should be between
        ///  -90° and 90°. An IllegalArgumentException will be
        ///  thrown if the value exceeds the limit. For example 40.095965
        ///  would be used for Lakewood, NJ. &lt;b&gt;Note: &lt;/b&gt; For latitudes south of the
        ///  equator, a negative value should be used. </value>
        public virtual double Latitude
        {
            set
            {
                if (value > 90 || value < -90)
                {
                    throw new ArgumentException("Latitude must be between -90 and  90");
                }
                this.latitude = value;
            }
            get { return latitude; }
        }


        ///<summary>
        ///  Method to set the latitude in degrees, minutes and seconds.
        ///</summary>
        ///<param name = "degrees">
        ///  The degrees of latitude to set between -90 and 90. An
        ///  IllegalArgumentException will be thrown if the value exceeds
        ///  the limit. For example 40 would be used for Lakewood, NJ. </param>
        ///<param name = "minutes"> <a href = "http://en.wikipedia.org/wiki/Minute_of_arc#Cartography">minutes of arc</a> </param>
        ///<param name = "seconds"> <a href = "http://en.wikipedia.org/wiki/Minute_of_arc#Cartography">seconds of arc</a> </param>
        ///<param name = "direction">
        ///  N for north and S for south. An IllegalArgumentException will
        ///  be thrown if the value is not S or N. </param>
        public virtual void SetLatitude(int degrees, int minutes, double seconds, string direction)
        {
            double tempLat = degrees + ((minutes + (seconds / 60.0)) / 60.0);
            if (tempLat > 90 || tempLat < 0)
            {
                throw new ArgumentException(
                    "Latitude must be between 0 and  90. Use direction of S instead of negative.");
            }
            if (direction.Equals("S"))
            {
                tempLat *= -1;
            }
            else if (!direction.Equals("N"))
            {
                throw new ArgumentException("Latitude direction must be N or S");
            }
            latitude = tempLat;
        }


        ///<summary>
        ///  Method to set the longitude in a double format.
        ///</summary>
        ///<value>
        ///  The degrees of longitude to set in a double format between
        ///  -180° and 180°. An IllegalArgumentException will be
        ///  thrown if the value exceeds the limit. For example -74.2094
        ///  would be used for Lakewood, NJ. Note: for longitudes east of
        ///  the &lt;a href = &quot;http://en.wikipedia.org/wiki/Prime_Meridian&quot;&gt;Prime
        ///  Meridian&lt;/a&gt; (Greenwich) a negative value should be used. </value>
        public virtual double Longitude
        {
            set
            {
                if (value > 180 || value < -180)
                {
                    throw new ArgumentException("Longitude must be between -180 and  180");
                }
                this.longitude = value;
            }
            get { return longitude; }
        }


        ///<summary>
        ///  Method to set the longitude in degrees, minutes and seconds.
        ///</summary>
        ///<param name = "degrees">
        ///  The degrees of longitude to set between -180 and 180. An
        ///  IllegalArgumentException will be thrown if the value exceeds
        ///  the limit. For example -74 would be used for Lakewood, NJ.
        ///  Note: for longitudes east of the <a href = "http://en.wikipedia.org/wiki/Prime_Meridian">Prime
        ///                                     Meridian </a> (Greenwich) a negative value should be used. </param>
        ///<param name = "minutes"> <a href = "http://en.wikipedia.org/wiki/Minute_of_arc#Cartography">minutes of arc</a> </param>
        ///<param name = "seconds"> <a href = "http://en.wikipedia.org/wiki/Minute_of_arc#Cartography">seconds of arc</a> </param>
        ///<param name = "direction">
        ///  E for east of the Prime Meridian or W for west of it. An
        ///  IllegalArgumentException will be thrown if the value is not E
        ///  or W. </param>
        public virtual void SetLongitude(int degrees, int minutes, double seconds, string direction)
        {
            double longTemp = degrees + ((minutes + (seconds / 60.0)) / 60.0);
            if (longTemp > 180 || longitude < 0)
            {
                throw new ArgumentException("Longitude must be between 0 and  180. Use the ");
            }
            if (direction.Equals("W"))
            {
                longTemp *= -1;
            }
            else if (!direction.Equals("E"))
            {
                throw new ArgumentException("Longitude direction must be E or W");
            }
            longitude = longTemp;
        }


        ///<value> Returns the location name. </value>
        public virtual string LocationName
        {
            get { return locationName; }
            set { locationName = value; }
        }


        ///<value> Returns the timeZone. </value>
        public virtual ITimeZone TimeZone
        {
            get { return timeZone; }
            set { this.timeZone = value; }
        }

        /// <summary>
        /// A method that will return the location's local mean time offset in
        /// milliseconds from local standard time. The globe is split into 360°,
        /// with 15° per hour of the day. For a local that is at a longitude that
        /// is evenly divisible by 15 (longitude % 15 == 0), at solar
        /// <see cref="AstronomicalCalendar.GetSunTransit">noon</see>
        /// (with adjustment for the <a href="http://en.wikipedia.org/wiki/Equation_of_time">equation of time</a>)
        /// the sun should be directly overhead, so a user who is 1° west of this
        /// will have noon at 4 minutes after standard time noon, and conversely, a
        /// user who is 1° east of the 15° longitude will have noon at 11:56
        /// AM.
        /// </summary>
        /// <param name="date">The date used to get the UtcOffset.</param>
        /// <returns>
        /// the offset in milliseconds not accounting for Daylight saving
        /// time. A positive value will be returned East of the timezone
        /// line, and a negative value West of it.
        /// </returns>
        public virtual long GetLocalMeanTimeOffset(DateTime date)
        {
            return (long)(Longitude * 4 * MINUTE_MILLIS - TimeZone.UtcOffset(date));
        }

        ///<summary>
        ///  Calculate the initial <a href = "http://en.wikipedia.org/wiki/Great_circle">geodesic</a> bearing
        ///  between this Object and a second Object passed to this method using
        ///  <a href = "http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ///  inverse formula See T Vincenty, "<a href = "http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ///                                     Solutions of Geodesics on the Ellipsoid with application of nested
        ///                                     equations</a>", Survey Review, vol XXII no 176, 1975
        ///</summary>
        ///<param name = "location">
        ///  the destination location </param>
        public virtual double GetGeodesicInitialBearing(GeoLocation location)
        {
            return VincentyFormula(location, INITIAL_BEARING);
        }


        ///<summary>
        ///  Calculate the final <a href = "http://en.wikipedia.org/wiki/Great_circle">geodesic</a> bearing
        ///  between this Object and a second Object passed to this method using
        ///  <a href = "http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ///  inverse formula See T Vincenty, "<a href = "http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ///                                     Solutions of Geodesics on the Ellipsoid with application of nested
        ///                                     equations</a>", Survey Review, vol XXII no 176, 1975
        ///</summary>
        ///<param name = "location">
        ///  the destination location </param>
        public virtual double GetGeodesicFinalBearing(GeoLocation location)
        {
            return VincentyFormula(location, FINAL_BEARING);
        }

        ///<summary>
        ///  Calculate <a href = "http://en.wikipedia.org/wiki/Great-circle_distance">geodesic
        ///              distance</a> in Meters between this Object and a second Object passed to
        ///  this method using <a href = "http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ///  inverse formula See T Vincenty, "<a href = "http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ///                                     Solutions of Geodesics on the Ellipsoid with application of nested
        ///                                     equations</a>", Survey Review, vol XXII no 176, 1975
        ///</summary>
        ///<param name = "location">
        ///  the destination location </param>
        public virtual double GetGeodesicDistance(GeoLocation location)
        {
            return VincentyFormula(location, DISTANCE);
        }

        ///<summary>
        ///  Calculate <a href = "http://en.wikipedia.org/wiki/Great-circle_distance">geodesic
        ///              distance</a> in Meters between this Object and a second Object passed to
        ///  this method using <a href = "http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ///  inverse formula See T Vincenty, "<a href = "http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ///                                     Solutions of Geodesics on the Ellipsoid with application of nested
        ///                                     equations</a>", Survey Review, vol XXII no 176, 1975
        ///</summary>
        ///<param name = "location">
        ///  the destination location </param>
        ///<param name = "formula">
        ///  This formula calculates initial bearing (<seealso cref = "INITIAL_BEARING" />),
        ///  final bearing (<seealso cref = "FINAL_BEARING" />) and distance (<seealso cref = "DISTANCE" />). </param>
        private double VincentyFormula(GeoLocation location, int formula)
        {
            double a = 6378137;
            double b = 6356752.3142;
            double f = 1 / 298.257223563; // WGS-84 ellipsiod
            double L = MathExtensions.ToRadians(location.Longitude - Longitude);
            double U1 = Math.Atan((1 - f) * Math.Tan(MathExtensions.ToRadians(Latitude)));
            double U2 = Math.Atan((1 - f) * Math.Tan(MathExtensions.ToRadians(location.Latitude)));
            double sinU1 = Math.Sin(U1), cosU1 = Math.Cos(U1);
            double sinU2 = Math.Sin(U2), cosU2 = Math.Cos(U2);

            double lambda = L;
            double lambdaP = 2 * Math.PI;
            double iterLimit = 20;
            double sinLambda = 0;
            double cosLambda = 0;
            double sinSigma = 0;
            double cosSigma = 0;
            double sigma = 0;
            double sinAlpha = 0;
            double cosSqAlpha = 0;
            double cos2SigmaM = 0;
            double C;
            while (Math.Abs(lambda - lambdaP) > 1e-12 && --iterLimit > 0)
            {
                sinLambda = Math.Sin(lambda);
                cosLambda = Math.Cos(lambda);
                sinSigma =
                    Math.Sqrt((cosU2 * sinLambda) * (cosU2 * sinLambda) +
                                     (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda) * (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda));
                if (sinSigma == 0)
                    return 0; // co-incident points
                cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;
                sigma = Math.Atan2(sinSigma, cosSigma);
                sinAlpha = cosU1 * cosU2 * sinLambda / sinSigma;
                cosSqAlpha = 1 - sinAlpha * sinAlpha;
                cos2SigmaM = cosSigma - 2 * sinU1 * sinU2 / cosSqAlpha;
                if (double.IsNaN(cos2SigmaM))
                    cos2SigmaM = 0; // equatorial line: cosSqAlpha=0 (§6)
                C = f / 16 * cosSqAlpha * (4 + f * (4 - 3 * cosSqAlpha));
                lambdaP = lambda;
                lambda = L +
                         (1 - C) * f * sinAlpha *
                         (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));
            }
            if (iterLimit == 0)
                return double.NaN; // formula failed to converge

            double uSq = cosSqAlpha * (a * a - b * b) / (b * b);
            double A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
            double B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));
            double deltaSigma = B * sinSigma *
                                (cos2SigmaM +
                                 B / 4 *
                                 (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM) -
                                  B / 6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2SigmaM * cos2SigmaM)));
            double distance = b * A * (sigma - deltaSigma);

            // initial bearing
            double fwdAz = MathExtensions.ToDegree(Math.Atan2(cosU2 * sinLambda, cosU1 * sinU2 - sinU1 * cosU2 * cosLambda));
            // final bearing
            double revAz = MathExtensions.ToDegree(Math.Atan2(cosU1 * sinLambda, -sinU1 * cosU2 + cosU1 * sinU2 * cosLambda));
            if (formula == DISTANCE)
            {
                return distance;
            }
            else if (formula == INITIAL_BEARING)
            {
                return fwdAz;
            }
            else if (formula == FINAL_BEARING)
            {
                return revAz;
            } // should never happpen
            else
            {
                return double.NaN;
            }
        }


        ///<summary>
        ///  Returns the <a href = "http://en.wikipedia.org/wiki/Rhumb_line">rhumb line</a>
        ///  bearing from the current location to the GeoLocation passed in.
        ///</summary>
        ///<param name = "location">
        ///  destination location </param>
        ///<returns> the bearing in degrees </returns>
        public virtual double GetRhumbLineBearing(GeoLocation location)
        {
            double dLon = MathExtensions.ToRadians(location.Longitude - Longitude);
            double dPhi =
                Math.Log(Math.Tan(MathExtensions.ToRadians(location.Latitude) / 2 + Math.PI / 4) /
                                Math.Tan(MathExtensions.ToRadians(Latitude) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return MathExtensions.ToDegree(Math.Atan2(dLon, dPhi));
        }


        ///<summary>
        ///  Returns the <a href = "http://en.wikipedia.org/wiki/Rhumb_line">rhumb line</a>
        ///  distance from the current location to the GeoLocation passed in.
        ///</summary>
        ///<param name = "location">
        ///  the destination location </param>
        ///<returns> the distance in Meters </returns>
        public virtual double GetRhumbLineDistance(GeoLocation location)
        {
            double R = 6371; // earth's mean radius in km
            double dLat = MathExtensions.ToRadians(location.Latitude - Latitude);
            double dLon = MathExtensions.ToRadians(Math.Abs(location.Longitude - Longitude));
            double dPhi =
                Math.Log(Math.Tan(MathExtensions.ToRadians(location.Longitude) / 2 + Math.PI / 4) /
                                Math.Tan(MathExtensions.ToRadians(Latitude) / 2 + Math.PI / 4));
            double q = (Math.Abs(dLat) > 1e-10) ? dLat / dPhi : Math.Cos(MathExtensions.ToRadians(Latitude));
            // if dLon over 180° take shorter rhumb across 180° meridian:
            if (dLon > Math.PI)
                dLon = 2 * Math.PI - dLon;
            double d = Math.Sqrt(dLat * dLat + q * q * dLon * dLon);
            return d * R;
        }


        ///<summary>
        ///  A method that returns an XML formatted <c>String</c> representing
        ///  the serialized <c>Object</c>. Very similar to the toString
        ///  method but the return value is in an xml format. The format currently
        ///  used (subject to change) is:
        ///	
        ///  <code>
        ///    &lt;GeoLocation&gt;
        ///    &lt;LocationName&gt;Lakewood, NJ&lt;/LocationName&gt;
        ///    &lt;Latitude&gt;40.0828&amp;deg&lt;/Latitude&gt;
        ///    &lt;Longitude&gt;-74.2094&amp;deg&lt;/Longitude&gt;
        ///    &lt;Elevation&gt;0 Meters&lt;/Elevation&gt;
        ///    &lt;TimezoneName&gt;America/New_York&lt;/TimezoneName&gt;
        ///    &lt;TimeZoneDisplayName&gt;Eastern Standard Time&lt;/TimeZoneDisplayName&gt;
        ///    &lt;TimezoneGMTOffset&gt;-5&lt;/TimezoneGMTOffset&gt;
        ///    &lt;TimezoneDSTOffset&gt;1&lt;/TimezoneDSTOffset&gt;
        ///    &lt;/GeoLocation&gt;
        ///  </code>
        ///</summary>
        ///<returns> The XML formatted <code>String</code>. </returns>
        public virtual string ToXml()
        {
            var sb = new StringBuilder();
            sb.Append("<GeoLocation>\n");
            sb.Append("\t<LocationName>").Append(LocationName).Append("</LocationName>\n");
            sb.Append("\t<Latitude>").Append(Latitude).Append("°").Append("</Latitude>\n");
            sb.Append("\t<Longitude>").Append(Longitude).Append("°").Append("</Longitude>\n");
            sb.Append("\t<Elevation>").Append(Elevation).Append(" Meters").Append("</Elevation>\n");
            sb.Append("\t<TimezoneName>").Append(TimeZone.GetId()).Append("</TimezoneName>\n");
            sb.Append("\t<TimeZoneDisplayName>").Append(TimeZone.GetDisplayName()).Append(
                "</TimeZoneDisplayName>\n");
            /*
            sb.Append("\t<TimezoneGMTOffset>").Append(getTimeZone().getRawOffset() / HOUR_MILLIS).Append(
                "</TimezoneGMTOffset>\n");
            sb.Append("\t<TimezoneDSTOffset>").Append(getTimeZone().getDSTSavings() / HOUR_MILLIS).Append(
                "</TimezoneDSTOffset>\n");
            */
            sb.Append("</GeoLocation>");
            return sb.ToString();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (!(obj is GeoLocation))
                return false;
            var geo = (GeoLocation)obj;
            return BitConverter.DoubleToInt64Bits(latitude) == BitConverter.DoubleToInt64Bits(geo.latitude) &&
                   BitConverter.DoubleToInt64Bits(longitude) == BitConverter.DoubleToInt64Bits(geo.longitude) &&
                   elevation == geo.elevation &&
                   (locationName == null ? geo.locationName == null : locationName.Equals(geo.locationName)) &&
                   (timeZone == null ? geo.timeZone == null : timeZone.Equals(geo.timeZone));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int result = 17;
            long latLong = BitConverter.DoubleToInt64Bits(latitude);
            long lonLong = BitConverter.DoubleToInt64Bits(longitude);
            long elevLong = BitConverter.DoubleToInt64Bits(elevation);
            var latInt = (int)(latLong ^ (latLong >> 32));
            var lonInt = (int)(lonLong ^ (lonLong >> 32));
            var elevInt = (int)(elevLong ^ (elevLong >> 32));
            result = 37 * result + GetType().GetHashCode();
            result += 37 * result + latInt;
            result += 37 * result + lonInt;
            result += 37 * result + elevInt;
            result += 37 * result + (locationName == null ? 0 : locationName.GetHashCode());
            result += 37 * result + (timeZone == null ? 0 : timeZone.GetHashCode());
            return result;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("\nLocation Name:\t\t\t").Append(LocationName);
            sb.Append("\nLatitude:\t\t\t").Append(Latitude).Append("°");
            sb.Append("\nLongitude:\t\t\t").Append(Longitude).Append("°");
            sb.Append("\nElevation:\t\t\t").Append(Elevation).Append(" Meters");
            sb.Append("\nTimezone Name:\t\t\t").Append(TimeZone.GetId());
            //        
            //		 * sb.append("\nTimezone Display Name:\t\t").append(
            //		 * getTimeZone().getDisplayName());
            //		 
            /*
            sb.Append("\nTimezone GMT Offset:\t\t").Append(getTimeZone().getRawOffset() / HOUR_MILLIS);
            sb.Append("\nTimezone DST Offset:\t\t").Append(getTimeZone().getDSTSavings() / HOUR_MILLIS);
            */
            return sb.ToString();
        }
    }
}