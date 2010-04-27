using System;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// Methods to manipulate arrays.
    /// </summary>
    internal static class ArrayUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static T[] Remove<T>(T[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (index < 0 || index >= array.Length)
            {
                throw new IndexOutOfRangeException("Cannot remove element at index " + index);
            }

            // Crush the elemnts from the end down to the index
            for (int i = index; i < array.Length - 1; i++)
            {
                array[i] = array[i + 1];
            }

            Array.Resize<T>(ref array, array.Length - 1);
            return array;
        }
    }
}
