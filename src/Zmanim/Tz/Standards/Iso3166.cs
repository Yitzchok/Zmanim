using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// Codes for the representation of names of countries and their subdivisions.
    /// http://en.wikipedia.org/wiki/ISO_3166
    /// </summary>
    [Serializable]
    public struct Iso3166
    {
        /// <summary>
        /// 
        /// </summary>
        public string TwoLetterCode;

        /// <summary>
        /// 
        /// </summary>
        public string CountryName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Iso3166"/> class.
        /// </summary>
        /// <param name="twoLetterCode">The two letter code.</param>
        /// <param name="countryName">Name of the country.</param>
        public Iso3166(string twoLetterCode, string countryName)
        {
            TwoLetterCode = twoLetterCode;
            CountryName = countryName;
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return TwoLetterCode;
        }
    }
}
