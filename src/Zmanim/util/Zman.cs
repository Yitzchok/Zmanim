// * Zmanim .NET API
// * Copyright (C) 2004-2010 Eliyahu Hershfeld
// *
// * Converted to C# by AdminJew
// *
// * This file is part of Zmanim .NET API.
// *
// * Zmanim .NET API is free software: you can redistribute it and/or modify
// * it under the terms of the GNU Lesser General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * Zmanim .NET API is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU Lesser General Public License for more details.
// *
// * You should have received a copy of the GNU Lesser General Public License
// * along with Zmanim.NET API.  If not, see <http://www.gnu.org/licenses/lgpl.html>.

using java.util;

namespace net.sourceforge.zmanim.util
{
    /// <summary>
    ///   Wrapper class for an astronomical time, mostly used to sort collections of
    ///   astronomical times.
    /// </summary>
    /// <author>Eliyahu Hershfeld</author>
    public class Zman
    {
        private long duration;
        private Date zman;
        private Date zmanDescription;
        private string zmanLabel;

        public Zman(Date date, string label)
        {
            zmanLabel = label;
            zman = date;
        }

        public Zman(long duration, string label)
        {
            zmanLabel = label;
            this.duration = duration;
        }

        public virtual long getDuration()
        {
            return duration;
        }

        public virtual Date getZman()
        {
            return zman;
        }

        public virtual Date getZmanDescription()
        {
            return zmanDescription;
        }

        public virtual string getZmanLabel()
        {
            return zmanLabel;
        }

        public virtual void setDuration(long duration)
        {
            this.duration = duration;
        }

        public virtual void setZman(Date date)
        {
            zman = date;
        }

        public virtual void setZmanDescription(Date zmanDescription)
        {
            this.zmanDescription = zmanDescription;
        }

        public virtual void setZmanLabel(string label)
        {
            zmanLabel = label;
        }
    }
}