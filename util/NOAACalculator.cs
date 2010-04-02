namespace net.sourceforge.zmanim.util
{
    using IKVM.Attributes;
    using java.lang;
    using java.util;
    using net.sourceforge.zmanim;
    using System;
    using System.Runtime.CompilerServices;

    public class NOAACalculator : AstronomicalCalculator
    {
        private string calculatorName = "US National Oceanic and Atmospheric Administration Algorithm";

        [LineNumberTable(new byte[] { 160, 0x47, 0x7f, 6 })]
        private static double calcEccentricityEarthOrbit(double num1)
        {
            return (0.016708634 - (num1 * (4.2037E-05 + (1.267E-07 * num1))));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0xc3, 0x69, 0x69, 0x69, 0x89, 120, 0x88, 120, 110, 120, 120, 0x98, 0x9f, 0x31 })]
        private static double calcEquationOfTime(double num1)
        {
            double angdeg = calcObliquityCorrection(num1);
            double num2 = calcGeomMeanLongSun(num1);
            double num3 = calcEccentricityEarthOrbit(num1);
            double num4 = calcGeomMeanAnomalySun(num1);
            double num5 = java.lang.Math.tan(java.lang.Math.toRadians(angdeg) / 2.0);
            num5 *= num5;
            double num6 = java.lang.Math.sin(2.0 * java.lang.Math.toRadians(num2));
            double num7 = java.lang.Math.sin(java.lang.Math.toRadians(num4));
            double num8 = java.lang.Math.cos(2.0 * java.lang.Math.toRadians(num2));
            double num9 = java.lang.Math.sin(4.0 * java.lang.Math.toRadians(num2));
            double num10 = java.lang.Math.sin(2.0 * java.lang.Math.toRadians(num4));
            double angrad = ((((num5 * num6) - ((2.0 * num3) * num7)) + ((((4.0 * num3) * num5) * num7) * num8)) - (((0.5 * num5) * num5) * num9)) - (((1.25 * num3) * num3) * num10);
            return (java.lang.Math.toDegrees(angrad) * 4.0);
        }

        [LineNumberTable(new byte[] { 0x7b, 0x7f, 6 })]
        private static double calcGeomMeanAnomalySun(double num1)
        {
            return (357.52911 + (num1 * (35999.05029 - (0.0001537 * num1))));
        }

        [LineNumberTable(new byte[] { 0x68, 0x7f, 6, 0x6f, 0x8f, 0x6b, 0xaf })]
        private static double calcGeomMeanLongSun(double num1)
        {
            double num = 280.46646 + (num1 * (36000.76983 + (0.0003032 * num1)));
            while (true)
            {
                if (num <= 360.0)
                {
                    break;
                }
                num -= 360.0;
            }
            while (num < 0f)
            {
                num += 360.0;
            }
            return num;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0xe1, 0x69, 0xe9, 70, 0xbf, 0x10 })]
        private static double calcHourAngleSunrise(double num1, double num4, double num5)
        {
            double a = java.lang.Math.toRadians(num1);
            double num2 = java.lang.Math.toRadians(num4);
            return java.lang.Math.acos((java.lang.Math.cos(java.lang.Math.toRadians(num5)) / (java.lang.Math.cos(a) * java.lang.Math.cos(num2))) - (java.lang.Math.tan(a) * java.lang.Math.tan(num2)));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 250, 0x69, 0xe9, 70, 0xbf, 0x10 })]
        private static double calcHourAngleSunset(double num1, double num4, double num5)
        {
            double a = java.lang.Math.toRadians(num1);
            double num2 = java.lang.Math.toRadians(num4);
            double num3 = java.lang.Math.acos((java.lang.Math.cos(java.lang.Math.toRadians(num5)) / (java.lang.Math.cos(a) * java.lang.Math.cos(num2))) - (java.lang.Math.tan(a) * java.lang.Math.tan(num2)));
            return -num3;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 60, 0x68, 0x6a, 0x68, 0x67, 100, 0x85, 0x6c, 0x9f, 0 })]
        private static double calcJD(Calendar calendar1)
        {
            int num = calendar1.get(1);
            int num2 = calendar1.get(2) + 1;
            int num3 = calendar1.get(5);
            if (num2 <= 2)
            {
                num += -1;
                num2 += 12;
            }
            double num4 = java.lang.Math.floor((double) (num / 100));
            double num5 = (2.0 - num4) + java.lang.Math.floor(num4 / 4.0);
            return ((((java.lang.Math.floor(365.25 * (num + 0x126c)) + java.lang.Math.floor(30.6001 * (num2 + 1))) + num3) + num5) - 1524.5);
        }

        private static double calcJDFromJulianCent(double num1)
        {
            return ((num1 * 36525.0) + 2451545.0);
        }

        [LineNumberTable(new byte[] { 160, 0x94, 0x9f, 0x13, 0x7f, 12 })]
        private static double calcMeanObliquityOfEcliptic(double num1)
        {
            double num = 21.448 - (num1 * (46.815 + (num1 * (0.00059 - (num1 * 0.001813)))));
            return (23.0 + ((26.0 + (num / 60.0)) / 60.0));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0xa2, 0x89, 120, 0x79 })]
        private static double calcObliquityCorrection(double num1)
        {
            double num = calcMeanObliquityOfEcliptic(num1);
            double angdeg = 125.04 - (1934.136 * num1);
            return (num + (0.00256 * java.lang.Math.cos(java.lang.Math.toRadians(angdeg))));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa1, 0x41, 0x9b, 0x68, 0x9a, 0xbf, 5, 0x68 })]
        private static double calcSolNoonUTC(double num1, double num5)
        {
            double num = calcTimeJulianCent(calcJDFromJulianCent(num1) + (num5 / 360.0));
            double num2 = calcEquationOfTime(num);
            double num3 = (720.0 + (num5 * 4.0)) - num2;
            double num4 = calcTimeJulianCent((calcJDFromJulianCent(num1) - 0.5) + (num3 / 1440.0));
            num2 = calcEquationOfTime(num4);
            return ((720.0 + (num5 * 4.0)) - num2);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0x85, 0x89, 120, 0x7f, 4 })]
        private static double calcSunApparentLong(double num1)
        {
            double num = calcSunTrueLong(num1);
            double angdeg = 125.04 - (1934.136 * num1);
            return ((num - 0.00569) - (0.00478 * java.lang.Math.sin(java.lang.Math.toRadians(angdeg))));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0xb2, 0x69, 0x89, 0x99, 0x6d })]
        private static double calcSunDeclination(double num1)
        {
            double angdeg = calcObliquityCorrection(num1);
            double num2 = calcSunApparentLong(num1);
            double a = java.lang.Math.sin(java.lang.Math.toRadians(angdeg)) * java.lang.Math.sin(java.lang.Math.toRadians(num2));
            return java.lang.Math.toDegrees(java.lang.Math.asin(a));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0x53, 0x89, 0x68, 0x68, 0x6a, 0x8d, 0x9f, 0x2f })]
        private static double calcSunEqOfCenter(double num1)
        {
            double angdeg = calcGeomMeanAnomalySun(num1);
            double a = java.lang.Math.toRadians(angdeg);
            double num3 = java.lang.Math.sin(a);
            double num4 = java.lang.Math.sin(a + a);
            double num5 = java.lang.Math.sin((a + a) + a);
            return (((num3 * (1.914602 - (num1 * (0.004817 + (1.4E-05 * num1))))) + (num4 * (0.019993 - (0.000101 * num1)))) + (num5 * 0.000289));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 
            0xa1, 0x15, 0xe9, 70, 0x6a, 0xd5, 0x68, 0x69, 0x8e, 0x6d, 0x6f, 0xd1, 0x9b, 0x69, 0x6a, 110, 
            0x6d, 0x6f, 0x71
         })]
        private static double calcSunriseUTC(double num1, double num12, double num11, double num13)
        {
            double num = calcTimeJulianCent(num1);
            double num2 = calcSolNoonUTC(num, num11);
            double num3 = calcTimeJulianCent(num1 + (num2 / 1440.0));
            double num4 = calcEquationOfTime(num3);
            double num5 = calcSunDeclination(num3);
            double angrad = calcHourAngleSunrise(num12, num5, num13);
            double num7 = num11 - java.lang.Math.toDegrees(angrad);
            double num8 = 4.0 * num7;
            double num9 = (720.0 + num8) - num4;
            double num10 = calcTimeJulianCent(calcJDFromJulianCent(num) + (num9 / 1440.0));
            num4 = calcEquationOfTime(num10);
            num5 = calcSunDeclination(num10);
            angrad = calcHourAngleSunrise(num12, num5, num13);
            num7 = num11 - java.lang.Math.toDegrees(angrad);
            num8 = 4.0 * num7;
            return ((720.0 + num8) - num4);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 
            0xa1, 0x5c, 0xe9, 70, 0x6a, 0xd5, 0x68, 0x69, 0x8e, 0x6d, 0x6f, 0xd1, 0x9b, 0x69, 0x6a, 0x8e, 
            0x6d, 0x6f
         })]
        private static double calcSunsetUTC(double num1, double num12, double num11, double num13)
        {
            double num = calcTimeJulianCent(num1);
            double num2 = calcSolNoonUTC(num, num11);
            double num3 = calcTimeJulianCent(num1 + (num2 / 1440.0));
            double num4 = calcEquationOfTime(num3);
            double num5 = calcSunDeclination(num3);
            double angrad = calcHourAngleSunset(num12, num5, num13);
            double num7 = num11 - java.lang.Math.toDegrees(angrad);
            double num8 = 4.0 * num7;
            double num9 = (720.0 + num8) - num4;
            double num10 = calcTimeJulianCent(calcJDFromJulianCent(num) + (num9 / 1440.0));
            num4 = calcEquationOfTime(num10);
            num5 = calcSunDeclination(num10);
            angrad = calcHourAngleSunset(num12, num5, num13);
            num7 = num11 - java.lang.Math.toDegrees(angrad);
            num8 = 4.0 * num7;
            return ((720.0 + num8) - num4);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0x67, 0x69, 0x89, 0x65 })]
        private static double calcSunTrueLong(double num1)
        {
            double num = calcGeomMeanLongSun(num1);
            double num2 = calcSunEqOfCenter(num1);
            return (num + num2);
        }

        private static double calcTimeJulianCent(double num1)
        {
            return ((num1 - 2451545.0) / 36525.0);
        }

        public override string getCalculatorName()
        {
            return this.calculatorName;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0x9f, 0x80, 0x43, 0x66, 0xbb, 0xb0, 0xdf, 12 })]
        public override double getUTCSunrise(AstronomicalCalendar astronomicalCalendar, double zenith, bool adjustForElevation)
        {
            if (adjustForElevation)
            {
                zenith = this.adjustZenith(zenith, astronomicalCalendar.getGeoLocation().getElevation());
            }
            else
            {
                zenith = this.adjustZenith(zenith, 0f);
            }
            double num2 = calcSunriseUTC(calcJD(astronomicalCalendar.getCalendar()), astronomicalCalendar.getGeoLocation().getLatitude(), -astronomicalCalendar.getGeoLocation().getLongitude(), zenith);
            return (num2 / 60.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0x9f, 0x79, 0xa3, 0x66, 0xbb, 0xb0, 0xdf, 12 })]
        public override double getUTCSunset(AstronomicalCalendar astronomicalCalendar, double zenith, bool adjustForElevation)
        {
            if (adjustForElevation)
            {
                zenith = this.adjustZenith(zenith, astronomicalCalendar.getGeoLocation().getElevation());
            }
            else
            {
                zenith = this.adjustZenith(zenith, 0f);
            }
            double num2 = calcSunsetUTC(calcJD(astronomicalCalendar.getCalendar()), astronomicalCalendar.getGeoLocation().getLatitude(), -astronomicalCalendar.getGeoLocation().getLongitude(), zenith);
            return (num2 / 60.0);
        }
    }
}

