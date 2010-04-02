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

        private static double calcEccentricityEarthOrbit(double num1)
        {
            return (0.016708634 - (num1 * (4.2037E-05 + (1.267E-07 * num1))));
        }

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

        private static double calcGeomMeanAnomalySun(double num1)
        {
            return (357.52911 + (num1 * (35999.05029 - (0.0001537 * num1))));
        }

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

        private static double calcHourAngleSunrise(double num1, double num4, double num5)
        {
            double a = java.lang.Math.toRadians(num1);
            double num2 = java.lang.Math.toRadians(num4);
            return java.lang.Math.acos((java.lang.Math.cos(java.lang.Math.toRadians(num5)) / (java.lang.Math.cos(a) * java.lang.Math.cos(num2))) - (java.lang.Math.tan(a) * java.lang.Math.tan(num2)));
        }

        private static double calcHourAngleSunset(double num1, double num4, double num5)
        {
            double a = java.lang.Math.toRadians(num1);
            double num2 = java.lang.Math.toRadians(num4);
            double num3 = java.lang.Math.acos((java.lang.Math.cos(java.lang.Math.toRadians(num5)) / (java.lang.Math.cos(a) * java.lang.Math.cos(num2))) - (java.lang.Math.tan(a) * java.lang.Math.tan(num2)));
            return -num3;
        }

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

        private static double calcMeanObliquityOfEcliptic(double num1)
        {
            double num = 21.448 - (num1 * (46.815 + (num1 * (0.00059 - (num1 * 0.001813)))));
            return (23.0 + ((26.0 + (num / 60.0)) / 60.0));
        }

        private static double calcObliquityCorrection(double num1)
        {
            double num = calcMeanObliquityOfEcliptic(num1);
            double angdeg = 125.04 - (1934.136 * num1);
            return (num + (0.00256 * java.lang.Math.cos(java.lang.Math.toRadians(angdeg))));
        }

        private static double calcSolNoonUTC(double num1, double num5)
        {
            double num = calcTimeJulianCent(calcJDFromJulianCent(num1) + (num5 / 360.0));
            double num2 = calcEquationOfTime(num);
            double num3 = (720.0 + (num5 * 4.0)) - num2;
            double num4 = calcTimeJulianCent((calcJDFromJulianCent(num1) - 0.5) + (num3 / 1440.0));
            num2 = calcEquationOfTime(num4);
            return ((720.0 + (num5 * 4.0)) - num2);
        }

        private static double calcSunApparentLong(double num1)
        {
            double num = calcSunTrueLong(num1);
            double angdeg = 125.04 - (1934.136 * num1);
            return ((num - 0.00569) - (0.00478 * java.lang.Math.sin(java.lang.Math.toRadians(angdeg))));
        }

        private static double calcSunDeclination(double num1)
        {
            double angdeg = calcObliquityCorrection(num1);
            double num2 = calcSunApparentLong(num1);
            double a = java.lang.Math.sin(java.lang.Math.toRadians(angdeg)) * java.lang.Math.sin(java.lang.Math.toRadians(num2));
            return java.lang.Math.toDegrees(java.lang.Math.asin(a));
        }

        private static double calcSunEqOfCenter(double num1)
        {
            double angdeg = calcGeomMeanAnomalySun(num1);
            double a = java.lang.Math.toRadians(angdeg);
            double num3 = java.lang.Math.sin(a);
            double num4 = java.lang.Math.sin(a + a);
            double num5 = java.lang.Math.sin((a + a) + a);
            return (((num3 * (1.914602 - (num1 * (0.004817 + (1.4E-05 * num1))))) + (num4 * (0.019993 - (0.000101 * num1)))) + (num5 * 0.000289));
        }

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

