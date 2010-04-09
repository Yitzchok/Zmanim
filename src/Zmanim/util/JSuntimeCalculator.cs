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

namespace net.sourceforge.zmanim.util
{
    using java.util;
    using zmanim;
    using System;

    [Obsolete]
    public class JSuntimeCalculator : AstronomicalCalculator
    {
        private string calculatorName = "US National Oceanic and Atmospheric Administration Algorithm";

        private static double dateToJulian(Calendar calendar1)
        {
            int num = calendar1.get(1);
            int num2 = calendar1.get(2) + 1;
            int num3 = calendar1.get(5);
            int num4 = calendar1.get(11);
            int num5 = calendar1.get(12);
            int num6 = calendar1.get(13);
            double a = ((100.0 * num) + num2) - 190002.5;
            return ((((((((367.0 * num) - java.lang.Math.floor((7.0 * (num + java.lang.Math.floor((num2 + 9.0) / 12.0))) / 4.0)) + java.lang.Math.floor((275.0 * num2) / 9.0)) + num3) + ((num4 + ((num5 + (((double) num6) / 60.0)) / 60.0)) / 24.0)) + 1721013.5) - ((0.5 * a) / java.lang.Math.abs(a))) + 0.5);
        }

        private static double eccentricityOfEarthsOrbit(double num1)
        {
            return (0.016708634 - (num1 * (4.2037E-05 + (1.267E-07 * num1))));
        }

        private static double equationOfCentreForSun(double num1)
        {
            double angdeg = geometricMeanAnomalyOfSun(num1);
            return (((java.lang.Math.sin(java.lang.Math.toRadians(angdeg)) * (1.914602 - (num1 * (0.004817 + (1.4E-05 * num1))))) + (java.lang.Math.sin(2.0 * java.lang.Math.toRadians(angdeg)) * (0.019993 - (0.000101 * num1)))) + (java.lang.Math.sin(3.0 * java.lang.Math.toRadians(angdeg)) * 0.000289));
        }

        private static double equationOfTime(double num1)
        {
            double angdeg = obliquityCorrection(num1);
            double num2 = geomMeanLongSun(num1);
            double num3 = eccentricityOfEarthsOrbit(num1);
            double num4 = geometricMeanAnomalyOfSun(num1);
            double num5 = java.lang.Math.pow(java.lang.Math.tan(java.lang.Math.toRadians(angdeg) / 2.0), 2.0);
            double angrad = ((((num5 * java.lang.Math.sin(2.0 * java.lang.Math.toRadians(num2))) - ((2.0 * num3) * java.lang.Math.sin(java.lang.Math.toRadians(num4)))) + ((((4.0 * num3) * num5) * java.lang.Math.sin(java.lang.Math.toRadians(num4))) * java.lang.Math.cos(2.0 * java.lang.Math.toRadians(num2)))) - (((0.5 * num5) * num5) * java.lang.Math.sin(4.0 * java.lang.Math.toRadians(num2)))) - (((1.25 * num3) * num3) * java.lang.Math.sin(2.0 * java.lang.Math.toRadians(num4)));
            return (java.lang.Math.toDegrees(angrad) * 4.0);
        }

        private static double eveningPhenomenon(double num1, double num10, double num12, double num11)
        {
            double num = julianDayToJulianCenturies(num1);
            double num2 = equationOfTime(num);
            double num3 = sunDeclination(num);
            double angrad = hourAngleEvening(num10, num3, num11);
            double num5 = num12 - java.lang.Math.toDegrees(angrad);
            double num6 = 4.0 * num5;
            double num7 = (720.0 + num6) - num2;
            double num8 = julianDayToJulianCenturies(julianCenturiesToJulianDay(num) + (num7 / 1440.0));
            num2 = equationOfTime(num8);
            num3 = sunDeclination(num8);
            angrad = hourAngleEvening(num10, num3, num11);
            num5 = num12 - java.lang.Math.toDegrees(angrad);
            num6 = 4.0 * num5;
            return ((720.0 + num6) - num2);
        }

        private static double geometricMeanAnomalyOfSun(double num1)
        {
            return (357.52911 + (num1 * (35999.05029 - (0.0001537 * num1))));
        }

        private static double geomMeanLongSun(double num1)
        {
            double num = 280.46646 + (num1 * (36000.76983 + (0.0003032 * num1)));
            while ((num >= 0f) && (num <= 360.0))
            {
                if (num > 360.0)
                {
                    num -= 360.0;
                }
                if (num < 0f)
                {
                    num += 360.0;
                }
            }
            return num;
        }

        [Obsolete]
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
            double num2 = morningPhenomenon(dateToJulian(astronomicalCalendar.getCalendar()), astronomicalCalendar.getGeoLocation().getLatitude(), -astronomicalCalendar.getGeoLocation().getLongitude(), zenith);
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
            double num2 = eveningPhenomenon(dateToJulian(astronomicalCalendar.getCalendar()), astronomicalCalendar.getGeoLocation().getLatitude(), -astronomicalCalendar.getGeoLocation().getLongitude(), zenith);
            return (num2 / 60.0);
        }

        private static double hourAngleEvening(double num1, double num2, double num3)
        {
            return -hourAngleMorning(num1, num2, num3);
        }

        private static double hourAngleMorning(double num2, double num3, double num1)
        {
            return java.lang.Math.acos((java.lang.Math.cos(java.lang.Math.toRadians(num1)) / (java.lang.Math.cos(java.lang.Math.toRadians(num2)) * java.lang.Math.cos(java.lang.Math.toRadians(num3)))) - (java.lang.Math.tan(java.lang.Math.toRadians(num2)) * java.lang.Math.tan(java.lang.Math.toRadians(num3))));
        }

        private static double julianCenturiesToJulianDay(double num1)
        {
            return ((num1 * 36525.0) + 2451545.0);
        }

        private static double julianDayToJulianCenturies(double num1)
        {
            return ((num1 - 2451545.0) / 36525.0);
        }

        private static double meanObliquityOfEcliptic(double num1)
        {
            return (23.0 + ((26.0 + (21.448 - ((num1 * (46.815 + (num1 * (0.00059 - (num1 * 0.001813))))) / 60.0))) / 60.0));
        }

        private static double morningPhenomenon(double num1, double num10, double num12, double num11)
        {
            double num = julianDayToJulianCenturies(num1);
            double num2 = equationOfTime(num);
            double num3 = sunDeclination(num);
            double angrad = hourAngleMorning(num10, num3, num11);
            double num5 = num12 - java.lang.Math.toDegrees(angrad);
            double num6 = 4.0 * num5;
            double num7 = (720.0 + num6) - num2;
            double num8 = julianDayToJulianCenturies(julianCenturiesToJulianDay(num) + (num7 / 1440.0));
            num2 = equationOfTime(num8);
            num3 = sunDeclination(num8);
            angrad = hourAngleMorning(num10, num3, num11);
            num5 = num12 - java.lang.Math.toDegrees(angrad);
            num6 = 4.0 * num5;
            return ((720.0 + num6) - num2);
        }

        private static double obliquityCorrection(double num1)
        {
            return (meanObliquityOfEcliptic(num1) + (0.00256 * java.lang.Math.cos(java.lang.Math.toRadians(125.04 - (1934.136 * num1)))));
        }

        private static double sunDeclination(double num1)
        {
            double angdeg = obliquityCorrection(num1);
            double num2 = sunsApparentLongitude(num1);
            double a = java.lang.Math.sin(java.lang.Math.toRadians(angdeg)) * java.lang.Math.sin(java.lang.Math.toRadians(num2));
            return java.lang.Math.toDegrees(java.lang.Math.asin(a));
        }

        private static double sunsApparentLongitude(double num1)
        {
            return ((sunsTrueLongitude(num1) - 0.00569) - (0.00478 * java.lang.Math.sin(java.lang.Math.toRadians(125.04 - (1934.136 * num1)))));
        }

        private static double sunsTrueLongitude(double num1)
        {
            return (geomMeanLongSun(num1) + equationOfCentreForSun(num1));
        }
    }
}

