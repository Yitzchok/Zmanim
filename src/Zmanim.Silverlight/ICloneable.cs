using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Supports cloning, which creates a new instance of a class with the same value as an existing instance.
    /// </summary>
    public interface ICloneable
    {
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        object Clone();
    }
}
