namespace Krafteq.Akamai.NetStorage.Tests
{
    using System;
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
        public void it_does_not_smoke()
        {
            var result = this.sut.DirAsync(new DirectoryRequest(this.apiParams.TestFolder)).Result;
            result.Should().NotBeNull();

            var dirResult = this.sut.ListAsync(new ListRequest(this.apiParams.TestFolder)).Result;
            result.Should().NotBeNull();
        }
    }
}