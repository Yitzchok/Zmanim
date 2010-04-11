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
using Math = java.lang.Math;

namespace net.sourceforge.zmanim.util
{
    public abstract class AstronomicalCalculator : ICloneable
    {
        private double refraction = 0.56666666666666665;
        private double solarRadius = 0.26666666666666666;

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        internal virtual double adjustZenith(double num1, double num2)
        {
            if (num1 == 90.0)
            {
                num1 += (getSolarRadius() + getRefraction()) + getElevationAdjustment(num2);
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
            return Math.toDegrees(Math.acos(num/(num + (num1/1000.0))));
        }

        internal virtual double getRefraction()
        {
            return refraction;
        }

        internal virtual double getSolarRadius()
        {
            return solarRadius;
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
    }
}