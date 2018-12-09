namespace Akamai.NetStorage.Tests
{
    using FluentAssertions;
    using Xunit;

    public class FullPathFormatterTests
    {
        readonly string host = "https://test.com";
        readonly string cdCode = "123456";

        [Fact]
        public void format_returns_absolute_path_as_is()
        {
            var sut = new FullPathFormatter(this.host, this.cdCode);
            var path = "https://test.com/123/abc.txt";

            sut.Format(path).Should().Be(path);
        }

        [Fact]
        public void format_should_make_absolute_path()
        {
            var sut = new FullPathFormatter(this.host, this.cdCode);

            sut.Format("123456/abc.txt").Should().Be($"{this.host}/123456/abc.txt");
            sut.Format("/123456/abc.txt").Should().Be($"{this.host}/123456/abc.txt");
        }

        [Fact]
        public void format_should_add_cp_code_to_absolute_path()
        {
            var sut = new FullPathFormatter(this.host, this.cdCode);

            sut.Format("abc.txt").Should().Be($"{this.host}/123456/abc.txt");
            sut.Format("/abc.txt").Should().Be($"{this.host}/123456/abc.txt");
        }
    }
}