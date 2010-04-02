namespace net.sourceforge.zmanim.util
{
    using IKVM.Attributes;
    using java.lang;
    using java.util;
    using System;
    using System.Runtime.CompilerServices;

    [Implements(new string[] { "java.util.Comparator" }), SourceFile("Zman.java"), InnerClass(null, Modifiers.Static)]
    internal sealed class Zman$2 : java.lang.Object, Comparator
    {
        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x4d)]
        internal Zman$2()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0x1d, 0x67, 0x67 })]
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

