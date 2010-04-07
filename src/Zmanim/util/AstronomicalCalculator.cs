namespace net.sourceforge.zmanim.util
{
    using IKVM.Attributes;
    using java.lang;
    using net.sourceforge.zmanim;
    using System;
    using System.Runtime.CompilerServices;

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

