namespace Krafteq.Akamai.NetStorage.Tests
{
    using System.Xml.Linq;
    using FluentAssertions;
    using Xunit;

    public class DirResponseXmlTests
    {
        [Fact]
        public void it_deserializes_xml()
        {
            var element = XElement.Parse(
                "<stat directory=\"dir1\">" +
                "<file type=\"file\" name=\"file1\"/>" +
                "<file type=\"dir\" name=\"file2\"/>" +
                "<file type=\"symlink\" name=\"file3\"/>" +
                "</stat>");

            var sut = DirResponseXml.Deserialize(element);

            sut.Should().NotBeNull();
            sut.Entries.Should().NotBeEmpty();

            sut.Directory.Should().Be("dir1");
        }
    }
}