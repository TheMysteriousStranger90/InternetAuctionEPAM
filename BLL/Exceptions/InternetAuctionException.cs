using System;
using System.Runtime.Serialization;

namespace BLL.Exceptions
{
    /// <summary>
    /// Basic Exception
    /// </summary>
    [Serializable]
    public class InternetAuctionException : Exception
    {
        private static readonly string DefaultMessage = "Internet Auction exception was thrown.";

        public InternetAuctionException() : base(DefaultMessage)
        {

        }

        public InternetAuctionException(string message) : base(message)
        {

        }

        public InternetAuctionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InternetAuctionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
