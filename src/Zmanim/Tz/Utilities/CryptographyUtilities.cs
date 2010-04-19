using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace PublicDomain
{
    /// <summary>
    /// 
    /// </summary>
    public static class CryptographyUtilities
    {
        /// <summary>
        /// Computes the SHA1 hash.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static byte[] ComputeSHA1Hash(string str)
        {
            byte[] data = StringUtilities.GetBytesFromString(str);
            return ComputeSHA1Hash(data);
        }

        /// <summary>
        /// Computes the SHA1 hash.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] ComputeSHA1Hash(byte[] data)
        {
            SHA1 shaM = new SHA1Managed();
            return shaM.ComputeHash(data);
        }
    }
}
