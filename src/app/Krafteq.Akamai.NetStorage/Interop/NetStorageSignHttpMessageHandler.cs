namespace Krafteq.Akamai.NetStorage.Interop
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    class NetStorageSignHttpMessageHandler : HttpClientHandler
    {
        readonly SignParams signParams;

        readonly byte[] secretKey;

        public NetStorageSignHttpMessageHandler(
            SignParams signParams)
        {
            this.signParams = signParams;
            this.secretKey = Encoding.UTF8.GetBytes(signParams.SecretKey);
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            this.Sign(request);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        void Sign(HttpRequestMessage request)
        {
            var action = this.GetHeaderValue(request, Headers.Action);
            var authData = this.GetAuthDataHeaderValue();
            var authSign = this.GetAuthSignHeaderValue(request.RequestUri, action, authData);

            request.Headers.Add(Headers.AuthData, authData);
            request.Headers.Add(Headers.AuthSign, authSign);
        }

        string GetAuthDataHeaderValue() => $"{this.signParams.Signature.VersionId}, 0.0.0.0, 0.0.0.0, " +
                                                    $"{DateTime.UtcNow.GetEpochSeconds()}, " +
                                                    $"{new Random().Next()}, " +
                                                    $"{this.signParams.UserName}";

        string GetAuthSignHeaderValue(Uri requestUri, string action, string authData)
        {
            var signDataString = $"{authData}{requestUri.AbsolutePath}\n{Headers.Action.ToLower()}:{action}\n";
            var signData = Encoding.UTF8.GetBytes(signDataString);

            var signedData = this.signParams.Signature.Sign(signData, this.secretKey);
            return Convert.ToBase64String(signedData);
        }

        string GetHeaderValue(HttpRequestMessage request, string header) =>
            request.Headers.TryGetValues(Headers.Action, out var values)
                ? values.FirstOrDefault()
                : null;
    }
}