// * Zmanim Java API
// * Copyright (C) 2004-2010 Eliyahu Hershfeld
// *
// * Converted to C# by AdminJew
// *
// * This program is free software; you can redistribute it and/or modify it under the terms of the
// * GNU General Public License as published by the Free Software Foundation; either version 2 of the
// * License, or (at your option) any later version.
// *
// * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// * even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// * General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License along with this program; if
// * not, write to the Free Software Foundation, Inc. 59 Temple Place - Suite 330, Boston, MA
// * 02111-1307, USA or connect to: http://www.fsf.org/copyleft/gpl.html

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

