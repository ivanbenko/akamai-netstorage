namespace Krafteq.Akamai.NetStorage.Interop
{
    using System;
    using System.IO;

    class NetStorageRequest
    {
        public string Method { get; }

        public string Path { get; }

        public string Action { get; }

        public Stream UploadStream { get; }

        public NetStorageRequest(string method, string path, string action, Stream uploadStream)
        {
            this.Method = method ?? throw new ArgumentNullException(nameof(method));
            this.Path = path ?? throw new ArgumentNullException(nameof(path));
            this.Action = action ?? throw new ArgumentNullException(nameof(action));
            this.UploadStream = uploadStream;
        }
    }
}