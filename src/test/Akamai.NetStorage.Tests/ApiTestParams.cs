namespace Akamai.NetStorage.Tests
{
    using System;

    class ApiTestParams
    {
        public string Host { get; }
        public string User { get; }
        public string SecretKey { get; }
        public string TestFolder { get; }
        public string CPCode { get; }

        public ApiTestParams(string host, string user, string secretKey, string testFolder, string cpCode)
        {
            this.Host = host;
            this.User = user;
            this.SecretKey = secretKey;
            this.TestFolder = testFolder;
            this.CPCode = cpCode;
        }

        public static ApiTestParams GetFromEnvironment() => 
            new ApiTestParams(
                Environment.GetEnvironmentVariable("NetStorageHost"),
                Environment.GetEnvironmentVariable("NetStorageUser"),
                Environment.GetEnvironmentVariable("NetStorageSecretKey"),
                Environment.GetEnvironmentVariable("NetStorageTestFolder"),
                Environment.GetEnvironmentVariable("NetStorageCPCode")
                );
    }
}