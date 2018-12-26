using System;
using System.Runtime.Serialization;

namespace Vivelin.Home.Twitch
{
    /// <summary>
    /// Represents errors from Twitch API calls.
    /// </summary>
    [Serializable]
    public class TwitchException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwitchException"/>
        /// class.
        /// </summary>
        public TwitchException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitchException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TwitchException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitchException"/> class
        /// with a specified error message, error type and status code.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="error">The type of error that occurred.</param>
        /// <param name="status">The status code of the error.</param>
        public TwitchException(string message, string error, int status) : base(message)
        {
            Error = error;
            Status = status;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitchException"/> class
        /// with a specified error message and a reference to the inner exception
        /// that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null
        /// reference (Nothing in Visual Basic) if no inner exception is
        /// specified.
        /// </param>
        public TwitchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitchException"/> class
        /// with serialized data.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"></see> that holds the serialized
        /// object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"></see> that contains contextual
        /// information about the source or destination.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="info">info</paramref> parameter is null.
        /// </exception>
        /// <exception cref="SerializationException">
        /// The class name is null or <see
        /// cref="P:System.Exception.HResult"></see> is zero (0).
        /// </exception>
        protected TwitchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Error = info.GetString("Error");
            Status = info.GetInt32("Status");
        }

        /// <summary>
        /// Gets the type of error that occurred.
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Gets the status code of the error.
        /// </summary>
        public int Status { get; }

        /// <summary>
        /// Sets the <see cref="SerializationInfo"/> with information about the
        /// exception.
        /// </summary>
        /// <param name="info">
        /// The <see cref="SerializationInfo"/> that holds the serialized object
        /// data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="StreamingContext"></see> that contains contextual
        /// information about the source or destination.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="info"/> is <c>null</c>.
        /// </exception>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Error", Error);
            info.AddValue("Status", Status);
        }
    }
}