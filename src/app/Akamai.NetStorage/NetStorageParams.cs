namespace Akamai.NetStorage
{
    using System;
    using System.Net.Http;
    using Interop;

    public class NetStorageParams
    {
        public Uri Host { get; }

        public string UserName { get; }

        public string SecretKey { get; }

        public string CPCode { get; }

        public Action<HttpClientHandler> HttpClientCustomization { get; }

        public NetStorageParams(
            Uri host, 
            string userName, 
            string secretKey, 
            string cpCode, 
            Action<HttpClientHandler> httpClientCustomization)
        {
            this.Host = host ?? throw new ArgumentNullException(nameof(host));
            this.UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            this.SecretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
            this.CPCode = cpCode;
            this.HttpClientCustomization = httpClientCustomization ?? (x => { });
        }

        internal SignParams SignParams() => new SignParams(this.UserName, this.SecretKey, Signature.HMACSHA256);
    }
}