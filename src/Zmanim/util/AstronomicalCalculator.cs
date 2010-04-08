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
    using net.sourceforge.zmanim;
    using System;

    public abstract class AstronomicalCalculator : ICloneable
    {
        private double refraction = 0.56666666666666665;
        private double solarRadius = 0.26666666666666666;

        internal virtual double adjustZenith(double num1, double num2)
        {
            if (num1 == 90.0)
            {
                num1 += (this.getSolarRadius() + this.getRefraction()) + this.getElevationAdjustment(num2);
            }
            return num1;
        }

        public abstract string getCalculatorName();
        public static AstronomicalCalculator getDefault()
        {
            return new SunTimesCalculator();
        }

        internal virtual double getElevationAdjustment(double num1)
        {
            double num = 6356.9;
            return java.lang.Math.toDegrees(java.lang.Math.acos(num / (num + (num1 / 1000.0))));
        }

        internal virtual double getRefraction()
        {
            return this.refraction;
        }

        internal virtual double getSolarRadius()
        {
            return this.solarRadius;
        }

        public abstract double getUTCSunrise(AstronomicalCalendar ac, double d, bool b);
        public abstract double getUTCSunset(AstronomicalCalendar ac, double d, bool b);

        public virtual void setRefraction(double refraction)
        {
            this.refraction = refraction;
        }

        public virtual void setSolarRadius(double solarRadius)
        {
            this.solarRadius = solarRadius;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}

