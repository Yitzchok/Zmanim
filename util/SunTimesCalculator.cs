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

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x74)]
        private static double acosDeg(double num1)
        {
            return ((java.lang.Math.acos(num1) * 360.0) / 6.2831853071795862);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x7b)]
        private static double asinDeg(double num1)
        {
            return ((java.lang.Math.asin(num1) * 360.0) / 6.2831853071795862);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x89)]
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

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0x6f, 0x73, 0x8d, 0xfd, 0x45 })]
        private static double getCosLocalHourAngle(double num1, double num5, double num4)
        {
            double num = 0.39782 * sinDeg(num1);
            double num2 = cosDeg(asinDeg(num));
            return ((cosDeg(num4) - (num * sinDeg(num5))) / (num2 * cosDeg(num5)));
        }

        [LineNumberTable(new byte[] { 0x5f, 0x6b, 0x68, 110, 0x6b })]
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

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 180)]
        private static double getMeanAnomaly(int num1, double num2, int num3)
        {
            return ((0.9856 * getApproxTimeDays(num1, getHoursFromMeridian(num2), num3)) - 3.289);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0x5d, 0x73, 0xf2, 0x45, 0x7d, 0x7c, 0x87 })]
        private static double getSunRightAscensionHours(double num1)
        {
            double a = 0.91764 * tanDeg(num1);
            double num2 = 57.295779513082323 * java.lang.Math.atan(a);
            double num3 = java.lang.Math.floor(num1 / 90.0) * 90.0;
            double num4 = java.lang.Math.floor(num2 / 90.0) * 90.0;
            num2 += num3 - num4;
            return (num2 / 15.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0x4b, 0xdf, 0x1d, 0x6f, 0x8d, 0x6b, 0x8d })]
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

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 
            160, 0xa9, 0x69, 0x6c, 0x68, 0x68, 0xaf, 0x66, 0x67, 0xcc, 0x99, 0xd0, 0x8a, 0x8f, 0xba, 0x6d, 
            0x6c, 0x91, 0x70, 0x91
         })]
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

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0x9f, 0x81, 0x43, 0x8a, 0x66, 0xbb, 0x90, 0xff, 0x27, 70 })]
        public override double getUTCSunrise(AstronomicalCalendar astronomicalCalendar, double zenith, bool adjustForElevation)
        {
            int num = (int) adjustForElevation;
            if (num != 0)
            {
                zenith = this.adjustZenith(zenith, astronomicalCalendar.getGeoLocation().getElevation());
            }
            else
            {
                zenith = this.adjustZenith(zenith, 0f);
            }
            return getTimeUTC(astronomicalCalendar.getCalendar().get(1), astronomicalCalendar.getCalendar().get(2) + 1, astronomicalCalendar.getCalendar().get(5), astronomicalCalendar.getGeoLocation().getLongitude(), astronomicalCalendar.getGeoLocation().getLatitude(), zenith, 0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0x9f, 0x7c, 0xa3, 0x8a, 0x66, 0xbb, 0x90, 0xff, 0x27, 70 })]
        public override double getUTCSunset(AstronomicalCalendar astronomicalCalendar, double zenith, bool adjustForElevation)
        {
            int num = (int) adjustForElevation;
            if (num != 0)
            {
                zenith = this.adjustZenith(zenith, astronomicalCalendar.getGeoLocation().getElevation());
            }
            else
            {
                zenith = this.adjustZenith(zenith, 0f);
            }
            return getTimeUTC(astronomicalCalendar.getCalendar().get(1), astronomicalCalendar.getCalendar().get(2) + 1, astronomicalCalendar.getCalendar().get(5), astronomicalCalendar.getGeoLocation().getLongitude(), astronomicalCalendar.getGeoLocation().getLatitude(), zenith, 1);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x6d)]
        private static double sinDeg(double num1)
        {
            return java.lang.Math.sin(((num1 * 2.0) * 3.1415926535897931) / 360.0);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 130)]
        private static double tanDeg(double num1)
        {
            return java.lang.Math.tan(((num1 * 2.0) * 3.1415926535897931) / 360.0);
        }
    }
}

