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
    /// Wrapper class for an astronomical time, mostly used to sort collections of
    /// astronomical times.
    /// </summary>
    /// <author>Eliyahu Hershfeld</author>
    public class Zman
    {
        private long duration;
        private DateTime zman;
        private string zmanLabel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Zman"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="label">The label.</param>
        public Zman(DateTime date, string label)
        {
            zmanLabel = label;
            zman = date;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Zman"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="label">The label.</param>
        public Zman(long duration, string label)
        {
            zmanLabel = label;
            this.duration = duration;
        }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <returns></returns>
        public virtual long getDuration()
        {
            return duration;
        }

        /// <summary>
        /// Gets the zman.
        /// </summary>
        /// <returns></returns>
        public virtual DateTime getZman()
        {
            return zman;
        }

        /// <summary>
        /// Gets the zman label.
        /// </summary>
        /// <returns></returns>
        public virtual string getZmanLabel()
        {
            return zmanLabel;
        }

        /// <summary>
        /// Sets the duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        public virtual void setDuration(long duration)
        {
            this.duration = duration;
        }

        /// <summary>
        /// Sets the zman.
        /// </summary>
        /// <param name="date">The date.</param>
        public virtual void setZman(DateTime date)
        {
            zman = date;
        }

        /// <summary>
        /// Sets the zman label.
        /// </summary>
        /// <param name="label">The label.</param>
        public virtual void setZmanLabel(string label)
        {
            zmanLabel = label;
        }
    }
}