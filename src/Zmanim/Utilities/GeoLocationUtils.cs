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
using Zmanim.Extensions;

namespace Zmanim.Utilities
{
    /// <summary>
    ///   A class for various location calculations
    ///   Most of the code in this class is ported from <a href = "http://www.movable-type.co.uk/">Chris Veness'</a>
    ///   <a href = "http://www.fsf.org/licensing/licenses/lgpl.html">LGPL</a> Javascript Implementation
    /// </summary>
    /// <author>Eliyahu Hershfeld</author>
    public class GeoLocationUtils
    {
        private static int DISTANCE;
        private static int INITIAL_BEARING = 1;
        private static int FINAL_BEARING = 2;

        /// <summary>
        /// Calculate the initial <a href="http://en.wikipedia.org/wiki/Great_circle">geodesic</a> bearing
        /// between this Object and a second Object passed to this method using <a href="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        /// inverse formula See T Vincenty, "<a href="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        /// Solutions of Geodesics on the Ellipsoid with application of nested
        /// equations</a>", Survey Review, vol XXII no 176, 1975.
        /// </summary>
        /// <param name="location">the destination location</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        public static double GetGeodesicInitialBearing(GeoLocation location, GeoLocation destination)
        {
            return VincentyFormula(location, destination, INITIAL_BEARING);
        }

        /// <summary>
        /// Calculate the final <a href="http://en.wikipedia.org/wiki/Great_circle">geodesic</a> bearing
        /// between this Object and a second Object passed to this method using <a href="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        /// inverse formula See T Vincenty, "<a href="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        /// Solutions of Geodesics on the Ellipsoid with application of nested
        /// equations</a>", Survey Review, vol XXII no 176, 1975.
        /// </summary>
        /// <param name="location">the destination location</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        public static double GetGeodesicFinalBearing(GeoLocation location, GeoLocation destination)
        {
            return VincentyFormula(location, destination, FINAL_BEARING);
        }

        /// <summary>
        /// Calculate <a href="http://en.wikipedia.org/wiki/Great-circle_distance">geodesic
        /// distance</a> in Meters between this Object and a second Object passed to
        /// this method using <a href="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        /// inverse formula See T Vincenty, "<a href="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        /// Solutions of Geodesics on the Ellipsoid with application of nested
        /// equations</a>", Survey Review, vol XXII no 176, 1975.
        /// </summary>
        /// <param name="location">the destination location</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        public static double GetGeodesicDistance(GeoLocation location, GeoLocation destination)
        {
            return VincentyFormula(location, destination, DISTANCE);
        }

        /// <summary>
        /// Calculate <a href="http://en.wikipedia.org/wiki/Great-circle_distance">geodesic
        /// distance</a> in Meters between this Object and a second Object passed to
        /// this method using <a href="http://en.wikipedia.org/wiki/Thaddeus_Vincenty">Thaddeus Vincenty's</a>
        /// inverse formula See T Vincenty, "<a href="http://www.ngs.noaa.gov/PUBS_LIB/inverse.pdf">Direct and Inverse
        /// Solutions of Geodesics on the Ellipsoid with application of nested
        /// equations</a>", Survey Review, vol XXII no 176, 1975.
        /// </summary>
        /// <param name="location">the destination location</param>
        /// <param name="destination">The destination.</param>
        /// <param name="formula">This formula calculates initial bearing (<seealso cref="INITIAL_BEARING"/>),
        /// final bearing (<seealso cref="FINAL_BEARING"/>) and distance (<seealso cref="DISTANCE"/>).</param>
        /// <returns></returns>
        private static double VincentyFormula(GeoLocation location, GeoLocation destination, int formula)
        {
            double a = 6378137;
            double b = 6356752.3142;
            double f = 1/298.257223563; // WGS-84 ellipsiod
            double L = MathExtensions.ToRadians(destination.Longitude - location.Longitude);
            double U1 = Math.Atan((1 - f)*Math.Tan(MathExtensions.ToRadians(location.Latitude)));
            double U2 = Math.Atan((1 - f)*Math.Tan(MathExtensions.ToRadians(destination.Latitude)));
            double sinU1 = Math.Sin(U1), cosU1 = Math.Cos(U1);
            double sinU2 = Math.Sin(U2), cosU2 = Math.Cos(U2);

            double lambda = L;
            double lambdaP = 2*Math.PI;
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
                    Math.Sqrt((cosU2*sinLambda)*(cosU2*sinLambda) +
                                     (cosU1*sinU2 - sinU1*cosU2*cosLambda)*(cosU1*sinU2 - sinU1*cosU2*cosLambda));
                if (sinSigma == 0)
                    return 0; // co-incident points
                cosSigma = sinU1*sinU2 + cosU1*cosU2*cosLambda;
                sigma = Math.Atan2(sinSigma, cosSigma);
                sinAlpha = cosU1*cosU2*sinLambda/sinSigma;
                cosSqAlpha = 1 - sinAlpha*sinAlpha;
                cos2SigmaM = cosSigma - 2*sinU1*sinU2/cosSqAlpha;
                if (double.IsNaN(cos2SigmaM))
                    cos2SigmaM = 0; // equatorial line: cosSqAlpha=0 (§6)
                C = f/16*cosSqAlpha*(4 + f*(4 - 3*cosSqAlpha));
                lambdaP = lambda;
                lambda = L +
                         (1 - C)*f*sinAlpha*
                         (sigma + C*sinSigma*(cos2SigmaM + C*cosSigma*(-1 + 2*cos2SigmaM*cos2SigmaM)));
            }
            if (iterLimit == 0)
                return double.NaN; // formula failed to converge

            double uSq = cosSqAlpha*(a*a - b*b)/(b*b);
            double A = 1 + uSq/16384*(4096 + uSq*(-768 + uSq*(320 - 175*uSq)));
            double B = uSq/1024*(256 + uSq*(-128 + uSq*(74 - 47*uSq)));
            double deltaSigma = B*sinSigma*
                                (cos2SigmaM +
                                 B/4*
                                 (cosSigma*(-1 + 2*cos2SigmaM*cos2SigmaM) -
                                  B/6*cos2SigmaM*(-3 + 4*sinSigma*sinSigma)*(-3 + 4*cos2SigmaM*cos2SigmaM)));
            double distance = b*A*(sigma - deltaSigma);

            // initial bearing
            double fwdAz = MathExtensions.ToDegree(Math.Atan2(cosU2*sinLambda, cosU1*sinU2 - sinU1*cosU2*cosLambda));
            // final bearing
            double revAz = MathExtensions.ToDegree(Math.Atan2(cosU1*sinLambda, -sinU1*cosU2 + cosU1*sinU2*cosLambda));
            if (formula == DISTANCE)
                return distance;
            if (formula == INITIAL_BEARING)
                return fwdAz;
            if (formula == FINAL_BEARING)
                return revAz;
            // should never happpen

            return double.NaN;
        }

        /// <summary>
        /// Returns the <a href="http://en.wikipedia.org/wiki/Rhumb_line">rhumb line</a>
        /// bearing from the current location to the GeoLocation passed in.
        /// </summary>
        /// <param name="location">destination location</param>
        /// <param name="destination">The destination.</param>
        /// <returns>the bearing in degrees</returns>
        public static double GetRhumbLineBearing(GeoLocation location, GeoLocation destination)
        {
            double dLon = MathExtensions.ToRadians(destination.Longitude - location.Longitude);
            double dPhi =
                Math.Log(Math.Tan(MathExtensions.ToRadians(destination.Latitude)/2 + Math.PI/4)/
                                Math.Tan(MathExtensions.ToRadians(location.Latitude)/2 + Math.PI/4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2*Math.PI - dLon) : (2*Math.PI + dLon);
            return MathExtensions.ToDegree(Math.Atan2(dLon, dPhi));
        }

        /// <summary>
        /// Returns the <a href="http://en.wikipedia.org/wiki/Rhumb_line">rhumb line</a>
        /// distance from the current location to the GeoLocation passed in.
        /// Ported from <a href="http://www.movable-type.co.uk/">Chris Veness'</a> Javascript Implementation
        /// </summary>
        /// <param name="location">the destination location</param>
        /// <param name="destination">The destination.</param>
        /// <returns>the distance in Meters</returns>
        public static double GetRhumbLineDistance(GeoLocation location, GeoLocation destination)
        {
            double R = 6371; // earth's mean radius in km
            double dLat = MathExtensions.ToRadians(destination.Latitude - location.Latitude);
            double dLon = MathExtensions.ToRadians(Math.Abs(destination.Longitude - location.Longitude));
            double dPhi =
                Math.Log(Math.Tan(MathExtensions.ToRadians(destination.Longitude)/2 + Math.PI/4)/
                                Math.Tan(MathExtensions.ToRadians(location.Latitude)/2 + Math.PI/4));
            double q = (Math.Abs(dLat) > 1e-10)
                           ? dLat/dPhi
                           : Math.Cos(MathExtensions.ToRadians(location.Latitude));
            // if dLon over 180° take shorter rhumb across 180° meridian:
            if (dLon > Math.PI)
                dLon = 2*Math.PI - dLon;
            double d = Math.Sqrt(dLat*dLat + q*q*dLon*dLon);
            return d*R;
        }
    }
}