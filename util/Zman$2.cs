namespace net.sourceforge.zmanim.util
{
    using IKVM.Attributes;
    using java.lang;
    using java.util;
    using System;
    using System.Runtime.CompilerServices;

    internal sealed class Zman2 : java.lang.Object, Comparator
    {
        internal Zman2()
        {
        }

        public virtual int compare(object obj1, object obj2)
        {
            Zman zman = (Zman) obj1;
            Zman zman2 = (Zman) obj2;
            return java.lang.String.instancehelper_compareTo(zman.getZmanLabel(), zman2.getZmanLabel());
        }

        [HideFromJava]
        bool Comparator.Object;)Zequals(object obj1)
        {
            return java.lang.Object.instancehelper_equals(this, obj1);
        }
    }
}

