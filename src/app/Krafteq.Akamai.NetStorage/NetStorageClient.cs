namespace Krafteq.Akamai.NetStorage
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Interop;

    public class NetStorageClient : IDisposable
    {
        readonly NetStorageInterop interop;
        readonly FullPathFormatter fullPathFormatter;

        public NetStorageClient(NetStorageParams @params)
        {
            this.interop = new NetStorageInterop(@params);
            this.fullPathFormatter = new FullPathFormatter(@params.Host.OriginalString, @params.CPCode);
        }

        public async Task<DirResponse> DirAsync(DirectoryRequest request)
        {
            var fullPath = this.fullPathFormatter.Format(request.Path);
            var action = request.BuildAction();

            var response = await this.interop.SendAsync(new NetStorageRequest("GET", fullPath, action, null))
                .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            using (var stream = response.ContentStream)
            {
                var xmlModel = DirResponseXml.Deserialize(XElement.Load(stream));

                return new DirResponse(
                    xmlModel.Directory,
                    xmlModel.Entries.Select(NetStorageEntry.FromXml).ToReadOnlyCollection());
            }
        }

        public async Task<ListResponse> ListAsync(ListRequest request)
        {
            var fullPath = this.fullPathFormatter.Format(request.Path);
            var action = request.BuildAction();

            var response = await this.interop.SendAsync(new NetStorageRequest("GET", fullPath, action, null))
                .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return new ListResponse(new NetStorageEntry[0]);

            using (var stream = response.ContentStream)
            {
                var xmlModel = DirResponseXml.Deserialize(XElement.Load(stream));

                return new ListResponse(
                    xmlModel.Entries.Select(NetStorageEntry.FromXml).ToReadOnlyCollection());
            }
        }

        public async Task UploadAsync(UploadFileRequest request)
        {
            var fullPath = this.fullPathFormatter.Format(request.Path);
            var action = request.BuildAction();

            var response = await this.interop
                .SendAsync(new NetStorageRequest("PUT", fullPath, action, request.File.Stream))
                .ConfigureAwait(false);

            await this.EnsureSucceed(response).ConfigureAwait(false);
        }

        public async Task DeleteAsync(DeleteRequest request)
        {
            var fullPath = this.fullPathFormatter.Format(request.Path);
            var action = request.BuildAction();

            var response = await this.interop.SendAsync(new NetStorageRequest("POST", fullPath, action, null))
                .ConfigureAwait(false);

            // be tolerant to NotFound response by delete
            if (response.StatusCode == HttpStatusCode.NotFound)
                return;

            await this.EnsureSucceed(response).ConfigureAwait(false);
        }

        public async Task CreateDirectoryAsync(string path)
        {
            throw new NotImplementedException();
        }

        public async Task MTimeAsync(string path, DateTime? mtime = null)
        {
            throw new NotImplementedException();
        }

        public async Task RenameAsync(string path, string target)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveDirectoryAsync(string path)
        {
            throw new NotImplementedException();
        }

        public async Task SymlinkAsync(string path, string target)
        {
            throw new NotImplementedException();
        }

        public async Task<DirResponse> StatAsync(StatRequest request)
        {
            var fullPath = this.fullPathFormatter.Format(request.Path);
            var action = request.BuildAction();

            var response = await this.interop.SendAsync(new NetStorageRequest("GET", fullPath, action, null))
                .ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            using (var stream = response.ContentStream)
            {
                var xmlModel = DirResponseXml.Deserialize(XElement.Load(stream));

                return new DirResponse(
                    xmlModel.Directory,
                    xmlModel.Entries.Select(NetStorageEntry.FromXml).ToReadOnlyCollection());
            }
        }

        public async Task QuickDeleteAsync(string path)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            this.interop.Dispose();
        }

        async Task EnsureSucceed(NetStorageResponse response)
        {
            var code = (int) response.StatusCode;
            if (code >= 400)
            {
                throw new NetStorageResponseException(
                    response.StatusCode,
                    await StreamUtil.ReadFullyAsync(response.ContentStream).ConfigureAwait(false));
            }
        }
    }
}