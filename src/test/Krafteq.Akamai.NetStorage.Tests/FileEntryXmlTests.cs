namespace Krafteq.Akamai.NetStorage.Tests
{
    using System.Xml.Linq;
    using FluentAssertions;
    using Xunit;

    public class FileEntryXmlTests
    {
        [Fact]
        public void it_deserializes_xml()
        {
            var element = XElement.Parse(
                "<file " +
                    "type=\"file\" " +
                    "name=\"777/dir/File9.ext\" " +
                    "size=\"3\" " +
                    "md5=\"md5hash\" " +
                    "mtime=\"1524068475\" " +
                    "implicit=\"true\"" +
                "/>");

            var sut = FileEntryXml.Deserialize(element);

            sut.Type.Should().Be("file");
            sut.Name.Should().Be("777/dir/File9.ext");
            sut.Size.Should().Be(3);
            sut.Md5Hash.Should().Be("md5hash");
            sut.Timestamp.Should().Be(1524068475);
            sut.Implicit.Should().BeTrue();
        }
    }
}