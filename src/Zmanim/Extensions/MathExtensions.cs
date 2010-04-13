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