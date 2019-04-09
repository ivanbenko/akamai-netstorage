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

        public async Task DeleteAsync(string path)
        {
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

        public async Task CreateDirectoryAsync(string path)
        {

        }

        public async Task MTimeAsync(string path, DateTime? mtime = null)
        {

        }

        public async Task RenameAsync(string path, string target)
        {

        }

        public async Task RemoveDirectoryAsync(string path)
        {

        }

        public async Task SymlinkAsync(string path, string target)
        {

        }

        public async Task<StatResponse> StatAsync(string path)
        {
            return new StatResponse();
        }

        public async Task QuickDeleteAsync(string path)
        {

        }

        public async Task UploadAsync(string path, UploadFile file)
        {

        }

        public void Dispose()
        {
            this.interop.Dispose();
        }
        
    }

}