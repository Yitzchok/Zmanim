using System;

namespace net.sourceforge.zmanim.util
{
    /// <summary>
    /// A class for various location calculations
    /// Most of the code in this class is ported from <a href="http://www.movable-type.co.uk/">Chris Veness'</a>
    /// <a href="http://www.fsf.org/licensing/licenses/lgpl.html">LGPL</a> Javascript Implementation
    ///
    /// @author &copy; Eliyahu Hershfeld 2009
    /// @version 0.1 </summary>
    public class GeoLocationUtils
    {
        private static int DISTANCE = 0;
        private static int INITIAL_BEARING = 1;
        private static int FINAL_BEARING = 2;

        ///	 <summary>
        /// Calculate the initial <a href="http://en.wikipedia.org/wiki/Great_circle">geodesic</a> bearing
        ///	between this Object and a second Object passed to this method using <a
        ///	href="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ///	inverse formula See T Vincenty, "<a
        ///	href="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ///	Solutions of Geodesics on the Ellipsoid with application of nested
        ///	equations</a>", Survey Review, vol XXII no 176, 1975.
        ///	</summary>
        ///	<param name="location">
        ///	           the destination location </param>
        public static double getGeodesicInitialBearing(GeoLocation location, GeoLocation destination)
        {
            return vincentyFormula(location, destination, INITIAL_BEARING);
        }
  
        ///	 <summary>
        ///  Calculate the final <a
        ///	 href="http://en.wikipedia.org/wiki/Great_circle">geodesic</a> bearing
        ///	 between this Object and a second Object passed to this method using <a
        ///	 href="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ///	 inverse formula See T Vincenty, "<a
        ///	 href="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ///	 Solutions of Geodesics on the Ellipsoid with application of nested
        ///	 equations</a>", Survey Review, vol XXII no 176, 1975.
        ///	 </summary>
        ///	 <param name="location">
        ///	            the destination location </param>
        public static double getGeodesicFinalBearing(GeoLocation location, GeoLocation destination)
        {
            return vincentyFormula(location, destination, FINAL_BEARING);
        }
 
        ///	 <summary>
        /// Calculate <a
        ///	href="http://en.wikipedia.org/wiki/Great-circle_distance">geodesic
        ///	distance</a> in Meters between this Object and a second Object passed to
        ///	this method using <a
        ///	href="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ///	inverse formula See T Vincenty, "<a
        ///	href="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ///	Solutions of Geodesics on the Ellipsoid with application of nested
        ///	equations</a>", Survey Review, vol XXII no 176, 1975.
        ///	</summary>
        ///	<param name="location">
        ///	           the destination location </param>
        public static double getGeodesicDistance(GeoLocation location, GeoLocation destination)
        {
            return vincentyFormula(location, destination, DISTANCE);
        }
  
        ///	 <summary>
        /// Calculate <a
        ///	href="http://en.wikipedia.org/wiki/Great-circle_distance">geodesic
        ///	distance</a> in Meters between this Object and a second Object passed to
        ///	this method using <a
        ///	href="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        ///	inverse formula See T Vincenty, "<a
        ///	href="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        ///	Solutions of Geodesics on the Ellipsoid with application of nested
        ///	equations</a>", Survey Review, vol XXII no 176, 1975.
        ///	</summary>
        ///	<param name="location">
        ///	           the destination location </param>
        ///	<param name="formula">
        ///	           This formula calculates initial bearing (<seealso cref="#INITIAL_BEARING"/>),
        ///	           final bearing (<seealso cref="#FINAL_BEARING"/>) and distance (<seealso cref="#DISTANCE"/>). </param>
        private static double vincentyFormula(GeoLocation location, GeoLocation destination, int formula)
        {
            double a = 6378137;
            double b = 6356752.3142;
            double f = 1 / 298.257223563; // WGS-84 ellipsiod
            double L = java.lang.Math.toRadians(destination.getLongitude() - location.getLongitude());
            double U1 = Math.Atan((1 - f) * Math.Tan(java.lang.Math.toRadians(location.getLatitude())));
            double U2 = Math.Atan((1 - f) * Math.Tan(java.lang.Math.toRadians(destination.getLatitude())));
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
                return distance;
            if (formula == INITIAL_BEARING)
                return fwdAz;
            if (formula == FINAL_BEARING)
                return revAz;
            // should never happpen

            return double.NaN;
        }

        ///    
        ///	 <summary> * Returns the <a href="http://en.wikipedia.org/wiki/Rhumb_line">rhumb line</a>
        ///	 * bearing from the current location to the GeoLocation passed in.
        ///	 * </summary>
        ///	 * <param name="location">
        ///	 *            destination location </param>
        ///	 * <returns> the bearing in degrees </returns>
        ///	 
        public static double getRhumbLineBearing(GeoLocation location, GeoLocation destination)
        {
            double dLon = java.lang.Math.toRadians(destination.getLongitude() - location.getLongitude());
            double dPhi = Math.Log(Math.Tan(java.lang.Math.toRadians(destination.getLatitude()) / 2 + Math.PI / 4) / Math.Tan(java.lang.Math.toRadians(location.getLatitude()) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return java.lang.Math.toDegrees(Math.Atan2(dLon, dPhi));
        }

        ///	 <summary>
        /// Returns the <a href="http://en.wikipedia.org/wiki/Rhumb_line">rhumb line</a>
        /// distance from the current location to the GeoLocation passed in.
        /// Ported from <a href="http://www.movable-type.co.uk/">Chris Veness'</a> Javascript Implementation
        /// </summary>
        /// <param name="location">
        ///            the destination location </param>
        /// <returns> the distance in Meters </returns>
        public static double getRhumbLineDistance(GeoLocation location, GeoLocation destination)
        {
            double R = 6371; // earth's mean radius in km
            double dLat = java.lang.Math.toRadians(destination.getLatitude() - location.getLatitude());
            double dLon = java.lang.Math.toRadians(Math.Abs(destination.getLongitude() - location.getLongitude()));
            double dPhi = Math.Log(Math.Tan(java.lang.Math.toRadians(destination.getLongitude()) / 2 + Math.PI / 4) / Math.Tan(java.lang.Math.toRadians(location.getLatitude()) / 2 + Math.PI / 4));
            double q = (Math.Abs(dLat) > 1e-10) ? dLat / dPhi : Math.Cos(java.lang.Math.toRadians(location.getLatitude()));
            // if dLon over 180° take shorter rhumb across 180° meridian:
            if (dLon > Math.PI)
                dLon = 2 * Math.PI - dLon;
            double d = Math.Sqrt(dLat * dLat + q * q * dLon * dLon);
            return d * R;
        }
    }
}

