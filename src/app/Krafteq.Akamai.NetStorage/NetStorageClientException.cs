namespace Krafteq.Akamai.NetStorage
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;

    public class NetStorageClientException : Exception
    {
        public NetStorageClientException()
        {
        }

        protected NetStorageClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NetStorageClientException(string message) : base(message)
        {
        }

        public NetStorageClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class NetStorageResponseException : NetStorageClientException
    {
        public HttpStatusCode StatusCode { get; }

        public byte[] Content { get; }

        public NetStorageResponseException(HttpStatusCode statusCode, byte[] content)
        {
            this.StatusCode = statusCode;
            this.Content = content;
        }
    }
}