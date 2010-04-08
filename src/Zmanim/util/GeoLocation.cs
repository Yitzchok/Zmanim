using System;
using java.util;
using TimeZone = java.util.TimeZone;

namespace net.sourceforge.zmanim.util
{
    /// <summary>
    /// A class that contains location information such as latitude and longitude
    /// required for astronomical calculations. The elevation field is not used by
    /// most calculation engines and would be ignored if set. Check the documentation
    /// for specific implementations of the <seealso cref="AstronomicalCalculator"/> to see if
    /// elevation is calculated as part o the algorithm.
    /// 
    /// @author &copy; Eliyahu Hershfeld 2004 - 2008
    /// @version 1.1 </summary>
    public class GeoLocation : ICloneable
    {
        private double latitude;
        private double longitude;
        private string locationName;
        private java.util.TimeZone timeZone;
        private double elevation;
        private int DISTANCE = 0;
        private int INITIAL_BEARING = 1;
        private int FINAL_BEARING = 2;

        /// <summary>constant for milliseconds in a minute (60,000)</summary>
        private const long MINUTE_MILLIS = 60 * 1000;

        /// <summary>constant for milliseconds in an hour (3,600,000)</summary>
        private const long HOUR_MILLIS = MINUTE_MILLIS * 60;



        ///	 <summary> Method to get the elevation in Meters.
        ///	 </summary>
        ///	<returns> Returns the elevation in Meters. </returns>
        public virtual double getElevation()
        {
            return elevation;
        }


        ///	 <summary> Method to set the elevation in Meters <b>above </b> sea level.
        ///	 </summary>
        ///	<param name="elevation">
        ///	           The elevation to set in Meters. An IllegalArgumentException
        ///	           will be thrown if the value is a negative. </param>
        public virtual void setElevation(double elevation)
        {
            if (elevation < 0)
            {
                throw new System.ArgumentException("Elevation cannot be negative");
            }
            this.elevation = elevation;
        }


        ///	 <summary> GeoLocation constructor with parameters for all required fields.
        ///	 </summary>
        ///	<param name="name">
        ///	           The location name for display use such as &quot;Lakewood,
        ///	           NJ&quot; </param>
        ///	<param name="latitude">
        ///	           the latitude in a double format such as 40.095965 for
        ///	           Lakewood, NJ <br/> <b>Note: </b> For latitudes south of the
        ///	           equator, a negative value should be used. </param>
        ///	<param name="longitude">
        ///	           double the longitude in a double format such as -74.222130 for
        ///	           Lakewood, NJ. <br/> <b>Note: </b> For longitudes east of the
        ///	           <a href="http://en.wikipedia.org/wiki/Prime_Meridian">Prime
        ///	           Meridian </a> (Greenwich), a negative value should be used. </param>
        ///	<param name="timeZone">
        ///	           the <code>TimeZone</code> for the location. </param>
        public GeoLocation(string name, double latitude, double longitude, TimeZone timeZone)
            : this(name, latitude, longitude, 0, timeZone)
        {
        }


        ///	 <summary> GeoLocation constructor with parameters for all required fields.
        ///	 </summary>
        ///	<param name="name">
        ///	           The location name for display use such as &quot;Lakewood,
        ///	           NJ&quot; </param>
        ///	<param name="latitude">
        ///	           the latitude in a double format such as 40.095965 for
        ///	           Lakewood, NJ <br/> <b>Note: </b> For latitudes south of the
        ///	           equator, a negative value should be used. </param>
        ///	<param name="longitude">
        ///	           double the longitude in a double format such as -74.222130 for
        ///	           Lakewood, NJ. <br/> <b>Note: </b> For longitudes east of the
        ///	           <a href="http://en.wikipedia.org/wiki/Prime_Meridian">Prime
        ///	           Meridian </a> (Greenwich), a negative value should be used. </param>
        ///	<param name="elevation">
        ///	           the elevation above sea level in Meters. Elevation is not used
        ///	           in most algorithms used for calculating sunrise and set. </param>
        ///	<param name="timeZone">
        ///	           the <code>TimeZone</code> for the location. </param>
        public GeoLocation(string name, double latitude, double longitude, double elevation, TimeZone timeZone)
        {
            setLocationName(name);
            setLatitude(latitude);
            setLongitude(longitude);
            setElevation(elevation);
            setTimeZone(timeZone);
        }


        ///	 <summary> Default GeoLocation constructor will set location to the Prime Meridian
        ///	at Greenwich, England and a TimeZone of GMT. The longitude will be set to
        ///	0 and the latitude will be 51.4772 to match the location of the <a
        ///	href="http://www.rog.nmm.ac.uk">Royal Observatory, Greenwich </a>. No
        ///	daylight savings time will be used. </summary>
        public GeoLocation()
        {
            setLocationName("Greenwich, England");
            setLongitude(0); // added for clarity
            setLatitude(51.4772);
            setTimeZone(TimeZone.getTimeZone("GMT"));
        }


        ///	 <summary> Method to set the latitude.
        ///	 </summary>
        ///	<param name="latitude">
        ///	           The degrees of latitude to set. The values should be between
        ///	           -90&deg; and 90&deg;. An IllegalArgumentException will be
        ///	           thrown if the value exceeds the limit. For example 40.095965
        ///	           would be used for Lakewood, NJ. <b>Note: </b> For latitudes south of the
        ///	           equator, a negative value should be used. </param>
        public virtual void setLatitude(double latitude)
        {
            if (latitude > 90 || latitude < -90)
            {
                throw new System.ArgumentException("Latitude must be between -90 and  90");
            }
            this.latitude = latitude;
        }


        ///	 <summary> Method to set the latitude in degrees, minutes and seconds.
        ///	 </summary>
        ///	<param name="degrees">
        ///	           The degrees of latitude to set between -90 and 90. An
        ///	           IllegalArgumentException will be thrown if the value exceeds
        ///	           the limit. For example 40 would be used for Lakewood, NJ. </param>
        ///	<param name="minutes"> <a href="http://en.wikipedia.org/wiki/Minute_of_arc#Cartography">minutes of arc</a> </param>
        ///	<param name="seconds"> <a href="http://en.wikipedia.org/wiki/Minute_of_arc#Cartography">seconds of arc</a> </param>
        ///	<param name="direction">
        ///	           N for north and S for south. An IllegalArgumentException will
        ///	           be thrown if the value is not S or N. </param>
        public virtual void setLatitude(int degrees, int minutes, double seconds, string direction)
        {
            double tempLat = degrees + ((minutes + (seconds / 60.0)) / 60.0);
            if (tempLat > 90 || tempLat < 0)
            {
                throw new System.ArgumentException("Latitude must be between 0 and  90. Use direction of S instead of negative.");
            }
            if (direction.Equals("S"))
            {
                tempLat *= -1;
            }
            else if (!direction.Equals("N"))
            {
                throw new System.ArgumentException("Latitude direction must be N or S");
            }
            this.latitude = tempLat;
        }


        ///	<returns> Returns the latitude. </returns>
        public virtual double getLatitude()
        {
            return latitude;
        }


        ///	 <summary> Method to set the longitude in a double format.
        ///	 </summary>
        ///	<param name="longitude">
        ///	           The degrees of longitude to set in a double format between
        ///	           -180&deg; and 180&deg;. An IllegalArgumentException will be
        ///	           thrown if the value exceeds the limit. For example -74.2094
        ///	           would be used for Lakewood, NJ. Note: for longitudes east of
        ///	           the <a href="http://en.wikipedia.org/wiki/Prime_Meridian">Prime
        ///	           Meridian</a> (Greenwich) a negative value should be used. </param>
        public virtual void setLongitude(double longitude)
        {
            if (longitude > 180 || longitude < -180)
            {
                throw new System.ArgumentException("Longitude must be between -180 and  180");
            }
            this.longitude = longitude;
        }


        ///	 <summary> Method to set the longitude in degrees, minutes and seconds.
        ///	 </summary>
        ///	<param name="degrees">
        ///	           The degrees of longitude to set between -180 and 180. An
        ///	           IllegalArgumentException will be thrown if the value exceeds
        ///	           the limit. For example -74 would be used for Lakewood, NJ.
        ///	           Note: for longitudes east of the <a
        ///	           href="http://en.wikipedia.org/wiki/Prime_Meridian">Prime
        ///	           Meridian </a> (Greenwich) a negative value should be used. </param>
        ///	<param name="minutes"> <a href="http://en.wikipedia.org/wiki/Minute_of_arc#Cartography">minutes of arc</a> </param>
        ///	<param name="seconds"> <a href="http://en.wikipedia.org/wiki/Minute_of_arc#Cartography">seconds of arc</a> </param>
        ///	<param name="direction">
        ///	           E for east of the Prime Meridian or W for west of it. An
        ///	           IllegalArgumentException will be thrown if the value is not E
        ///	           or W. </param>
        public virtual void setLongitude(int degrees, int minutes, double seconds, string direction)
        {
            double longTemp = degrees + ((minutes + (seconds / 60.0)) / 60.0);
            if (longTemp > 180 || longitude < 0)
            {
                throw new System.ArgumentException("Longitude must be between 0 and  180. Use the ");
            }
            if (direction.Equals("W"))
            {
                longTemp *= -1;
            }
            else if (!direction.Equals("E"))
            {
                throw new System.ArgumentException("Longitude direction must be E or W");
            }
            this.longitude = longTemp;
        }


        ///	<returns> Returns the longitude. </returns>
        public virtual double getLongitude()
        {
            return longitude;
        }


        ///	<returns> Returns the location name. </returns>
        public virtual string getLocationName()
        {
            return locationName;
        }


        ///	<param name="name">
        ///	           The setter method for the display name. </param>
        public virtual void setLocationName(string name)
        {
            this.locationName = name;
        }


        ///	<returns> Returns the timeZone. </returns>
        public virtual TimeZone getTimeZone()
        {
            return timeZone;
        }


        ///	 <summary> Method to set the TimeZone. If this is ever set after the GeoLocation is
        ///	set in the <seealso cref="net.sourceforge.zmanim.AstronomicalCalendar"/>, it is
        ///	critical that
        ///	<seealso cref="net.sourceforge.zmanim.AstronomicalCalendar#getCalendar()"/>.<seealso cref="java.util.Calendar#setTimeZone(TimeZone) setTimeZone(TimeZone)"/>
        ///	be called in order for the AstronomicalCalendar to output times in the
        ///	expected offset. This situation will arise if the AstronomicalCalendar is
        ///	ever <seealso cref="net.sourceforge.zmanim.AstronomicalCalendar#clone() cloned"/>.
        ///	 </summary>
        ///	<param name="timeZone">
        ///	           The timeZone to set. </param>
        public virtual void setTimeZone(TimeZone timeZone)
        {
            this.timeZone = timeZone;
        }


        ///	 <summary> A method that will return the location's local mean time offset in
        ///	milliseconds from local standard time. The globe is split into 360&deg;,
        ///	with 15&deg; per hour of the day. For a local that is at a longitude that
        ///	is evenly divisible by 15 (longitude % 15 == 0), at solar
        ///	<seealso cref="net.sourceforge.zmanim.AstronomicalCalendar#getSunTransit() noon"/>
        ///	(with adjustment for the <a
        ///	href="http://en.wikipedia.org/wiki/Equation_of_time">equation of time</a>)
        ///	the sun should be directly overhead, so a user who is 1&deg; west of this
        ///	will have noon at 4 minutes after standard time noon, and conversely, a
        ///	user who is 1&deg; east of the 15&deg longitude will have noon at 11:56
        ///	AM. The offset returned does not account for the <a
        ///	href="http://en.wikipedia.org/wiki/Daylight_saving_time">Daylight saving
        ///	time</a> offset since this class is unaware of dates.
        ///	 </summary>
        ///	<returns> the offset in milliseconds not accounting for Daylight saving
        ///	        time. A positive value will be returned East of the timezone
        ///	        line, and a negative value West of it.
        ///	@since 1.1 </returns>
        public virtual long getLocalMeanTimeOffset()
        {
            return (long)(getLongitude() * 4 * MINUTE_MILLIS - getTimeZone().getRawOffset());
        }


        ///	 <summary> Calculate the initial <a
        ///	href="http://en.wikipedia.org/wiki/Great_circle">geodesic</a> bearing
        ///	between this Object and a second Object passed to this method using <a
        ///	href="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ///	inverse formula See T Vincenty, "<a
        ///	href="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ///	Solutions of Geodesics on the Ellipsoid with application of nested
        ///	equations</a>", Survey Review, vol XXII no 176, 1975
        ///	 </summary>
        ///	<param name="location">
        ///	           the destination location </param>
        public virtual double getGeodesicInitialBearing(GeoLocation location)
        {
            return vincentyFormula(location, INITIAL_BEARING);
        }


        ///	 <summary> Calculate the final <a
        ///	href="http://en.wikipedia.org/wiki/Great_circle">geodesic</a> bearing
        ///	between this Object and a second Object passed to this method using <a
        ///	href="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ///	inverse formula See T Vincenty, "<a
        ///	href="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ///	Solutions of Geodesics on the Ellipsoid with application of nested
        ///	equations</a>", Survey Review, vol XXII no 176, 1975
        ///	 </summary>
        ///	<param name="location">
        ///	           the destination location </param>
        public virtual double getGeodesicFinalBearing(GeoLocation location)
        {
            return vincentyFormula(location, FINAL_BEARING);
        }


        ///	 <summary> Calculate <a
        ///	href="http://en.wikipedia.org/wiki/Great-circle_distance">geodesic
        ///	distance</a> in Meters between this Object and a second Object passed to
        ///	this method using <a
        ///	href="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ///	inverse formula See T Vincenty, "<a
        ///	href="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ///	Solutions of Geodesics on the Ellipsoid with application of nested
        ///	equations</a>", Survey Review, vol XXII no 176, 1975
        ///	 </summary>
        ///	<param name="location">
        ///	           the destination location </param>
        public virtual double getGeodesicDistance(GeoLocation location)
        {
            return vincentyFormula(location, DISTANCE);
        }


        ///	 <summary> Calculate <a
        ///	href="http://en.wikipedia.org/wiki/Great-circle_distance">geodesic
        ///	distance</a> in Meters between this Object and a second Object passed to
        ///	this method using <a
        ///	href="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ///	inverse formula See T Vincenty, "<a
        ///	href="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ///	Solutions of Geodesics on the Ellipsoid with application of nested
        ///	equations</a>", Survey Review, vol XXII no 176, 1975
        ///	 </summary>
        ///	<param name="location">
        ///	           the destination location </param>
        ///	<param name="formula">
        ///	           This formula calculates initial bearing (<seealso cref="#INITIAL_BEARING"/>),
        ///	           final bearing (<seealso cref="#FINAL_BEARING"/>) and distance (<seealso cref="#DISTANCE"/>). </param>
        private double vincentyFormula(GeoLocation location, int formula)
        {
            double a = 6378137;
            double b = 6356752.3142;
            double f = 1 / 298.257223563; // WGS-84 ellipsiod
            double L = java.lang.Math.toRadians(location.getLongitude() - getLongitude());
            double U1 = Math.Atan((1 - f) * Math.Tan(java.lang.Math.toRadians(getLatitude())));
            double U2 = Math.Atan((1 - f) * Math.Tan(java.lang.Math.toRadians(location.getLatitude())));
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
                sinSigma = Math.Sqrt((cosU2 * sinLambda) * (cosU2 * sinLambda) + (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda) * (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda));
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
                lambda = L + (1 - C) * f * sinAlpha * (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM)));
            }
            if (iterLimit == 0)
                return double.NaN; // formula failed to converge

            double uSq = cosSqAlpha * (a * a - b * b) / (b * b);
            double A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
            double B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));
            double deltaSigma = B * sinSigma * (cos2SigmaM + B / 4 * (cosSigma * (-1 + 2 * cos2SigmaM * cos2SigmaM) - B / 6 * cos2SigmaM * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2SigmaM * cos2SigmaM)));
            double distance = b * A * (sigma - deltaSigma);

            // initial bearing
            double fwdAz = java.lang.Math.toDegrees(Math.Atan2(cosU2 * sinLambda, cosU1 * sinU2 - sinU1 * cosU2 * cosLambda));
            // final bearing
            double revAz = java.lang.Math.toDegrees(Math.Atan2(cosU1 * sinLambda, -sinU1 * cosU2 + cosU1 * sinU2 * cosLambda));
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


        ///	 <summary> Returns the <a href="http://en.wikipedia.org/wiki/Rhumb_line">rhumb line</a>
        ///	bearing from the current location to the GeoLocation passed in.
        ///	 </summary>
        ///	<param name="location">
        ///	           destination location </param>
        ///	<returns> the bearing in degrees </returns>
        public virtual double getRhumbLineBearing(GeoLocation location)
        {
            double dLon = java.lang.Math.toRadians(location.getLongitude() - getLongitude());
            double dPhi = Math.Log(Math.Tan(java.lang.Math.toRadians(location.getLatitude()) / 2 + Math.PI / 4) / Math.Tan(java.lang.Math.toRadians(getLatitude()) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return java.lang.Math.toDegrees(Math.Atan2(dLon, dPhi));
        }


        ///	 <summary> Returns the <a href="http://en.wikipedia.org/wiki/Rhumb_line">rhumb line</a>
        ///	distance from the current location to the GeoLocation passed in.
        ///	 </summary>
        ///	<param name="location">
        ///	           the destination location </param>
        ///	<returns> the distance in Meters </returns>
        public virtual double getRhumbLineDistance(GeoLocation location)
        {
            double R = 6371; // earth's mean radius in km
            double dLat = java.lang.Math.toRadians(location.getLatitude() - getLatitude());
            double dLon = java.lang.Math.toRadians(Math.Abs(location.getLongitude() - getLongitude()));
            double dPhi = Math.Log(Math.Tan(java.lang.Math.toRadians(location.getLongitude()) / 2 + Math.PI / 4) / Math.Tan(java.lang.Math.toRadians(getLatitude()) / 2 + Math.PI / 4));
            double q = (Math.Abs(dLat) > 1e-10) ? dLat / dPhi : Math.Cos(java.lang.Math.toRadians(getLatitude()));
            // if dLon over 180° take shorter rhumb across 180° meridian:
            if (dLon > Math.PI)
                dLon = 2 * Math.PI - dLon;
            double d = Math.Sqrt(dLat * dLat + q * q * dLon * dLon);
            return d * R;
        }


        ///	 <summary> A method that returns an XML formatted <code>String</code> representing
        ///	the serialized <code>Object</code>. Very similar to the toString
        ///	method but the return value is in an xml format. The format currently
        ///	used (subject to change) is:
        ///	
        ///	<pre>
        ///	  &lt;GeoLocation&gt;
        ///	  	 &lt;LocationName&gt;Lakewood, NJ&lt;/LocationName&gt;
        ///	  	 &lt;Latitude&gt;40.0828&amp;deg&lt;/Latitude&gt;
        ///	  	 &lt;Longitude&gt;-74.2094&amp;deg&lt;/Longitude&gt;
        ///	  	 &lt;Elevation&gt;0 Meters&lt;/Elevation&gt;
        ///	  	 &lt;TimezoneName&gt;America/New_York&lt;/TimezoneName&gt;
        ///	  	 &lt;TimeZoneDisplayName&gt;Eastern Standard Time&lt;/TimeZoneDisplayName&gt;
        ///	  	 &lt;TimezoneGMTOffset&gt;-5&lt;/TimezoneGMTOffset&gt;
        ///	  	 &lt;TimezoneDSTOffset&gt;1&lt;/TimezoneDSTOffset&gt;
        ///	  &lt;/GeoLocation&gt;
        ///	</pre>
        ///	 </summary>
        ///	<returns> The XML formatted <code>String</code>. </returns>
        public virtual string toXML()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("<GeoLocation>\n");
            sb.Append("\t<LocationName>").Append(getLocationName()).Append("</LocationName>\n");
            sb.Append("\t<Latitude>").Append(getLatitude()).Append("&deg;").Append("</Latitude>\n");
            sb.Append("\t<Longitude>").Append(getLongitude()).Append("&deg;").Append("</Longitude>\n");
            sb.Append("\t<Elevation>").Append(getElevation()).Append(" Meters").Append("</Elevation>\n");
            sb.Append("\t<TimezoneName>").Append(getTimeZone().getID()).Append("</TimezoneName>\n");
            sb.Append("\t<TimeZoneDisplayName>").Append(getTimeZone().getDisplayName()).Append("</TimeZoneDisplayName>\n");
            sb.Append("\t<TimezoneGMTOffset>").Append(getTimeZone().getRawOffset() / HOUR_MILLIS).Append("</TimezoneGMTOffset>\n");
            sb.Append("\t<TimezoneDSTOffset>").Append(getTimeZone().getDSTSavings() / HOUR_MILLIS).Append("</TimezoneDSTOffset>\n");
            sb.Append("</GeoLocation>");
            return sb.ToString();
        }


        ///	<seealso cref="java.lang.Object#equals(Object)"/>
        public override bool Equals(object @object)
        {
            if (this == @object)
                return true;
            if (!(@object is GeoLocation))
                return false;
            GeoLocation geo = (GeoLocation)@object;
            return java.lang.Double.doubleToLongBits(latitude) == java.lang.Double.doubleToLongBits(geo.latitude) && java.lang.Double.doubleToLongBits(longitude) == java.lang.Double.doubleToLongBits(geo.longitude) && elevation == geo.elevation && (locationName == null ? geo.locationName == null : locationName.Equals(geo.locationName)) && (timeZone == null ? geo.timeZone == null : timeZone.Equals(geo.timeZone));
        }


        ///	<seealso cref="java.lang.Object#hashCode()"/>
        public override int GetHashCode()
        {
            int result = 17;
            long latLong = java.lang.Double.doubleToLongBits(latitude);
            long lonLong = java.lang.Double.doubleToLongBits(longitude);
            long elevLong = java.lang.Double.doubleToLongBits(elevation);
            int latInt = (int)(latLong ^ (latLong >> 32));
            int lonInt = (int)(lonLong ^ (lonLong >> 32));
            int elevInt = (int)(elevLong ^ (elevLong >> 32));
            result = 37 * result + this.GetType().GetHashCode();
            result += 37 * result + latInt;
            result += 37 * result + lonInt;
            result += 37 * result + elevInt;
            result += 37 * result + (locationName == null ? 0 : locationName.GetHashCode());
            result += 37 * result + (timeZone == null ? 0 : timeZone.GetHashCode());
            return result;
        }


        ///	<seealso cref="java.lang.Object#toString()"/>
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("\nLocation Name:\t\t\t").Append(getLocationName());
            sb.Append("\nLatitude:\t\t\t").Append(getLatitude()).Append("&deg;");
            sb.Append("\nLongitude:\t\t\t").Append(getLongitude()).Append("&deg;");
            sb.Append("\nElevation:\t\t\t").Append(getElevation()).Append(" Meters");
            sb.Append("\nTimezone Name:\t\t\t").Append(getTimeZone().getID());
            //        
            //		 * sb.append("\nTimezone Display Name:\t\t").append(
            //		 * getTimeZone().getDisplayName());
            //		 
            sb.Append("\nTimezone GMT Offset:\t\t").Append(getTimeZone().getRawOffset() / HOUR_MILLIS);
            sb.Append("\nTimezone DST Offset:\t\t").Append(getTimeZone().getDSTSavings() / HOUR_MILLIS);
            return sb.ToString();
        }


        ///	 <summary> An implementation of the <seealso cref="java.lang.Object#clone()"/> method that
        ///	creates a <a
        ///	href="http://en.wikipedia.org/wiki/Object_copy#Deep_copy">deep copy</a>
        ///	of the object. <br/><b>Note:</b> If the <seealso cref="java.util.TimeZone"/> in
        ///	the clone will be changed from the original, it is critical that
        ///	<seealso cref="net.sourceforge.zmanim.AstronomicalCalendar#getCalendar()"/>.<seealso cref="java.util.Calendar#setTimeZone(TimeZone) setTimeZone(TimeZone)"/>
        ///	is called after cloning in order for the AstronomicalCalendar to output
        ///	times in the expected offset.
        ///	 </summary>
        ///	<seealso cref= java.lang.Object#clone()
        ///	@since 1.1 </seealso>
        public virtual object Clone()
        {
            GeoLocation clone = (GeoLocation)MemberwiseClone();
            clone.timeZone = (TimeZone)getTimeZone().clone();
            clone.locationName = getLocationName();
            return clone;
        }

    }
}

