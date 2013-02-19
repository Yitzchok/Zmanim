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

namespace Zmanim.Utilities
{
    /// <summary>
    /// A class that represents a numeric time. Times that represent a time of day
    /// are stored as <seealso cref="DateTime"/>s in this API. The time class is used to
    /// represent numeric time such as the time in hours, minutes, seconds and
    /// milliseconds of a
    /// <see cref="AstronomicalCalendar.GetTemporalHour()">temporal hour</see>.
    /// </summary>
    /// <author>Eliyahu Hershfeld</author>
    public class Time
    {
        private const int SECOND_MILLIS = 1000;

        private const int MINUTE_MILLIS = SECOND_MILLIS * 60;

        private const int HOUR_MILLIS = MINUTE_MILLIS * 60;

        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> class.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="milliseconds">The milliseconds.</param>
        public Time(int hours, int minutes, int seconds, int milliseconds)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Milliseconds = milliseconds;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Time"/> class.
        /// </summary>
        /// <param name="millis">The millis.</param>
        public Time(double millis)
            : this((int)millis)
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
                IsNegative = true;
                millis = Math.Abs(millis);
            }

            Hours = timeSpan.Hours;
            Minutes = timeSpan.Minutes;
            Seconds = timeSpan.Seconds;
            Milliseconds = timeSpan.Milliseconds;
        }

        /// <summary>
        /// Determines whether this instance is negative.
        /// </summary>
        /// <value>
        ///   &lt;c&gt;true&lt;/c&gt; if this instance is negative; otherwise, &lt;c&gt;false&lt;/c&gt;.
        /// </value>
        public virtual bool IsNegative { get; set; }

        /// <summary>
        /// Gets the hours.
        /// </summary>
        /// <value>Returns the hour.</value>
        public virtual int Hours { get; set; }

        /// <summary>
        /// Gets the minutes.
        /// </summary>
        /// <value>Returns the minutes.</value>
        public virtual int Minutes { get; set; }

        /// <summary>
        /// Gets the seconds.
        /// </summary>
        /// <value>Returns the seconds.</value>
        public virtual int Seconds { get; set; }

        /// <summary>
        /// Gets the milliseconds.
        /// </summary>
        /// <value>Returns the milliseconds.</value>
        public virtual int Milliseconds { get; set; }

        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <returns></returns>
        public virtual double GetTime()
        {
            return Hours * HOUR_MILLIS + Minutes * MINUTE_MILLIS + Seconds * SECOND_MILLIS + Milliseconds;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return new ZmanimFormatter().Format(this);
        }
    }
}