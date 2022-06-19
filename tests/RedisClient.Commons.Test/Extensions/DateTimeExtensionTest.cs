using RedisClient.Commons.Extensions;

namespace RedisClient.Commons.Test.Extensions;

public class DateTimeExtensionTest
{
    private static readonly DateTime DateTimeCase = DateTime.Parse("2022-06-19T21:37:38.123Z").ToUniversalTime();
    private const long Timestamp = 1655674658;
    private const long TimestampMillSeconds = 1655674658123;

    [Fact]
    public void UnixSecondsTimestamp_Valid_ReturnTrue()
    {
        var timestamp = DateTimeCase.UnixSecondsTimestamp();
        Assert.Equal(Timestamp, timestamp);
    }

    [Fact]
    public void UnixMillisecondsTimestamp_Valid_ReturnTrue()
    {
        var timestamp = DateTimeCase.UnixMillisecondsTimestamp();
        Assert.Equal(TimestampMillSeconds, timestamp);
    }
}