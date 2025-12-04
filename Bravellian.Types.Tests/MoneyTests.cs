// Copyright (c) Samuel McAravey
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Bravellian;
using Xunit;

public class MoneyTests
{
    [Theory]
    [InlineData(123.4567, "123.45")]
    [InlineData(0.999, "0.99")]
    [InlineData(-1.234, "-1.23")]
    public void ToDecimal_TruncatesToTwoDecimalPlaces(decimal input, string expected)
    {
        var money = new Money(input);
        Assert.Equal(decimal.Parse(expected), money.ToDecimal());
    }

    [Theory]
    [InlineData(123.4567, "$123.45")]
    [InlineData(-123.4567, "($123.45)")]
    public void ToAccounting_FormatsCorrectly(decimal input, string expected)
    {
        var money = new Money(input);
        Assert.Equal(expected, money.ToAccounting());
    }

    [Theory]
    [InlineData(1234.567, "$1,234.56")]
    [InlineData(-1234.567, "-$1,234.56")]
    public void ToCurrency_FormatsCorrectly(decimal input, string expected)
    {
        var money = new Money(input);
        Assert.Equal(expected, money.ToCurrency());
    }

    [Fact]
    public void Operators_WorkAsExpected()
    {
        var a = new Money(10.555m);
        var b = new Money(2.333m);
        Assert.Equal(new Money(12.88m), a + b);
        Assert.Equal(new Money(8.22m), a - b);
        Assert.True(a > b);
        Assert.True(b < a);
        Assert.True(a >= b);
        Assert.True(b <= a);
    }

    [Fact]
    public void UnaryOperators_WorkAsExpected()
    {
        var money = new Money(123.45m);
        Assert.Equal(new Money(-123.45m), -money);
        Assert.Equal(new Money(123.45m), +money);
    }

    [Fact]
    public void Multiplication_WithPercentage_Works()
    {
        var money = new Money(200m);
        var percent = new Percentage(0.25m);
        Assert.Equal(new Money(50m), money * percent);
        Assert.Equal(new Money(50m), percent * money);
    }

    [Fact]
    public void Multiplication_WithInteger_Works()
    {
        var money = new Money(200m);
        Assert.Equal(new Money(400m), money * 2);
        Assert.Equal(new Money(400m), 2 * money);
    }

    [Fact]
    public void Parse_And_TryParse_Work()
    {
        var parsed = Money.Parse("123.45");
        Assert.Equal(new Money(123.45m), parsed);
        Assert.True(Money.TryParse("123.45", out var result));
        Assert.Equal(new Money(123.45m), result);
        Assert.Null(Money.TryParse("notanumber"));
    }

    [Fact]
    public void ToWords_ProducesExpectedOutput()
    {
        var money = new Money(123.45m);
        var words = money.ToWords();
        Assert.Equal("one hundred and twenty-three dollars and forty-five cents", words);
    }

    [Fact]
    public void Unary_Operators_Work()
    {
        var money = new Money(123.45m);
        Assert.Equal(new Money(-123.45m), -money);
        Assert.Equal(new Money(123.45m), +money);
    }
}
