namespace net.sourceforge.zmanim.util
{
    using IKVM.Attributes;
    using java.lang;
    using java.util;
    using System;
    using System.Runtime.CompilerServices;

    public class Zman : java.lang.Object
    {
        [Modifiers(Modifiers.Static | Modifiers.Public | Modifiers.Final)]
        public static Comparator DATE_ORDER = new Zman$1();
        private long duration;
        [Modifiers(Modifiers.Static | Modifiers.Public | Modifiers.Final)]
        public static Comparator DURATION_ORDER = new Zman$3();
        [Modifiers(Modifiers.Static | Modifiers.Public | Modifiers.Final)]
        public static Comparator NAME_ORDER = new Zman$2();
        private Date zman;
        private Date zmanDescription;
        private string zmanLabel;

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0x9f, 0xb1, 0x68, 0x67, 0x67 })]
        public Zman(Date date, string label)
        {
            this.zmanLabel = label;
            this.zman = date;
        }

        [MethodImpl(MethodImplOptions.NoInlining), LineNumberTable(new byte[] { 0x9f, 0xb6, 0x68, 0x67, 0x67 })]
        public Zman(long duration, string label)
        {
            this.zmanLabel = label;
            this.duration = duration;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void __<clinit>()
        {
        }

        public virtual long getDuration()
        {
            return this.duration;
        }

        public virtual Date getZman()
        {
            return this.zman;
        }

        public virtual Date getZmanDescription()
        {
            return this.zmanDescription;
        }

        public virtual string getZmanLabel()
        {
            return this.zmanLabel;
        }

        public virtual void setDuration(long duration)
        {
            this.duration = duration;
        }

        public virtual void setZman(Date date)
        {
            this.zman = date;
        }

        public virtual void setZmanDescription(Date zmanDescription)
        {
            this.zmanDescription = zmanDescription;
        }

        public virtual void setZmanLabel(string label)
        {
            this.zmanLabel = label;
        }
    }
}

