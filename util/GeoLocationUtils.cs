namespace net.sourceforge.zmanim.util
{
    using IKVM.Attributes;
    using java.lang;
    using System;
    using System.Runtime.CompilerServices;

    public class GeoLocationUtils
    {
        private static int DISTANCE = 0;
        private static int FINAL_BEARING = 2;
        private static int INITIAL_BEARING = 1;

        public static double getGeodesicDistance(GeoLocation location, GeoLocation destination)
        {
            return vincentyFormula(location, destination, DISTANCE);
        }

        public static double getGeodesicFinalBearing(GeoLocation location, GeoLocation destination)
        {
            return vincentyFormula(location, destination, FINAL_BEARING);
        }

        public static double getGeodesicInitialBearing(GeoLocation location, GeoLocation destination)
        {
            return vincentyFormula(location, destination, INITIAL_BEARING);
        }

        public static double getRhumbLineBearing(GeoLocation location, GeoLocation destination)
        {
            double a = java.lang.Math.toRadians(destination.getLongitude() - location.getLongitude());
            double x = java.lang.Math.log(java.lang.Math.tan((java.lang.Math.toRadians(destination.getLatitude()) / 2.0) + 0.78539816339744828) / java.lang.Math.tan((java.lang.Math.toRadians(location.getLatitude()) / 2.0) + 0.78539816339744828));
            if (java.lang.Math.abs(a) > 3.1415926535897931)
            {
                a = (a <= 0f) ? (6.2831853071795862 + a) : -(6.2831853071795862 - a);
            }
            return java.lang.Math.toDegrees(java.lang.Math.atan2(a, x));
        }

        public static double getRhumbLineDistance(GeoLocation location, GeoLocation destination)
        {
            double num = 6371.0;
            double a = java.lang.Math.toRadians(destination.getLatitude() - location.getLatitude());
            double num3 = java.lang.Math.toRadians(java.lang.Math.abs((double) (destination.getLongitude() - location.getLongitude())));
            double num4 = java.lang.Math.log(java.lang.Math.tan((java.lang.Math.toRadians(destination.getLongitude()) / 2.0) + 0.78539816339744828) / java.lang.Math.tan((java.lang.Math.toRadians(location.getLatitude()) / 2.0) + 0.78539816339744828));
            double num5 = (java.lang.Math.abs(a) <= 1E-10) ? java.lang.Math.cos(java.lang.Math.toRadians(location.getLatitude())) : (a / num4);
            if (num3 > 3.1415926535897931)
            {
                num3 = 6.2831853071795862 - num3;
            }
            double num6 = java.lang.Math.sqrt((a * a) + (((num5 * num5) * num3) * num3));
            return (num6 * num);
        }

        private static double vincentyFormula(GeoLocation location2, GeoLocation location1, int num30)
        {
            double num = 6378137.0;
            double num2 = 6356752.3142;
            double num3 = 0.0033528106647474805;
            double num4 = java.lang.Math.toRadians(location1.getLongitude() - location2.getLongitude());
            double a = java.lang.Math.atan((1f - num3) * java.lang.Math.tan(java.lang.Math.toRadians(location2.getLatitude())));
            double num6 = java.lang.Math.atan((1f - num3) * java.lang.Math.tan(java.lang.Math.toRadians(location1.getLatitude())));
            double num7 = java.lang.Math.sin(a);
            double num8 = java.lang.Math.cos(a);
            double num9 = java.lang.Math.sin(num6);
            double num10 = java.lang.Math.cos(num6);
            double num11 = num4;
            double num12 = 6.2831853071795862;
            double num13 = 20.0;
            double num14 = 0f;
            double num15 = 0f;
            double y = 0f;
            double x = 0f;
            double num18 = 0f;
            double num19 = 0f;
            double v = 0f;
            while (true)
            {
                if (java.lang.Math.abs((double) (num11 - num12)) <= 1E-12)
                {
                    break;
                }
                double num1 = num13 - 1f;
                num13 = num1;
                if (num1 <= 0f)
                {
                    break;
                }
                num14 = java.lang.Math.sin(num11);
                num15 = java.lang.Math.cos(num11);
                y = java.lang.Math.sqrt(((num10 * num14) * (num10 * num14)) + (((num8 * num9) - ((num7 * num10) * num15)) * ((num8 * num9) - ((num7 * num10) * num15))));
                if (y == 0f)
                {
                    return 0f;
                }
                x = (num7 * num9) + ((num8 * num10) * num15);
                num18 = java.lang.Math.atan2(y, x);
                double num21 = ((num8 * num10) * num14) / y;
                num19 = 1f - (num21 * num21);
                v = x - (((2.0 * num7) * num9) / num19);
                if (java.lang.Double.isNaN(v))
                {
                    v = 0f;
                }
                double num22 = ((num3 / 16.0) * num19) * (4.0 + (num3 * (4.0 - (3.0 * num19))));
                num12 = num11;
                num11 = num4 + ((((1f - num22) * num3) * num21) * (num18 + ((num22 * y) * (v + ((num22 * x) * (-1.0 + ((2.0 * v) * v)))))));
            }
            if (num13 != 0f)
            {
                double num23 = (num19 * ((num * num) - (num2 * num2))) / (num2 * num2);
                double num24 = 1f + ((num23 / 16384.0) * (4096.0 + (num23 * (-768.0 + (num23 * (320.0 - (175.0 * num23)))))));
                double num25 = (num23 / 1024.0) * (256.0 + (num23 * (-128.0 + (num23 * (74.0 - (47.0 * num23))))));
                double num26 = (num25 * y) * (v + ((num25 / 4.0) * ((x * (-1.0 + ((2.0 * v) * v))) - ((((num25 / 6.0) * v) * (-3.0 + ((4.0 * y) * y))) * (-3.0 + ((4.0 * v) * v))))));
                double num27 = (num2 * num24) * (num18 - num26);
                double num28 = java.lang.Math.toDegrees(java.lang.Math.atan2(num10 * num14, (num8 * num9) - ((num7 * num10) * num15)));
                double num29 = java.lang.Math.toDegrees(java.lang.Math.atan2(num8 * num14, (-num7 * num10) + ((num8 * num9) * num15)));
                if (num30 == DISTANCE)
                {
                    return num27;
                }
                if (num30 == INITIAL_BEARING)
                {
                    return num28;
                }
                if (num30 == FINAL_BEARING)
                {
                    return num29;
                }
            }
            return double.NaN;
        }
    }
}

