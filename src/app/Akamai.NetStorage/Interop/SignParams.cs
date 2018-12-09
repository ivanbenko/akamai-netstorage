namespace Akamai.NetStorage.Interop
{
    using System;

    class SignParams
    {
        public string UserName { get; }

        public string SecretKey { get; }

        public Signature Signature { get; }

        public SignParams(string userName, string secretKey, Signature signature)
        {
            this.UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            this.SecretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
            this.Signature = signature;
        }
    }
}