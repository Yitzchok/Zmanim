namespace net.sourceforge.zmanim.util
{
    using IKVM.Attributes;
    using java.lang;
    using java.util;
    using System;
    using System.Runtime.CompilerServices;

    internal sealed class Zman3 : java.lang.Object, Comparator
    {
        internal Zman3()
        {
        }

        public virtual int compare(object obj1, object obj2)
        {
            Zman zman = (Zman) obj1;
            Zman zman2 = (Zman) obj2;
            return ((zman.getDuration() != zman2.getDuration()) ? ((zman.getDuration() <= zman2.getDuration()) ? -1 : 1) : 0);
        }

        [HideFromJava]
        bool Comparator.Object;)Zequals(object obj1)
        {
            return java.lang.Object.instancehelper_equals(this, obj1);
        }
    }
}

