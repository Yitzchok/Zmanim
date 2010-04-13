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

namespace Zmanim.Extensions
{
    public static class MathExtensions
    {
        /// <summary>
        ///   Converts degrees to radians.
        /// </summary>
        /// <param name = "angle"></param>
        /// <returns></returns>
        public static double ToRadians(this double angle)
        {
            return Math.PI*angle/180.0;
        }

        /// <summary>
        ///   Converts radians to degrees.
        /// </summary>
        /// <param name = "angle"></param>
        /// <returns></returns>
        public static double ToDegree(this double angle)
        {
            return angle*(180.0/Math.PI);
        }
    }
}