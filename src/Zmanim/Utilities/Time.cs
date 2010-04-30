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

namespace net.sourceforge.zmanim.util
{
    /// <summary>
    ///   A class that represents a numeric time. Times that represent a time of day
    ///   are stored as <seealso cref = "DateTime" />s in this API. The time class is used to
    ///   represent numeric time such as the time in hours, minutes, seconds and
    ///   milliseconds of a
    ///   <see cref = "net.sourceforge.zmanim.AstronomicalCalendar.getTemporalHour()">temporal hour"/>.</see>.
    /// </summary>
    /// <author>Eliyahu Hershfeld</author>
    public class Time
    {
        private const int SECOND_MILLIS = 1000;

        private const int MINUTE_MILLIS = SECOND_MILLIS*60;

        private const int HOUR_MILLIS = MINUTE_MILLIS*60;

        private int hours;
        private bool isNegative;
        private int milliseconds;

        private int minutes;

        private int seconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> class.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="milliseconds">The milliseconds.</param>
        public Time(int hours, int minutes, int seconds, int milliseconds)
        {
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
            this.milliseconds = milliseconds;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> class.
        /// </summary>
        /// <param name="millis">The millis.</param>
        public Time(double millis)
            : this((int) millis)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> class.
        /// </summary>
        /// <param name="millis">The millis.</param>
        public Time(int millis)
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(millis);
            if (millis < 0)
            {
                isNegative = true;
                millis = Math.Abs(millis);
            }
            hours = timeSpan.Hours;

            minutes = timeSpan.Minutes;

            seconds = timeSpan.Seconds;

            milliseconds = timeSpan.Milliseconds;
        }

        /// <summary>
        /// Determines whether this instance is negative.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is negative; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsNegative()
        {
            return isNegative;
        }

        /// <summary>
        /// Sets the is negative.
        /// </summary>
        /// <param name="isNegative">if set to <c>true</c> [is negative].</param>
        public virtual void setIsNegative(bool isNegative)
        {
            this.isNegative = isNegative;
        }


        /// <summary>
        /// Gets the hours.
        /// </summary>
        /// <returns>Returns the hour.</returns>
        public virtual int getHours()
        {
            return hours;
        }


        /// <summary>
        /// Sets the hours.
        /// </summary>
        /// <param name="hours">The hours to set.</param>
        public virtual void setHours(int hours)
        {
            this.hours = hours;
        }


        /// <summary>
        /// Gets the minutes.
        /// </summary>
        /// <returns>Returns the minutes.</returns>
        public virtual int getMinutes()
        {
            return minutes;
        }


        /// <summary>
        /// Sets the minutes.
        /// </summary>
        /// <param name="minutes">The minutes to set.</param>
        public virtual void setMinutes(int minutes)
        {
            this.minutes = minutes;
        }


        /// <summary>
        /// Gets the seconds.
        /// </summary>
        /// <returns>Returns the seconds.</returns>
        public virtual int getSeconds()
        {
            return seconds;
        }


        /// <summary>
        /// Sets the seconds.
        /// </summary>
        /// <param name="seconds">The seconds to set.</param>
        public virtual void setSeconds(int seconds)
        {
            this.seconds = seconds;
        }


        /// <summary>
        /// Gets the milliseconds.
        /// </summary>
        /// <returns>Returns the milliseconds.</returns>
        public virtual int getMilliseconds()
        {
            return milliseconds;
        }


        /// <summary>
        /// Sets the milliseconds.
        /// </summary>
        /// <param name="milliseconds">The milliseconds to set.</param>
        public virtual void setMilliseconds(int milliseconds)
        {
            this.milliseconds = milliseconds;
        }

        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <returns></returns>
        public virtual double getTime()
        {
            return hours*HOUR_MILLIS + minutes*MINUTE_MILLIS + seconds*SECOND_MILLIS + milliseconds;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return new ZmanimFormatter().format(this);
        }
    }
}