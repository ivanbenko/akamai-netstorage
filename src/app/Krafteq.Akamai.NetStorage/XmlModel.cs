namespace Krafteq.Akamai.NetStorage
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    class DirResponseXml
    {
        public string Directory { get; set; }

        public List<FileEntryXml> Entries { get; set; }

        public static DirResponseXml Deserialize(XElement element) => new DirResponseXml
        {
            Directory = element.Attribute("directory")?.Value,
            Entries = element.Elements()
                .Select(FileEntryXml.Deserialize)
                .ToList()
        };
    }

    class ListResponseXml
    {
        public List<FileEntryXml> Entries { get; set; }

        public static ListResponseXml Deserialize(XElement element) => new ListResponseXml
        {
            Entries = element.Elements()
                .Select(FileEntryXml.Deserialize)
                .ToList()
        };
    }

    class FileEntryXml
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string Target { get; set; }

        public long? Size { get; set; }

        public string Md5Hash { get; set; }

        public long? Timestamp { get; set; }

        public bool? Implicit { get; set; }

        public long? TotalSize { get; set; }

        public int? FilesCount { get; set; }

        public static FileEntryXml Deserialize(XElement element) => new FileEntryXml
        {
            Type = element.Attribute("type")?.Value,
            Name = element.Attribute("name")?.Value,
            Timestamp = ParseLong(element.Attribute("mtime")?.Value),
            Md5Hash = element.Attribute("md5")?.Value,
            Implicit = ParseBool(element.Attribute("implicit")?.Value),
            Size = ParseLong(element.Attribute("size")?.Value),
            Target = element.Attribute("target")?.Value,
            TotalSize = ParseLong(element.Attribute("files")?.Value),
            FilesCount = ParseInt(element.Attribute("files")?.Value)
        };

        static int? ParseInt(string s) =>
            int.TryParse(s, out var res) ? (int?)res : null;

        static long? ParseLong(string s) =>
            long.TryParse(s, out var res) ? (long?)res : null;

        static bool? ParseBool(string s) =>
            bool.TryParse(s, out var res) ? (bool?)res : null;
    }
}