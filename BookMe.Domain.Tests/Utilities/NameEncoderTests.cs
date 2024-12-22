using Xunit;
using BookMe.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Domain.Utilities.Tests
{
    public class NameEncoderTests
    {
        [Theory]
        [InlineData("Example Name", "example-name")]
        [InlineData("John Doe", "john-doe")]
        [InlineData("  Leading and Trailing  ", "leading-and-trailing")]
        public void Encode_ShouldReturnEncodedName(string input, string expected)
        {
            // Act
            var result = NameEncoder.Encode(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Encode_ShouldHandleNullOrEmptyInput()
        {
            // Act
            var result = NameEncoder.Encode("");

            // Assert
            Assert.Equal("", result);
        }
    }
}