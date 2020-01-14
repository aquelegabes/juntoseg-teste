using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static JuntoSeg.Common.HashString;
namespace JuntoSeg.Tests.Common
{
    public class HashStringTest : MoqConfig
    {
        [Theory]
        [InlineData("testes1")]
        [InlineData("test2")]
        public void HashString_CreateHashString_Ok(string value)
        {
            var generatedHash = CreateHashString(value);

            Assert.NotNull(generatedHash);
            Assert.NotEmpty(generatedHash);
        }

        [Theory]
        [InlineData("testes1")]
        [InlineData("test2")]
        public void HashString_VerifyHashedString_Ok(string value)
        {
            var generatedHash = CreateHashString(value);
            bool hashMatches = VerifyHashString(generatedHash, value);

            Assert.True(hashMatches);
        }
    }
}
