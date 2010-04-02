namespace net.sourceforge.zmanim.util
{
    using IKVM.Attributes;
    using java.lang;
    using java.util;
    using System;
    using System.Runtime.CompilerServices;

    public class Zman : java.lang.Object
    {
        private long duration;
        private Date zman;
        private Date zmanDescription;
        private string zmanLabel;

        public Zman(Date date, string label)
        {
            this.zmanLabel = label;
            this.zman = date;
        }

        public Zman(long duration, string label)
        {
            this.zmanLabel = label;
            this.duration = duration;
        }

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

