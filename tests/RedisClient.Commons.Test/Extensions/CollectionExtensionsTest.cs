using System.Collections;
using RedisClient.Commons.Extensions;

namespace RedisClient.Commons.Test.Extensions;

public class CollectionExtensionsTest
{
    [Theory]
    [InlineData(null)]
    [InlineData(new int[0])]
    public void IsEmpty_NullOrEmpty_ReturnTrue(IEnumerable? input)
    {
        var result = input.IsEmpty();
        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_Valid_ReturnFalse()
    {
        var input = new[] { 0 };
        var result = input.IsEmpty();
        Assert.False(result);
    }
}