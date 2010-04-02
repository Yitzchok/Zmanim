namespace net.sourceforge.zmanim.util
{
    using IKVM.Attributes;
    using java.lang;
    using System;
    using System.Runtime.CompilerServices;

    public class Time : java.lang.Object
    {
        private const int HOUR_MILLIS = 0x36ee80;
        private int hours;
        private bool isNegative;
        private int milliseconds;
        private const int MINUTE_MILLIS = 0xea60;
        private int minutes;
        private const int SECOND_MILLIS = 0x3e8;
        private int seconds;

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 4, 0x6d })]
        public Time(double millis) : this(ByteCodeHelper.d2i(millis))
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 
            7, 0xe8, 0x2b, 0x87, 0x87, 0x87, 0x87, 0xe7, 0x4e, 0x67, 0x67, 0x88, 0x6d, 0x90, 0x6d, 0x90, 
            0x6d, 0x90, 0x67
         })]
        public Time(int millis)
        {
            this.hours = 0;
            this.minutes = 0;
            this.seconds = 0;
            this.milliseconds = 0;
            this.isNegative = false;
            if (millis < 0)
            {
                this.isNegative = true;
                millis = java.lang.Math.abs(millis);
            }
            this.hours = millis / 0x36ee80;
            millis -= this.hours * 0x36ee80;
            this.minutes = millis / 0xea60;
            millis -= this.minutes * 0xea60;
            this.seconds = millis / 0x3e8;
            millis -= this.seconds * 0x3e8;
            this.milliseconds = millis;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0x9f, 0xbc, 0xe8, 0x36, 0x87, 0x87, 0x87, 0x87, 0xa7, 0x67, 0x67, 0x67, 0x68 })]
        public Time(int hours, int minutes, int seconds, int milliseconds)
        {
            this.hours = 0;
            this.minutes = 0;
            this.seconds = 0;
            this.milliseconds = 0;
            this.isNegative = false;
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
            this.milliseconds = milliseconds;
        }

        public virtual int getHours()
        {
            return this.hours;
        }

        public virtual int getMilliseconds()
        {
            return this.milliseconds;
        }

        public virtual int getMinutes()
        {
            return this.minutes;
        }

        public virtual int getSeconds()
        {
            return this.seconds;
        }

        public virtual double getTime()
        {
            return (double) ((((this.hours * 0x36ee80) + (this.minutes * 0xea60)) + (this.seconds * 0x3e8)) + this.milliseconds);
        }

        public virtual bool isNegative()
        {
            return this.isNegative;
        }

        public virtual void setHours(int hours)
        {
            this.hours = hours;
        }

        public virtual void setIsNegative(bool isNegative)
        {
            int num = (int) isNegative;
            this.isNegative = (bool) num;
        }

        public virtual void setMilliseconds(int milliseconds)
        {
            this.milliseconds = milliseconds;
        }

        public virtual void setMinutes(int minutes)
        {
            this.minutes = minutes;
        }

        public virtual void setSeconds(int seconds)
        {
            this.seconds = seconds;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x93)]
        public override string toString()
        {
            return new ZmanimFormatter().format(this);
        }
    }
}

