namespace Zmanim.Utilities
{
    /// <summary>
    /// System.BitConverter.DoubleToInt64Bits method is not presents in Silverlight 3.
    /// </summary>
    internal class BitConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static long DoubleToInt64Bits(double x)
        {
            byte[] bytes = System.BitConverter.GetBytes(x);
            long value = System.BitConverter.ToInt64(bytes, 0);
            return value;
        }
    }
}