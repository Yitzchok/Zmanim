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
    /// Wrapper class for an astronomical time, mostly used to sort collections of
    /// astronomical times.
    /// </summary>
    /// <author>Eliyahu Hershfeld</author>
    public class Zman
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Zman"/> class.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="label">The label.</param>
        public Zman(DateTime date, string label)
        {
            ZmanLabel = label;
            ZmanTime = date;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Zman"/> class.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="label">The label.</param>
        public Zman(long duration, string label)
        {
            ZmanLabel = label;
            this.Duration = duration;
        }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value></value>
        public virtual long Duration { get; set; }

        /// <summary>
        /// Gets the zman.
        /// </summary>
        /// <value></value>
        public virtual DateTime ZmanTime { get; set; }

        /// <summary>
        /// Gets the zman label.
        /// </summary>
        /// <value></value>
        public virtual string ZmanLabel { get; set; }
    }
}