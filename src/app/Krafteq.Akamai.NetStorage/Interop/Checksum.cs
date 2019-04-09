namespace Krafteq.Akamai.NetStorage.Interop
{
    class Checksum
    {
        public static readonly Checksum MD5 = new Checksum("MD5");
        public static readonly Checksum SHA1 = new Checksum("SHA1");
        public static readonly Checksum SHA256 = new Checksum("SHA256");

        readonly string algorithm;

        Checksum(string algorithm)
        {
            this.algorithm = algorithm;
        }
    }
}