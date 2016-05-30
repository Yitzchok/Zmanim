// * Zmanim .NET API
// * Copyright (C) 2004-2011 Eliyahu Hershfeld
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

namespace Zmanim
{
    /// <summary>
    /// An Object representing a Daf in the Daf Yomi cycle.
    /// 
    /// @author &copy; Eliyahu Hershfeld 2011
    /// @version 0.0.1
    /// </summary>
    public class Daf
    {
        private int masechtaNumber;
        private int page;

        private static string[] masechtosBavliTransliterated = { "Berachos", "Shabbos", "Eruvin", "Pesachim", "Shekalim", "Yoma", "Sukkah", "Beitzah", "Rosh Hashana", "Taanis", "Megillah", "Moed Katan", "Chagigah", "Yevamos", "Kesubos", "Nedarim", "Nazir", "Sotah", "Gitin", "Kiddushin", "Bava Kamma", "Bava Metzia", "Bava Basra", "Sanhedrin", "Makkos", "Shevuos", "Avodah Zarah", "Horiyos", "Zevachim", "Menachos", "Chullin", "Bechoros", "Arachin", "Temurah", "Kerisos", "Meilah", "Kinnim", "Tamid", "Midos", "Niddah" };

        private static string[] masechtosBavli = { "\u05D1\u05E8\u05DB\u05D5\u05EA", "\u05E9\u05D1\u05EA", "\u05E2\u05D9\u05E8\u05D5\u05D1\u05D9\u05DF", "\u05E4\u05E1\u05D7\u05D9\u05DD", "\u05E9\u05E7\u05DC\u05D9\u05DD", "\u05D9\u05D5\u05DE\u05D0", "\u05E1\u05D5\u05DB\u05D4", "\u05D1\u05D9\u05E6\u05D4", "\u05E8\u05D0\u05E9 \u05D4\u05E9\u05E0\u05D4", "\u05EA\u05E2\u05E0\u05D9\u05EA", "\u05DE\u05D2\u05D9\u05DC\u05D4", "\u05DE\u05D5\u05E2\u05D3 \u05E7\u05D8\u05DF", "\u05D7\u05D2\u05D9\u05D2\u05D4", "\u05D9\u05D1\u05DE\u05D5\u05EA", "\u05DB\u05EA\u05D5\u05D1\u05D5\u05EA", "\u05E0\u05D3\u05E8\u05D9\u05DD", "\u05E0\u05D6\u05D9\u05E8", "\u05E1\u05D5\u05D8\u05D4", "\u05D2\u05D9\u05D8\u05D9\u05DF", "\u05E7\u05D9\u05D3\u05D5\u05E9\u05D9\u05DF", "\u05D1\u05D1\u05D0 \u05E7\u05DE\u05D0", "\u05D1\u05D1\u05D0 \u05DE\u05E6\u05D9\u05E2\u05D0", "\u05D1\u05D1\u05D0 \u05D1\u05EA\u05E8\u05D0", "\u05E1\u05E0\u05D4\u05D3\u05E8\u05D9\u05DF", "\u05DE\u05DB\u05D5\u05EA", "\u05E9\u05D1\u05D5\u05E2\u05D5\u05EA", "\u05E2\u05D1\u05D5\u05D3\u05D4 \u05D6\u05E8\u05D4", "\u05D4\u05D5\u05E8\u05D9\u05D5\u05EA", "\u05D6\u05D1\u05D7\u05D9\u05DD", "\u05DE\u05E0\u05D7\u05D5\u05EA", "\u05D7\u05D5\u05DC\u05D9\u05DF", "\u05D1\u05DB\u05D5\u05E8\u05D5\u05EA", "\u05E2\u05E8\u05DB\u05D9\u05DF", "\u05EA\u05DE\u05D5\u05E8\u05D4", "\u05DB\u05E8\u05D9\u05EA\u05D5\u05EA", "\u05DE\u05E2\u05D9\u05DC\u05D4", "\u05EA\u05DE\u05D9\u05D3", "\u05E7\u05D9\u05E0\u05D9\u05DD", "\u05DE\u05D9\u05D3\u05D5\u05EA", "\u05E0\u05D3\u05D4" };

        /// <returns> the masechtaNumber </returns>
        public virtual int MasechtaNumber
        {
            get
            {
                return masechtaNumber;
            }
            set
            {
                this.masechtaNumber = value;
            }
        }


        /// <summary>
        /// Constructor that creates a Daf setting the <seealso cref="#setMasechtaNumber(int) masechta Number"/> and
        /// <seealso cref="#setDaf(int) daf Number"/>
        /// </summary>
        /// <param name="masechtaNumber"> </param>
        /// <param name="page"> </param>
        public Daf(int masechtaNumber, int page)
        {
            this.masechtaNumber = masechtaNumber;
            this.page = page;
        }

        /// <summary>
        /// Returns the daf (page number) of the Daf Yomi </summary>
        /// <returns> the daf (page number) of the Daf Yomi </returns>
        public virtual int Page
        {
            get
            {
                return page;
            }
            set
            {
                this.page = value;
            }
        }


        /// <summary>
        /// Returns the transliterated name of the masechta (tractate) of the Daf Yomi. The list of mashechtos is: Berachos,
        /// Shabbos, Eruvin, Pesachim, Shekalim, Yoma, Sukkah, Beitzah, Rosh Hashana, Taanis, Megillah, Moed Katan, Chagigah,
        /// Yevamos, Kesubos, Nedarim, Nazir, Sotah, Gitin, Kiddushin, Bava Kamma, Bava Metzia, Bava Basra, Sanhedrin,
        /// Makkos, Shevuos, Avodah Zarah, Horiyos, Zevachim, Menachos, Chullin, Bechoros, Arachin, Temurah, Kerisos, Meilah,
        /// Kinnim, Tamid, Midos and Niddah.
        /// </summary>
        /// <returns> the transliterated name of the masechta (tractate) of the Daf Yomi such as Berachos. </returns>
        public virtual string MasechtaTransliterated
        {
            get
            {
                return masechtosBavliTransliterated[masechtaNumber];
            }
        }

        /// <summary>
        /// Returns the masechta (tractate) of the Daf Yomi in Hebrew, It will return
        /// &#x05D1;&#x05E8;&#x05DB;&#x05D5;&#x05EA; for Berachos.
        /// </summary>
        /// <returns> the masechta (tractate) of the Daf Yomi in Hebrew, It will return
        ///         &#x05D1;&#x05E8;&#x05DB;&#x05D5;&#x05EA; for Berachos. </returns>
        public virtual string Masechta
        {
            get
            {
                return masechtosBavli[masechtaNumber];
            }
        }
    }
}