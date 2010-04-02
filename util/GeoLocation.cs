namespace net.sourceforge.zmanim.util
{
    using IKVM.Attributes;
    using IKVM.Runtime;
    using java.lang;
    using java.util;
    using System;
    using System.Runtime.CompilerServices;

    [Implements(new string[] { "java.lang.Cloneable" })]
    public class GeoLocation : java.lang.Object, Cloneable.__Interface
    {
        private int DISTANCE;
        private double elevation;
        private int FINAL_BEARING;
        private const long HOUR_MILLIS = 0x36ee80L;
        private int INITIAL_BEARING;
        private double latitude;
        private string locationName;
        private double longitude;
        private const long MINUTE_MILLIS = 0xea60L;
        private java.util.TimeZone timeZone;

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 80, 0xe8, 0x9f, 0xa3, 0x67, 0x67, 0xe7, 160, 0x5c, 0x6b, 0x6b, 0x6f, 0x70 })]
        public GeoLocation()
        {
            this.DISTANCE = 0;
            this.INITIAL_BEARING = 1;
            this.FINAL_BEARING = 2;
            this.setLocationName("Greenwich, England");
            this.setLongitude(0f);
            this.setLatitude(51.4772);
            this.setTimeZone(java.util.TimeZone.getTimeZone("GMT"));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 40, 0x72 })]
        public GeoLocation(string name, double latitude, double longitude, java.util.TimeZone timeZone) : this(name, latitude, longitude, 0f, timeZone)
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0x41, 0xe8, 0x9f, 0xb2, 0x67, 0x67, 0xe7, 160, 0x4d, 0x67, 0x68, 0x68, 0x69, 0x68 })]
        public GeoLocation(string name, double latitude, double longitude, double elevation, java.util.TimeZone timeZone)
        {
            this.DISTANCE = 0;
            this.INITIAL_BEARING = 1;
            this.FINAL_BEARING = 2;
            this.setLocationName(name);
            this.setLatitude(latitude);
            this.setLongitude(longitude);
            this.setElevation(elevation);
            this.setTimeZone(timeZone);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa2, 9, 130, 0xdf, 2, 0xe5, 0x3d, 0x61, 0xaf, 0x76, 0x6c })]
        public override object clone()
        {
            GeoLocation location = null;
            try
            {
                location = (GeoLocation) base.clone();
            }
            catch (CloneNotSupportedException)
            {
                System.@out.print("Required by the compiler. Should never be reached since we implement clone()");
            }
            location.timeZone = (java.util.TimeZone) this.getTimeZone().clone();
            location.locationName = this.getLocationName();
            return location;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa1, 0xbf, 0x67, 0x62, 0x6b, 0x62, 0x67 })]
        public override bool equals(object @object)
        {
            if (this == @object)
            {
                return true;
            }
            if (!(@object is GeoLocation))
            {
                return false;
            }
            GeoLocation location = (GeoLocation) @object;
            if (((java.lang.Double.doubleToLongBits(this.latitude) == java.lang.Double.doubleToLongBits(location.latitude)) && (java.lang.Double.doubleToLongBits(this.longitude) == java.lang.Double.doubleToLongBits(location.longitude))) && (this.elevation == location.elevation))
            {
                if (this.locationName == null)
                {
                    if (location.locationName != null)
                    {
                        goto Label_00C6;
                    }
                }
                else if (!java.lang.String.instancehelper_equals(this.locationName, location.locationName))
                {
                    goto Label_00C6;
                }
                if ((this.timeZone == null) && (location.timeZone == null))
                {
                }
            }
        Label_00C6:
            return java.lang.Object.instancehelper_equals(this.timeZone, location.timeZone);
        }

        public virtual double getElevation()
        {
            return this.elevation;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x16b)]
        public virtual double getGeodesicDistance(GeoLocation location)
        {
            return this.vincentyFormula(location, this.DISTANCE);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x159)]
        public virtual double getGeodesicFinalBearing(GeoLocation location)
        {
            return this.vincentyFormula(location, this.FINAL_BEARING);
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x148)]
        public virtual double getGeodesicInitialBearing(GeoLocation location)
        {
            return this.vincentyFormula(location, this.INITIAL_BEARING);
        }

        public virtual double getLatitude()
        {
            return this.latitude;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 310)]
        public virtual long getLocalMeanTimeOffset()
        {
            return ByteCodeHelper.d2l(((this.getLongitude() * 4.0) * 60000.0) - this.getTimeZone().getRawOffset());
        }

        public virtual string getLocationName()
        {
            return this.locationName;
        }

        public virtual double getLongitude()
        {
            return this.longitude;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa1, 0x6a, 0x74, 0xbf, 0x31, 0x74, 0x7f, 10 })]
        public virtual double getRhumbLineBearing(GeoLocation location)
        {
            double a = java.lang.Math.toRadians(location.getLongitude() - this.getLongitude());
            double x = java.lang.Math.log(java.lang.Math.tan((java.lang.Math.toRadians(location.getLatitude()) / 2.0) + 0.78539816339744828) / java.lang.Math.tan((java.lang.Math.toRadians(this.getLatitude()) / 2.0) + 0.78539816339744828));
            if (java.lang.Math.abs(a) > 3.1415926535897931)
            {
                a = (a <= 0f) ? (6.2831853071795862 + a) : -(6.2831853071795862 - a);
            }
            return java.lang.Math.toDegrees(java.lang.Math.atan2(a, x));
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa1, 0x7c, 0x6a, 0x74, 0x99, 0xbf, 0x31, 0xbf, 0x10, 0x6f, 0x6d, 0x75 })]
        public virtual double getRhumbLineDistance(GeoLocation location)
        {
            double num = 6371.0;
            double a = java.lang.Math.toRadians(location.getLatitude() - this.getLatitude());
            double num3 = java.lang.Math.toRadians(java.lang.Math.abs((double) (location.getLongitude() - this.getLongitude())));
            double num4 = java.lang.Math.log(java.lang.Math.tan((java.lang.Math.toRadians(location.getLongitude()) / 2.0) + 0.78539816339744828) / java.lang.Math.tan((java.lang.Math.toRadians(this.getLatitude()) / 2.0) + 0.78539816339744828));
            double num5 = (java.lang.Math.abs(a) <= 1E-10) ? java.lang.Math.cos(java.lang.Math.toRadians(this.getLatitude())) : (a / num4);
            if (num3 > 3.1415926535897931)
            {
                num3 = 6.2831853071795862 - num3;
            }
            double num6 = java.lang.Math.sqrt((a * a) + (((num5 * num5) * num3) * num3));
            return (num6 * num);
        }

        public virtual java.util.TimeZone getTimeZone()
        {
            return this.timeZone;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 
            0xa1, 0xd4, 0x63, 0x6c, 0x6c, 0x6c, 0x69, 0x69, 0x69, 0x71, 0x6a, 0x6a, 0x6a, 0x9f, 5, 0x7f, 
            5
         })]
        public override int hashCode()
        {
            int num = 0x11;
            long num2 = java.lang.Double.doubleToLongBits(this.latitude);
            long num3 = java.lang.Double.doubleToLongBits(this.longitude);
            long num4 = java.lang.Double.doubleToLongBits(this.elevation);
            int num5 = (int) (num2 ^ (num2 >> 0x20));
            int num6 = (int) (num3 ^ (num3 >> 0x20));
            int num7 = (int) (num4 ^ (num4 >> 0x20));
            num = (0x25 * num) + java.lang.Object.instancehelper_hashCode(base.getClass());
            num += (0x25 * num) + num5;
            num += (0x25 * num) + num6;
            num += (0x25 * num) + num7;
            num += (0x25 * num) + ((this.locationName != null) ? java.lang.String.instancehelper_hashCode(this.locationName) : 0);
            return (num + ((0x25 * num) + ((this.timeZone != null) ? java.lang.Object.instancehelper_hashCode(this.timeZone) : 0)));
        }

        [HideFromJava]
        public static implicit operator Cloneable(GeoLocation location1)
        {
            Cloneable cloneable;
            cloneable.__<ref> = location1;
            return cloneable;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 14, 0x6c, 0x90, 0x69 })]
        public virtual void setElevation(double elevation)
        {
            if (elevation < 0f)
            {
                Throwable.__<suppressFillInStackTrace>();
                throw new IllegalArgumentException("Elevation cannot be negative");
            }
            this.elevation = elevation;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0x62, 0x7f, 1, 0xb0, 0x69 })]
        public virtual void setLatitude(double latitude)
        {
            if ((latitude > 90.0) || (latitude < -90.0))
            {
                Throwable.__<suppressFillInStackTrace>();
                throw new IllegalArgumentException("Latitude must be between -90 and  90");
            }
            this.latitude = latitude;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 120, 0x7e, 0x7a, 0xb0, 0x71, 0x72, 0x71, 0xb0, 0x68 })]
        public virtual void setLatitude(int degrees, int minutes, double seconds, string direction)
        {
            double num = degrees + ((minutes + (seconds / 60.0)) / 60.0);
            if ((num > 90.0) || (num < 0f))
            {
                Throwable.__<suppressFillInStackTrace>();
                throw new IllegalArgumentException("Latitude must be between 0 and  90. Use direction of S instead of negative.");
            }
            if (java.lang.String.instancehelper_equals(direction, "S"))
            {
                num *= -1.0;
            }
            else if (!java.lang.String.instancehelper_equals(direction, "N"))
            {
                Throwable.__<suppressFillInStackTrace>();
                throw new IllegalArgumentException("Latitude direction must be N or S");
            }
            this.latitude = num;
        }

        public virtual void setLocationName(string name)
        {
            this.locationName = name;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 90, 0x7f, 1, 0xb0, 0x69 })]
        public virtual void setLongitude(double longitude)
        {
            if ((longitude > 180.0) || (longitude < -180.0))
            {
                Throwable.__<suppressFillInStackTrace>();
                throw new IllegalArgumentException("Longitude must be between -180 and  180");
            }
            this.longitude = longitude;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 160, 0x74, 0x7e, 0x7f, 0, 0xb0, 0x71, 0x72, 0x71, 0xb0, 0x68 })]
        public virtual void setLongitude(int degrees, int minutes, double seconds, string direction)
        {
            double num = degrees + ((minutes + (seconds / 60.0)) / 60.0);
            if ((num > 180.0) || (this.longitude < 0f))
            {
                Throwable.__<suppressFillInStackTrace>();
                throw new IllegalArgumentException("Longitude must be between 0 and  180. Use the ");
            }
            if (java.lang.String.instancehelper_equals(direction, "W"))
            {
                num *= -1.0;
            }
            else if (!java.lang.String.instancehelper_equals(direction, "E"))
            {
                Throwable.__<suppressFillInStackTrace>();
                throw new IllegalArgumentException("Longitude direction must be E or W");
            }
            this.longitude = num;
        }

        public virtual void setTimeZone(java.util.TimeZone timeZone)
        {
            this.timeZone = timeZone;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0xa1, 0xe9, 0x66, 0x77, 0x7f, 2, 0x7f, 2, 0x9f, 2, 0xfc, 0x45, 0x9f, 5, 0x9f, 5 })]
        public override string toString()
        {
            StringBuffer buffer = new StringBuffer();
            buffer.append("\nLocation Name:\t\t\t").append(this.getLocationName());
            buffer.append("\nLatitude:\t\t\t").append(this.getLatitude()).append("&deg;");
            buffer.append("\nLongitude:\t\t\t").append(this.getLongitude()).append("&deg;");
            buffer.append("\nElevation:\t\t\t").append(this.getElevation()).append(" Meters");
            buffer.append("\nTimezone Name:\t\t\t").append(this.getTimeZone().getID());
            buffer.append("\nTimezone GMT Offset:\t\t").append((long) (((long) this.getTimeZone().getRawOffset()) / 0x36ee80L));
            buffer.append("\nTimezone DST Offset:\t\t").append((long) (((long) this.getTimeZone().getDSTSavings()) / 0x36ee80L));
            return buffer.toString();
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 
            0xa1, 0xa2, 0x66, 0x6c, 0x9f, 2, 0x9f, 12, 0x9f, 12, 0x9f, 12, 0x9f, 7, 0xbf, 7, 
            0xbf, 15, 0xbf, 15, 0x6c
         })]
        public virtual string toXML()
        {
            StringBuffer buffer = new StringBuffer();
            buffer.append("<GeoLocation>\n");
            buffer.append("\t<LocationName>").append(this.getLocationName()).append("</LocationName>\n");
            buffer.append("\t<Latitude>").append(this.getLatitude()).append("&deg;").append("</Latitude>\n");
            buffer.append("\t<Longitude>").append(this.getLongitude()).append("&deg;").append("</Longitude>\n");
            buffer.append("\t<Elevation>").append(this.getElevation()).append(" Meters").append("</Elevation>\n");
            buffer.append("\t<TimezoneName>").append(this.getTimeZone().getID()).append("</TimezoneName>\n");
            buffer.append("\t<TimeZoneDisplayName>").append(this.getTimeZone().getDisplayName()).append("</TimeZoneDisplayName>\n");
            buffer.append("\t<TimezoneGMTOffset>").append((long) (((long) this.getTimeZone().getRawOffset()) / 0x36ee80L)).append("</TimezoneGMTOffset>\n");
            buffer.append("\t<TimezoneDSTOffset>").append((long) (((long) this.getTimeZone().getDSTSavings()) / 0x36ee80L)).append("</TimezoneDSTOffset>\n");
            buffer.append("</GeoLocation>");
            return buffer.toString();
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 
            0xa1, 14, 0x6a, 0x6a, 0x6a, 0x74, 0x9f, 1, 0x9f, 1, 0x74, 0x94, 0x63, 0x6b, 0x6b, 0x67, 
            0x67, 0x67, 0x67, 0x67, 0x66, 0x67, 0x87, 0x7f, 15, 0x6a, 0x6a, 0xbf, 0x12, 0x6c, 0x66, 0x71, 
            0x6c, 110, 110, 120, 0x6c, 0x67, 0x7f, 0x15, 100, 0xff, 0x24, 0x49, 0x6c, 0x8a, 0x71, 0x9f, 
            0x27, 0x7f, 0x21, 0xff, 90, 0x48, 0xad, 0xbf, 1, 0x9f, 2, 0x6c, 0x63, 0x6c, 0x63, 0x6c, 
            0x83
         })]
        private double vincentyFormula(GeoLocation location1, int num30)
        {
            double num = 6378137.0;
            double num2 = 6356752.3142;
            double num3 = 0.0033528106647474805;
            double num4 = java.lang.Math.toRadians(location1.getLongitude() - this.getLongitude());
            double a = java.lang.Math.atan((1f - num3) * java.lang.Math.tan(java.lang.Math.toRadians(this.getLatitude())));
            double num6 = java.lang.Math.atan((1f - num3) * java.lang.Math.tan(java.lang.Math.toRadians(location1.getLatitude())));
            double num7 = java.lang.Math.sin(a);
            double num8 = java.lang.Math.cos(a);
            double num9 = java.lang.Math.sin(num6);
            double num10 = java.lang.Math.cos(num6);
            double num11 = num4;
            double num12 = 6.2831853071795862;
            double num13 = 20.0;
            double num14 = 0f;
            double num15 = 0f;
            double y = 0f;
            double x = 0f;
            double num18 = 0f;
            double num19 = 0f;
            double v = 0f;
            while (true)
            {
                if (java.lang.Math.abs((double) (num11 - num12)) <= 1E-12)
                {
                    break;
                }
                double num1 = num13 - 1f;
                num13 = num1;
                if (num1 <= 0f)
                {
                    break;
                }
                num14 = java.lang.Math.sin(num11);
                num15 = java.lang.Math.cos(num11);
                y = java.lang.Math.sqrt(((num10 * num14) * (num10 * num14)) + (((num8 * num9) - ((num7 * num10) * num15)) * ((num8 * num9) - ((num7 * num10) * num15))));
                if (y == 0f)
                {
                    return 0f;
                }
                x = (num7 * num9) + ((num8 * num10) * num15);
                num18 = java.lang.Math.atan2(y, x);
                double num21 = ((num8 * num10) * num14) / y;
                num19 = 1f - (num21 * num21);
                v = x - (((2.0 * num7) * num9) / num19);
                if (java.lang.Double.isNaN(v))
                {
                    v = 0f;
                }
                double num22 = ((num3 / 16.0) * num19) * (4.0 + (num3 * (4.0 - (3.0 * num19))));
                num12 = num11;
                num11 = num4 + ((((1f - num22) * num3) * num21) * (num18 + ((num22 * y) * (v + ((num22 * x) * (-1.0 + ((2.0 * v) * v)))))));
            }
            if (num13 != 0f)
            {
                double num23 = (num19 * ((num * num) - (num2 * num2))) / (num2 * num2);
                double num24 = 1f + ((num23 / 16384.0) * (4096.0 + (num23 * (-768.0 + (num23 * (320.0 - (175.0 * num23)))))));
                double num25 = (num23 / 1024.0) * (256.0 + (num23 * (-128.0 + (num23 * (74.0 - (47.0 * num23))))));
                double num26 = (num25 * y) * (v + ((num25 / 4.0) * ((x * (-1.0 + ((2.0 * v) * v))) - ((((num25 / 6.0) * v) * (-3.0 + ((4.0 * y) * y))) * (-3.0 + ((4.0 * v) * v))))));
                double num27 = (num2 * num24) * (num18 - num26);
                double num28 = java.lang.Math.toDegrees(java.lang.Math.atan2(num10 * num14, (num8 * num9) - ((num7 * num10) * num15)));
                double num29 = java.lang.Math.toDegrees(java.lang.Math.atan2(num8 * num14, (-num7 * num10) + ((num8 * num9) * num15)));
                if (num30 == this.DISTANCE)
                {
                    return num27;
                }
                if (num30 == this.INITIAL_BEARING)
                {
                    return num28;
                }
                if (num30 == this.FINAL_BEARING)
                {
                    return num29;
                }
            }
            return double.NaN;
        }
    }
}

