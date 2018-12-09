namespace Akamai.NetStorage.Interop
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;

    class NetStorageResponse : IDisposable
    {
        readonly HttpResponseMessage responseMessage;
        public HttpStatusCode StatusCode { get; }

        public Stream ContentStream { get; }

        public NetStorageResponse(
            HttpResponseMessage responseMessage,
            Stream contentStream)
        {
            this.responseMessage = responseMessage ?? throw new ArgumentNullException(nameof(responseMessage));
            this.ContentStream = contentStream;
            this.StatusCode = this.responseMessage.StatusCode;
        }

        public void Dispose()
        {
            this.responseMessage.Dispose();
        }
    }
}