using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// Various useful global constants.
    /// </summary>
    public static class GlobalConstants
    {
        /// <summary>
        /// 
        /// </summary>
        public const string PublicDomainMainVersion = "0.2.50";

        /// <summary>
        /// 
        /// </summary>
        public const string PublicDomainBuildVersion = "0";

        /// <summary>
        /// Current version of this code, in string form. In a standalone build,
        /// this is the assembly version.
        /// </summary>
        public const string PublicDomainVersion = PublicDomainMainVersion + ".0";

        /// <summary>
        /// Current version of this code, in string form. In a standalone build,
        /// this is the file version.
        /// </summary>
        public const string PublicDomainFileVersion = PublicDomainMainVersion + "." + PublicDomainBuildVersion;

        /// <summary>
        /// Static Initializer
        /// </summary>
        static GlobalConstants()
        {
            BitsInAByte = (int)Math.Pow(2, 3);
            BytesInAKilobyte = (int)Math.Pow(2, 10);
            BitsInAKilobyte = BitsInAByte * BytesInAKilobyte;
            BytesInAMegabyte = (int)Math.Pow(2, 20);
            BitsInAMegabyte = BitsInAByte * BytesInAMegabyte;
            BytesInAGigabyte = (long)Math.Pow(2, 30);
            BitsInAGigabyte = BitsInAByte * BytesInAGigabyte;
            BytesInATerabyte = (long)Math.Pow(2, 40);
            BitsInATerabyte = BitsInAByte * BytesInATerabyte;
            BytesInAPetabyte = (long)Math.Pow(2, 50);
            BitsInAPetabyte = BitsInAByte * BytesInAPetabyte;
            KilometersInAStatuteMile = ((FeetInAStatuteMile) * (InchesInAFoot) * (CentimetersInAnInch)) / (Math.Pow(10, 5));
        }

        /// <summary>
        /// The name of the PublicDomain assembly, if this is a standalone build. If
        /// this file is included in an existing project, this is purely a logical name.
        /// </summary>
        public const string PublicDomainName = "PublicDomain";

        /// <summary>
        /// Strong, public name of the PublicDomain assembly, if this is a standalone
        /// build. If this file is included in an existing project, this is meaningless.
        /// </summary>
        public const string PublicDomainStrongName = PublicDomainName + ", Version=" + PublicDomainVersion + ", Culture=neutral, PublicKeyToken=FD3F43B5776A962B";

        /// <summary>
        /// Fully qualified, absolute URL which acts as a namespace for the classes in the
        /// PublicDomain.
        /// Always ends in a trailing slash.
        /// </summary>
        public const string PublicDomainNamespace = "http://www.codeplex.com/PublicDomain/";

        /// <summary>
        /// Always ends in a trailing slash.
        /// C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\
        /// </summary>
        public const string DotNetFrameworkLocation20 = @"C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\";

        /// <summary>
        /// The number of bits in 1 Byte (8)
        /// </summary>
        public static readonly int BitsInAByte;

        /// <summary>
        /// The number of bytes in 1KB (1024).
        /// </summary>
        public static readonly int BytesInAKilobyte;

        /// <summary>
        /// The number of bits in 1KB (8192).
        /// </summary>
        public static readonly int BitsInAKilobyte;

        /// <summary>
        /// The number of bytes in 1MB (1048576).
        /// </summary>
        public static readonly int BytesInAMegabyte;

        /// <summary>
        /// The number of bits in 1MB (8388608).
        /// </summary>
        public static readonly int BitsInAMegabyte;

        /// <summary>
        /// The number of bytes in 1GB (1073741824).
        /// </summary>
        public static readonly long BytesInAGigabyte;

        /// <summary>
        /// The number of bits in 1GB (8589934592).
        /// </summary>
        public static readonly long BitsInAGigabyte;

        /// <summary>
        /// The number of bytes in 1TB (1099511627776).
        /// </summary>
        public static readonly long BytesInATerabyte;

        /// <summary>
        /// The number of bits in 1TB (8796093022208).
        /// </summary>
        public static readonly long BitsInATerabyte;

        /// <summary>
        /// The number of bytes in 1PB (1125899906842624).
        /// </summary>
        public static readonly long BytesInAPetabyte;

        /// <summary>
        /// The number of bits in 1PB (9007199254740992).
        /// </summary>
        public static readonly long BitsInAPetabyte;

        /// <summary>
        /// A reasonable default block size for block reading/writing to and from
        /// a Stream.
        /// </summary>
        public const int DefaultStreamBlockSize = 1024;

        /// <summary>
        /// A reasonable default timeout value, in milliseconds, for a
        /// small process to timeout.
        /// </summary>
        public const int DefaultExecuteSmallProcessTimeout = 60000;

        /// <summary>
        /// 5280
        /// http://scienceworld.wolfram.com/physics/Mile.html
        /// </summary>
        public const int FeetInAStatuteMile = 5280;

        /// <summary>
        /// 0.3937007874015748031496062992126
        /// http://scienceworld.wolfram.com/physics/Inch.html
        /// </summary>
        public const double InchesInACentimeter = 0.3937007874015748031496062992126;

        /// <summary>
        /// 2.54
        /// http://scienceworld.wolfram.com/physics/Inch.html
        /// </summary>
        public const double CentimetersInAnInch = 2.54;

        /// <summary>
        /// 12
        /// http://scienceworld.wolfram.com/physics/Inch.html
        /// </summary>
        public const int InchesInAFoot = 12;

        /// <summary>
        /// 3
        /// http://scienceworld.wolfram.com/physics/Yard.html
        /// </summary>
        public const int FeetInAYard = 3;

        /// <summary>
        /// http://scienceworld.wolfram.com/physics/Mile.html
        /// </summary>
        public static readonly double KilometersInAStatuteMile;

        /// <summary>
        /// 3963.19
        /// http://scienceworld.wolfram.com/astronomy/EarthRadius.html
        /// </summary>
        public const double EarthEquatorialRadiusInStatuteMiles = 3963.19;

        /// <summary>
        /// 3443.9
        /// </summary>
        public const double EarthEquatorialRadiusInNauticalMiles = 3443.9;

        /// <summary>
        /// 6378.137
        /// http://scienceworld.wolfram.com/astronomy/EarthRadius.html
        /// </summary>
        public const double EarthEquatorialRadiusInKilometers = 6378.137;

        /// <summary>
        /// 24901.5
        /// http://scienceworld.wolfram.com/astronomy/EarthRadius.html
        /// </summary>
        public const double EarthEquatorialCircumferenceInStatuteMiles = 24901.5;

        /// <summary>
        /// 40075
        /// http://scienceworld.wolfram.com/astronomy/EarthRadius.html
        /// </summary>
        public const int EarthEquatorialCircumferenceInKilometers = 40075;

        /// <summary>
        /// 
        /// </summary>
        public const double EarthEquatorialDiameterInStatuteMiles = EarthEquatorialRadiusInStatuteMiles * 2;

        /// <summary>
        /// 
        /// </summary>
        public const double EarthEquatorialDiameterInNauticalMiles = EarthEquatorialRadiusInNauticalMiles * 2;

        /// <summary>
        /// 
        /// </summary>
        public const double EarthEquatorialDiameterInKilometers = EarthEquatorialRadiusInKilometers * 2;

        /// <summary>
        /// The default installation diretory of a standalone PublicDomain assembly.
        /// Always ends in a trailing slash.
        /// </summary>
        public const string PublicDomainDefaultInstallLocation = @"C:\Program Files\Public Domain\";

        /// <summary>
        /// Represents the string (50 characters): 
        /// ==================================================
        /// </summary>
        public const string DividerEquals = "==================================================";

        /// <summary>
        /// 
        /// </summary>
        public const string LogClassDatabase = "Database";
    }

    /// <summary>
    /// 
    /// </summary>
    public delegate void CallbackNoArgs();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rock"></param>
    public delegate void CallbackWithRock(object rock);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    public delegate void CallbackWithString(string str);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="i"></param>
    public delegate void CallbackWithInt(int i);
}
