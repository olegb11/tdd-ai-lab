using Xunit;
using Domain;

namespace Domain.Tests;

public class CalculatorTests
{
    private readonly Calculator _calculator = new();

    [Fact]
    public void GetTotal_EmptyCart_ReturnsZero()
    {
        var result = _calculator.GetTotalByProducts(Enumerable.Empty<Item>());
        Assert.Equal(0m, result);
    }

    [Theory]
    [InlineData(100.0, 1, 0, 100.0)]
    [InlineData(100.0, 1, 10, 90.0)]
    [InlineData(50.0, 2, 20, 80.0)]
    public void GetTotal_WithDiscount_CalculatesCorrectly(
        decimal price, int quantity, decimal discount, decimal expected)
    {
        var item = Item.Create(price, quantity, discount);
        var total = _calculator.GetTotalByProducts(new[] { item });

        Assert.Equal(expected, total);
    }

    [Fact]
    public void GetTotal_NullItems_ReturnsZero()
    {
        var result = _calculator.GetTotalByProducts(null!);
        Assert.Equal(0m, result);
    }

    [Fact]
    public void GetTotal_MultipleItems_SumsEachItemTotal()
    {
        var items = new[]
        {
            Item.Create(100.0m, 1, 0),  // 100.0
            Item.Create(50.0m, 2, 20)   // 80.0
        };

        var total = _calculator.GetTotalByProducts(items);

        Assert.Equal(180.0m, total);
    }
}
