namespace net.sourceforge.zmanim.util
{
    using java.util;

    public class Zman
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

