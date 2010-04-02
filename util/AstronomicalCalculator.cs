namespace net.sourceforge.zmanim.util
{
    using IKVM.Attributes;
    using java.lang;
    using net.sourceforge.zmanim;
    using System;
    using System.Runtime.CompilerServices;

    [Implements(new string[] { "java.lang.Cloneable" })]
    public abstract class AstronomicalCalculator : java.lang.Object, Cloneable.__Interface
    {
        private double refraction = 0.56666666666666665;
        private double solarRadius = 0.26666666666666666;

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0x40, 0x70, 220 })]
        internal virtual double adjustZenith(double num1, double num2)
        {
            if (num1 == 90.0)
            {
                num1 += (this.getSolarRadius() + this.getRefraction()) + this.getElevationAdjustment(num2);
            }
            return num1;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0x91, 130, 0xdf, 2, 0xe5, 0x3d, 0x61, 0xaf })]
        public override object clone()
        {
            AstronomicalCalculator calculator = null;
            try
            {
                calculator = (AstronomicalCalculator) base.clone();
            }
            catch (CloneNotSupportedException)
            {
                System.@out.print("Required by the compiler. Should never be reached since we implement clone()");
                return calculator;
            }
            return calculator;
        }

        public abstract string getCalculatorName();
        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x2b)]
        public static AstronomicalCalculator getDefault()
        {
            return new SunTimesCalculator();
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0x5e, 0x8a, 0x9c })]
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
        [HideFromJava]
        public static implicit operator Cloneable(AstronomicalCalculator calculator1)
        {
            Cloneable cloneable;
            cloneable.__<ref> = calculator1;
            return cloneable;
        }

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

