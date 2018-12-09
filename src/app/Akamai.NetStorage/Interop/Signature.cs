namespace Akamai.NetStorage.Interop
{
    using System.Security.Cryptography;

    class Signature
    {
        public static readonly Signature HMACMD5 = new Signature(3, "HMACMD5");
        public static readonly Signature HMACSHA1 = new Signature(4, "HMACSHA1");
        public static readonly Signature HMACSHA256 = new Signature(5, "HMACSHA256");

        readonly string algorithm;

        public int VersionId { get; }

        Signature(int versionId, string algorithm)
        {
            this.VersionId = versionId;
            this.algorithm = algorithm;
        }

        public byte[] Sign(byte[] data, byte[] secretKey)
        {
            using (var hmac = HMAC.Create(this.algorithm))
            {
                hmac.Key = secretKey;
                return hmac.ComputeHash(data);
            }
        }
    }
}