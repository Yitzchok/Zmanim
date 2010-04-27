using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PublicDomain
{
    /// <summary>
    /// This simply extends the <see cref="Exception"/> class
    /// by adding a variable length parameter list in the basic
    /// constructor which takes the exception message, and then
    /// apply string.Format if necessary, which is an incredibly
    /// common expectation when throwing exceptions, and should have been
    /// part of the base exception class.
    /// </summary>
    [Serializable]
    [XmlType(Namespace = GlobalConstants.PublicDomainNamespace)]
    [SoapType(Namespace = GlobalConstants.PublicDomainNamespace)]
    public class BaseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Exception"/> class.
        /// </summary>
        public BaseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exception"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="formatParameters">The format parameters.</param>
        public BaseException(string message, params object[] formatParameters)
            : base(formatParameters != null && formatParameters.Length > 0 ? string.Format(message, formatParameters) : message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exception"/> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        /// <param name="message">The message.</param>
        /// <param name="formatParameters">The format parameters.</param>
        public BaseException(Exception inner, string message, params object[] formatParameters)
            : base(formatParameters != null && formatParameters.Length > 0 ? string.Format(message, formatParameters) : message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exception"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}