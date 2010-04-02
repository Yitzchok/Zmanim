namespace net.sourceforge.zmanim.util
{
    using IKVM.Attributes;
    using java.lang;
    using java.util;
    using System;
    using System.Runtime.CompilerServices;

    [SourceFile("Zman.java"), Implements(new string[] { "java.util.Comparator" }), InnerClass(null, Modifiers.Static)]
    internal sealed class Zman$1 : java.lang.Object, Comparator
    {
        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable((ushort) 0x45)]
        internal Zman$1()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0x15, 0x67, 0x67 })]
        public virtual int compare(object obj1, object obj2)
        {
            Zman zman = (Zman) obj1;
            Zman zman2 = (Zman) obj2;
            return zman.getZman().compareTo(zman2.getZman());
        }

        [HideFromJava]
        bool Comparator.Object;)Zequals(object obj1)
        {
            return java.lang.Object.instancehelper_equals(this, obj1);
        }
    }
}

