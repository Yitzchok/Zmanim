using IKVM.Runtime;
using IKVM.Attributes;
using java.lang;
using System;
using System.Runtime.CompilerServices;

namespace net.sourceforge.zmanim.util
{

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

        public Time(double millis)
            : this((int)millis)
        {
        }

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
            return (double)((((this.hours * 0x36ee80) + (this.minutes * 0xea60)) + (this.seconds * 0x3e8)) + this.milliseconds);
        }

        public virtual bool IsNegative()
        {
            return this.isNegative;
        }

        public virtual void setHours(int hours)
        {
            this.hours = hours;
        }

        public virtual void setIsNegative(bool isNegative)
        {
            this.isNegative = isNegative;
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
    }
}

