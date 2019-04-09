namespace Krafteq.Akamai.NetStorage.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using FluentAssertions;
    using Xunit;

    public class NetStorageClientTests
    {
        readonly NetStorageClient sut;
        readonly ApiTestParams apiParams;

        public NetStorageClientTests()
        {
            this.apiParams = ApiTestParams.GetFromEnvironment();
            this.sut = new NetStorageClient(new NetStorageParams(
                new Uri(this.apiParams.Host),
                this.apiParams.User,
                this.apiParams.SecretKey,
                this.apiParams.CPCode,
                h => { }
            ));
        }

        [Fact]
        public void it_does_not_smoke_on_listing_directories()
        {
            var result = this.sut.DirAsync(new DirectoryRequest(this.apiParams.TestFolder)).Result;
            result.Should().NotBeNull();

            var dirResult = this.sut.ListAsync(new ListRequest(this.apiParams.TestFolder)).Result;
            result.Should().NotBeNull();
        }

        [Fact]
        public void it_should_upload_and_delete_file()
        {
            var content = Guid.NewGuid().ToByteArray();
            var fileName = Guid.NewGuid() + ".txt";

            var path = $"{this.apiParams.TestFolder}/{fileName}";

            this.sut.UploadAsync(new UploadFileRequest(path, new UploadFile(new MemoryStream(content)))).Wait();

            var testEntries = this.sut.DirAsync(new DirectoryRequest(this.apiParams.TestFolder)).Result;
            testEntries.Entries.Where(x => x.File?.Name == fileName).Should().NotBeEmpty();

            this.sut.DeleteAsync(new DeleteRequest(path)).Wait();

            var testEntriesAfterDelete = this.sut.DirAsync(new DirectoryRequest(this.apiParams.TestFolder)).Result;
            testEntriesAfterDelete.Entries.Where(x => x.File?.Name == fileName).Should().BeEmpty();
        }
    }
}