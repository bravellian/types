using Bravillian;
using Xunit;

public class PercentageTests
{
    [Theory]
    [InlineData(0.123456, 0.1234)]
    [InlineData(0.99999, 0.9999)]
    [InlineData(-0.123456, -0.1234)]
    public void Value_TruncatesToFourDecimalPlaces(decimal input, decimal expected)
    {
        var pct = new Percentage(input);
        Assert.Equal(expected, pct.Value);
    }

    [Theory]
    [InlineData(0.123456, 12.34)]
    [InlineData(0.99999, 99.99)]
    [InlineData(-0.123456, -12.34)]
    public void ScaledValue_TruncatesAndScales(decimal input, decimal expected)
    {
        var pct = new Percentage(input);
        Assert.Equal(expected, pct.ScaledValue);
    }

    [Theory]
    [InlineData(0.1234, "12.34%")]
    [InlineData(0.5, "50.00%")]
    public void ToString_DefaultFormats(decimal input, string expected)
    {
        var pct = new Percentage(input);
        Assert.Equal(expected, pct.ToString());
    }

    [Theory]
    [InlineData(0.123456, "12.3456%")]
    [InlineData(1.0, "100%")]
    public void ToStringRaw_FormatsCorrectly(decimal input, string expected)
    {
        var pct = new Percentage(input);
        Assert.Equal(expected, pct.ToStringRaw());
    }

    [Fact]
    public void Operators_WorkAsExpected()
    {
        var a = new Percentage(0.5m);
        var b = new Percentage(0.25m);
        Assert.True(a > b);
        Assert.True(b < a);
        Assert.True(a >= b);
        Assert.True(b <= a);
    }

    [Fact]
    public void Parse_And_TryParse_Work()
    {
        var parsed = Percentage.Parse("0.25");
        Assert.Equal(new Percentage(0.25m), parsed);
        Assert.True(Percentage.TryParse("0.25", out var result));
        Assert.Equal(new Percentage(0.25m), result);
        Assert.Null(Percentage.TryParse("notanumber"));
    }
}
