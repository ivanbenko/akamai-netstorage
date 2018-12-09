namespace Akamai.NetStorage.Tests
{
    using System.Xml.Linq;
    using FluentAssertions;
    using Xunit;

    public class ListResponseXmlTests
    {
        [Fact]
        public void it_deserializes_xml()
        {
            var element = XElement.Parse(
                "<list>" +
                "<file type=\"file\" name=\"file1\"/>" +
                "<file type=\"dir\" name=\"file2\"/>" +
                "<file type=\"symlink\" name=\"file3\"/>" +
                "</list>");

            var sut = ListResponseXml.Deserialize(element);

            sut.Should().NotBeNull();
            sut.Entries.Should().NotBeEmpty();

            sut.Entries[0].Name.Should().Be("file1");
            sut.Entries[1].Name.Should().Be("file2");
            sut.Entries[2].Name.Should().Be("file3");
        }
    }
}