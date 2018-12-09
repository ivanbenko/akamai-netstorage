namespace Akamai.NetStorage
{
    using System;

    class FullPathFormatter
    {
        readonly string host;
        readonly string cpCode;

        public FullPathFormatter(string host, string cpCode)
        {
            this.host = host;
            this.cpCode = cpCode;
        }

        public string Format(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
                return path;

            path = path.TrimStart('/');

            if (!path.StartsWith(this.cpCode))
            {
                path = $"{this.cpCode}/{path}";
            }

            return $"{this.host}/{path}";
        }
    }
}