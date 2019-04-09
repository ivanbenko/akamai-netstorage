namespace Krafteq.Akamai.NetStorage.Interop
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    class NetStorageInterop : IDisposable
    {
        const string KitVersion = "CSharp/3.51";
        
        readonly HttpClient httpClient;

        public NetStorageInterop(NetStorageParams @params)
        {
            var handler = new NetStorageSignHttpMessageHandler(@params.SignParams());

            @params.HttpClientCustomization(handler);

            this.httpClient = new HttpClient(handler)
            {
                BaseAddress = @params.Host
            };
        }

        public async Task<NetStorageResponse> SendAsync(NetStorageRequest request)
        {
            var httpRequest = new HttpRequestMessage(
                new HttpMethod(request.Method),
                request.Path);

            this.SetHeaders(request, httpRequest);

            var httpResponse = await this.httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead)
                .ConfigureAwait(false);

            return new NetStorageResponse(
                httpResponse,
                await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false)
            );
        }

        void SetHeaders(NetStorageRequest request, HttpRequestMessage httpRequest)
        {
            httpRequest.Headers.Add(Headers.KitVersion, KitVersion);
            httpRequest.Headers.Add(Headers.Action, request.Action);
        }

        public void Dispose()
        {
            this.httpClient.Dispose();
        }
    }
}