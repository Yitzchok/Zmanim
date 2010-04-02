namespace net.sourceforge.zmanim.util
{
    using IKVM.Attributes;
    using java.lang;
    using java.util;
    using System;
    using System.Runtime.CompilerServices;

    [SourceFile("Zman.java"), InnerClass(null, Modifiers.Static), Implements(new string[] { "java.util.Comparator" })]
    internal sealed class Zman$3 : java.lang.Object, Comparator
    {
        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x55)]
        internal Zman$3()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0x25, 0x67, 0x67 })]
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

