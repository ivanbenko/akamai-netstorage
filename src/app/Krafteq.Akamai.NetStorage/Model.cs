namespace Krafteq.Akamai.NetStorage
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ListResponse
    {
        public IReadOnlyCollection<NetStorageEntry> Entries { get; }

        public ListResponse(IReadOnlyCollection<NetStorageEntry> entries)
        {
            this.Entries = entries ?? throw new ArgumentNullException(nameof(entries));
        }
    }
    
    public class ListRequest
    {
        public string Path { get; }

        public int? MaxEntries { get; set; }

        public string End { get; set; }

        public ListRequest(string path)
        {
            this.Path = path;
        }

        internal string BuildAction()
        {
            return ActionBuilder.Build("list", b =>
            {
                b.Add("format", "xml");
                if (this.MaxEntries != null)
                    b.Add("max_entries", b.FormatValue(this.MaxEntries));

                if (this.End != null)
                    b.Add("end", this.End);
            });
        }
    }

    public class DirectoryRequest
    {
        public string Path { get; }

        public string Prefix { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public int? MaxEntries { get; set; }

        public bool Slash { get; set; }

        public DirectoryRequest(string path)
        {
            this.Path = path;
        }

        internal string BuildAction()
        {
            return ActionBuilder.Build("dir", b =>
            {
                b.Add("format", "xml");
                if (this.Prefix != null)
                    b.Add("prefix", this.Prefix);

                if (this.Start != null)
                    b.Add("start", this.Start);

                if (this.End != null)
                    b.Add("end", this.End);
                
                if (this.MaxEntries != null)
                    b.Add("max_entries", b.FormatValue(this.MaxEntries));

                if (this.Slash) 
                    b.Add("slash", "both");
            });
        }
    }

    public class StatRequest
    {
        public string Path { get; }

        public StatRequest(string path)
        {
            this.Path = path;
        }

        internal string BuildAction() => ActionBuilder.Build("stat", b => { });
    }

    public class DirResponse
    {
        public string Name { get; }
        public IReadOnlyCollection<NetStorageEntry> Entries { get; }

        public DirResponse(string name, IReadOnlyCollection<NetStorageEntry> entries)
        {
            this.Entries = entries ?? throw new ArgumentNullException(nameof(entries));
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }

    public class NetStorageEntry
    {
        public FileEntry File { get; }

        public DirectoryEntry Directory { get; }

        public SymlinkEntry Symlink { get; }

        NetStorageEntry(FileEntry file, DirectoryEntry directory, SymlinkEntry symlink)
        {
            this.File = file;
            this.Directory = directory;
            this.Symlink = symlink;
        }

        public static NetStorageEntry FileEntry(FileEntry file) =>
            new NetStorageEntry(file, null, null);

        public static NetStorageEntry DirectoryEntry(DirectoryEntry directory) =>
            new NetStorageEntry(null, directory, null);

        public static NetStorageEntry SymlinkEntry(SymlinkEntry symlink) =>
            new NetStorageEntry(null, null, symlink);

        internal static NetStorageEntry FromXml(FileEntryXml xml)
        {
            switch (xml.Type)
            {
                case "file":
                    return FileEntry(NetStorage.FileEntry.FromXml(xml));
                case "dir":
                    return DirectoryEntry(NetStorage.DirectoryEntry.FromXml(xml));
                case "symlink":
                    return SymlinkEntry(NetStorage.SymlinkEntry.FromXml(xml));
                default:
                    throw new InvalidOperationException($"Invalid type {xml.Type}");
            }
        }
    }


    public class FileEntry
    {
        public string Name { get; }

        public long Size { get; }

        public string Md5 { get; }

        public DateTime ModifiedAt { get; }

        public FileEntry(string name, long size, string md5, DateTime modifiedAt)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Size = size;
            this.Md5 = md5 ?? throw new ArgumentNullException(nameof(md5));
            this.ModifiedAt = modifiedAt;
        }

        internal static FileEntry FromXml(FileEntryXml xml) =>
            new FileEntry(
                xml.Name, 
                xml.Size.EnsureNotNull(), 
                xml.Md5Hash, 
                xml.Timestamp.EnsureNotNull().FromEpochSeconds());
    }

    public class DirectoryEntry
    {
        public string Name { get; }

        public long? TotalSize { get; }

        public int? FilesCount { get; }

        public DateTime? ModifiedAt { get; }

        public bool Implicit { get; }

        public DirectoryEntry(string name, long? totalSize, int? filesCount, DateTime? modifiedAt, bool @implicit)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.TotalSize = totalSize;
            this.FilesCount = filesCount;
            this.ModifiedAt = modifiedAt;
            this.Implicit = @implicit;
        }

        internal static DirectoryEntry FromXml(FileEntryXml xml) =>
            new DirectoryEntry(
                xml.Name,
                xml.TotalSize,
                xml.FilesCount,
                xml.Timestamp.Map(y => y.FromEpochSeconds()),
                xml.Implicit ?? false
            );
    }

    public class SymlinkEntry
    {
        public string Name { get; }

        public string Target { get; }

        public DateTime? ModifiedAt { get; }

        public SymlinkEntry(string name, string target, DateTime? modifiedAt)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Target = target;
            this.ModifiedAt = modifiedAt;
        }

        internal static SymlinkEntry FromXml(FileEntryXml xml) =>
            new SymlinkEntry(xml.Name, xml.Target, xml.Timestamp.Map(y => y.FromEpochSeconds()));
    }

    public class UploadFileRequest
    {
        public string Path { get; }

        public UploadFile File { get; }

        public bool IndexZip { get; set; }

        public UploadFileRequest(string path, UploadFile file)
        {
            this.Path = path ?? throw new ArgumentNullException(nameof(path));
            this.File = file ?? throw new ArgumentNullException(nameof(file));
        }

        internal string BuildAction() => ActionBuilder.Build("upload", b =>
        {
            if (this.IndexZip && this.Path.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase))
                b.Add("index-zip", "1");

            if (this.File.ModifiedAt != null)
                b.Add("mtime", b.FormatValue(this.File.ModifiedAt));

            if (this.File.Size != null)
                b.Add("size", b.FormatValue(this.File.Size.Value));

            if (this.File.Md5Checksum != null)
                b.Add("md5", b.FormatValue(this.File.Md5Checksum));

            if (this.File.Sha1Checksum != null)
                b.Add("sha1", b.FormatValue(this.File.Sha1Checksum));
                
            if (this.File.Sha256Checksum != null)
                b.Add("sha256", b.FormatValue(this.File.Sha256Checksum));
        });
    }

    public class DeleteRequest
    {
        public string Path { get; }

        public DeleteRequest(string path)
        {
            this.Path = path;
        }

        internal string BuildAction() => ActionBuilder.Build("delete", b =>
        {
        });
    }

    public class UploadFile
    {
        public Stream Stream { get; }

        public long? Size { get; set; }

        public byte[] Md5Checksum { get; set; }

        public byte[] Sha1Checksum { get; set; }

        public byte[] Sha256Checksum { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public UploadFile(Stream stream)
        {
            this.Stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }
    }
}