// * Zmanim Java API
// * Copyright (C) 2004-2010 Eliyahu Hershfeld
// *
// * Converted to C# by AdminJew
// *
// * This program is free software; you can redistribute it and/or modify it under the terms of the
// * GNU General Public License as published by the Free Software Foundation; either version 2 of the
// * License, or (at your option) any later version.
// *
// * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// * even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// * General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License along with this program; if
// * not, write to the Free Software Foundation, Inc. 59 Temple Place - Suite 330, Boston, MA
// * 02111-1307, USA or connect to: http://www.fsf.org/copyleft/gpl.html

namespace net.sourceforge.zmanim.util
{
    using IKVM.Attributes;
    using java.lang;
    using net.sourceforge.zmanim;
    using System;
    using System.Runtime.CompilerServices;

    public class ZmanimCalculator : AstronomicalCalculator
    {
        private string calculatorName = "US Naval Almanac Algorithm";

        public override string getCalculatorName()
        {
            return this.calculatorName;
        }

        public override double getUTCSunrise(AstronomicalCalendar astronomicalCalendar, double zenith, bool adjustForElevation)
        {
            double num6;
            double num7;
            double num15;
            if (adjustForElevation)
            {
                zenith = this.adjustZenith(zenith, astronomicalCalendar.getGeoLocation().getElevation());
            }
            else
            {
                zenith = this.adjustZenith(zenith, 0f);
            }
            double num2 = astronomicalCalendar.getGeoLocation().getLongitude() / 15.0;
            double num3 = astronomicalCalendar.getCalendar().get(6) + ((6.0 - num2) / 24.0);
            double angdeg = (0.9856 * num3) - 3.289;
            double num5 = ((angdeg + (1.916 * java.lang.Math.sin(java.lang.Math.toRadians(angdeg)))) + (0.02 * java.lang.Math.sin(java.lang.Math.toRadians(2.0 * angdeg)))) + 282.634;
            while (true)
            {
                if (num5 >= 0f)
                {
                    break;
                }
                num6 = num5 + 360.0;
                num5 = num6;
            }
            while (num5 >= 360.0)
            {
                num6 = num5 - 360.0;
                num5 = num6;
            }
            num6 = java.lang.Math.toDegrees(java.lang.Math.atan(0.91764 * java.lang.Math.tan(java.lang.Math.toRadians(num5))));
            while (true)
            {
                if (num6 >= 0f)
                {
                    break;
                }
                num7 = num6 + 360.0;
                num6 = num7;
            }
            while (num6 >= 360.0)
            {
                num7 = num6 - 360.0;
                num6 = num7;
            }
            num7 = java.lang.Math.floor(num5 / 90.0) * 90.0;
            double num8 = java.lang.Math.floor(num6 / 90.0) * 90.0;
            num6 += num7 - num8;
            num6 /= 15.0;
            double a = 0.39782 * java.lang.Math.sin(java.lang.Math.toRadians(num5));
            double num10 = java.lang.Math.cos(java.lang.Math.asin(a));
            double num11 = (java.lang.Math.cos(java.lang.Math.toRadians(zenith)) - (a * java.lang.Math.sin(java.lang.Math.toRadians(astronomicalCalendar.getGeoLocation().getLatitude())))) / (num10 * java.lang.Math.cos(java.lang.Math.toRadians(astronomicalCalendar.getGeoLocation().getLatitude())));
            double num12 = 360.0 - java.lang.Math.toDegrees(java.lang.Math.acos(num11));
            num12 /= 15.0;
            double num13 = ((num12 + num6) - (0.06571 * num3)) - 6.622;
            double num14 = num13 - num2;
            while (true)
            {
                if (num14 >= 0f)
                {
                    break;
                }
                num15 = num14 + 24.0;
                num14 = num15;
            }
            while (num14 >= 24.0)
            {
                num15 = num14 - 24.0;
                num14 = num15;
            }
            return num14;
        }

        public override double getUTCSunset(AstronomicalCalendar astronomicalCalendar, double zenith, bool adjustForElevation)
        {
            double num7;
            double num8;
            double num16;
            if (adjustForElevation)
            {
                zenith = this.adjustZenith(zenith, astronomicalCalendar.getGeoLocation().getElevation());
            }
            else
            {
                zenith = this.adjustZenith(zenith, 0f);
            }
            int num2 = astronomicalCalendar.getCalendar().get(6);
            double num3 = astronomicalCalendar.getGeoLocation().getLongitude() / 15.0;
            double num4 = num2 + ((18.0 - num3) / 24.0);
            double angdeg = (0.9856 * num4) - 3.289;
            double num6 = ((angdeg + (1.916 * java.lang.Math.sin(java.lang.Math.toRadians(angdeg)))) + (0.02 * java.lang.Math.sin(java.lang.Math.toRadians(2.0 * angdeg)))) + 282.634;
            while (true)
            {
                if (num6 >= 0f)
                {
                    break;
                }
                num7 = num6 + 360.0;
                num6 = num7;
            }
            while (num6 >= 360.0)
            {
                num7 = num6 - 360.0;
                num6 = num7;
            }
            num7 = java.lang.Math.toDegrees(java.lang.Math.atan(0.91764 * java.lang.Math.tan(java.lang.Math.toRadians(num6))));
            while (true)
            {
                if (num7 >= 0f)
                {
                    break;
                }
                num8 = num7 + 360.0;
                num7 = num8;
            }
            while (num7 >= 360.0)
            {
                num8 = num7 - 360.0;
                num7 = num8;
            }
            num8 = java.lang.Math.floor(num6 / 90.0) * 90.0;
            double num9 = java.lang.Math.floor(num7 / 90.0) * 90.0;
            num7 += num8 - num9;
            num7 /= 15.0;
            double a = 0.39782 * java.lang.Math.sin(java.lang.Math.toRadians(num6));
            double num11 = java.lang.Math.cos(java.lang.Math.asin(a));
            double num12 = (java.lang.Math.cos(java.lang.Math.toRadians(zenith)) - (a * java.lang.Math.sin(java.lang.Math.toRadians(astronomicalCalendar.getGeoLocation().getLatitude())))) / (num11 * java.lang.Math.cos(java.lang.Math.toRadians(astronomicalCalendar.getGeoLocation().getLatitude())));
            double num13 = java.lang.Math.toDegrees(java.lang.Math.acos(num12));
            num13 /= 15.0;
            double num14 = ((num13 + num7) - (0.06571 * num4)) - 6.622;
            double num15 = num14 - num3;
            while (true)
            {
                if (num15 >= 0f)
                {
                    break;
                }
                num16 = num15 + 24.0;
                num15 = num16;
            }
            while (num15 >= 24.0)
            {
                num16 = num15 - 24.0;
                num15 = num16;
            }
            return num15;
        }
    }
}

