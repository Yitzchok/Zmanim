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
    using IKVM.Attributes;
    using java.lang;
    using net.sourceforge.zmanim;
    using System;
    using System.Runtime.CompilerServices;

    public class SunTimesCalculator : AstronomicalCalculator
    {
        private string calculatorName = "US Naval Almanac Algorithm";
        private const double DEG_PER_HOUR = 15.0;
        private const int TYPE_SUNRISE = 0;
        private const int TYPE_SUNSET = 1;
        public const double ZENITH = 90.833333333333329;

        private static double acosDeg(double num1)
        {
            return ((java.lang.Math.acos(num1) * 360.0) / 6.2831853071795862);
        }

        private static double asinDeg(double num1)
        {
            return ((java.lang.Math.asin(num1) * 360.0) / 6.2831853071795862);
        }

        private static double cosDeg(double num1)
        {
            return java.lang.Math.cos(((num1 * 2.0) * 3.1415926535897931) / 360.0);
        }

        private static double getApproxTimeDays(int num2, double num3, int num1)
        {
            if (num1 == 0)
            {
                return (num2 + ((6.0 - num3) / 24.0));
            }
            return (num2 + ((18.0 - num3) / 24.0));
        }

        public override string getCalculatorName()
        {
            return this.calculatorName;
        }

        private static double getCosLocalHourAngle(double num1, double num5, double num4)
        {
            double num = 0.39782 * sinDeg(num1);
            double num2 = cosDeg(asinDeg(num));
            return ((cosDeg(num4) - (num * sinDeg(num5))) / (num2 * cosDeg(num5)));
        }

        private static int getDayOfYear(int num5, int num1, int num6)
        {
            int num = (0x113 * num1) / 9;
            int num2 = (num1 + 9) / 12;
            int num3 = 1 + (((num5 - (4 * (num5 / 4))) + 2) / 3);
            return (((num - (num2 * num3)) + num6) - 30);
        }

        private static double getHoursFromMeridian(double num1)
        {
            return (num1 / 15.0);
        }

        private static double getLocalMeanTime(double num1, double num2, double num3)
        {
            return (((num1 + num2) - (0.06571 * num3)) - 6.622);
        }

        private static double getMeanAnomaly(int num1, double num2, int num3)
        {
            return ((0.9856 * getApproxTimeDays(num1, getHoursFromMeridian(num2), num3)) - 3.289);
        }

        private static double getSunRightAscensionHours(double num1)
        {
            double a = 0.91764 * tanDeg(num1);
            double num2 = 57.295779513082323 * java.lang.Math.atan(a);
            double num3 = java.lang.Math.floor(num1 / 90.0) * 90.0;
            double num4 = java.lang.Math.floor(num2 / 90.0) * 90.0;
            num2 += num3 - num4;
            return (num2 / 15.0);
        }

        private static double getSunTrueLongitude(double num1)
        {
            double num = ((num1 + (1.916 * sinDeg(num1))) + (0.02 * sinDeg(2.0 * num1))) + 282.634;
            if (num >= 360.0)
            {
                num -= 360.0;
            }
            if (num < 0f)
            {
                num += 360.0;
            }
            return num;
        }

        private static double getTimeUTC(int num1, int num10, int num11, double num12, double num14, double num15, int num13)
        {
            double num6;
            int num = getDayOfYear(num1, num10, num11);
            double num2 = getMeanAnomaly(num, num12, num13);
            double num3 = getSunTrueLongitude(num2);
            double num4 = getSunRightAscensionHours(num3);
            double num5 = getCosLocalHourAngle(num3, num14, num15);
            if (num13 == 0)
            {
                if (num5 > 1f)
                {
                }
                num6 = 360.0 - acosDeg(num5);
            }
            else
            {
                if (num5 < -1.0)
                {
                }
                num6 = acosDeg(num5);
            }
            double num7 = num6 / 15.0;
            double num8 = getLocalMeanTime(num7, num4, getApproxTimeDays(num, getHoursFromMeridian(num12), num13));
            double num9 = num8 - getHoursFromMeridian(num12);
            while (true)
            {
                if (num9 >= 0f)
                {
                    break;
                }
                num9 += 24.0;
            }
            while (num9 >= 24.0)
            {
                num9 -= 24.0;
            }
            return num9;
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
            return getTimeUTC(astronomicalCalendar.getCalendar().get(1), astronomicalCalendar.getCalendar().get(2) + 1, astronomicalCalendar.getCalendar().get(5), astronomicalCalendar.getGeoLocation().getLongitude(), astronomicalCalendar.getGeoLocation().getLatitude(), zenith, 0);
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
            return getTimeUTC(astronomicalCalendar.getCalendar().get(1), astronomicalCalendar.getCalendar().get(2) + 1, astronomicalCalendar.getCalendar().get(5), astronomicalCalendar.getGeoLocation().getLongitude(), astronomicalCalendar.getGeoLocation().getLatitude(), zenith, 1);
        }

        private static double sinDeg(double num1)
        {
            return java.lang.Math.sin(((num1 * 2.0) * 3.1415926535897931) / 360.0);
        }

        private static double tanDeg(double num1)
        {
            return java.lang.Math.tan(((num1 * 2.0) * 3.1415926535897931) / 360.0);
        }
    }
}

