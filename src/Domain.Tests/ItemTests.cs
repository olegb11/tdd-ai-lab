using Xunit;
using Domain;

namespace Domain.Tests;

public class ItemTests
{
    [Theory]
    [InlineData(-10.0, 1, 0, "Price cannot be negative")]
    [InlineData(100.0, 0, 0, "Quantity must be at least 1")]
    [InlineData(100.0, 1, -5, "Discount cannot be negative")]
    [InlineData(100.0, 1, 105, "Discount cannot exceed 100%")]
    public void Create_InvalidParameters_ThrowsArgumentException(
        decimal price, int quantity, decimal discountPercent, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() => 
            Item.Create(price, quantity, discountPercent));

        Assert.Contains(expectedMessage, ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_InvalidQuantity_ThrowsArgumentException(int quantity)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Create(100.0m, quantity, 0));
    }

    [Fact]
    public void Create_ZeroPrice_IsAllowed()
    {
        var item = Item.Create(0.0m, 1, 0);

        Assert.Equal(0.0m, item.Price);
        Assert.Equal(1, item.Quantity);
    }

    [Fact]
    public void Create_MaxDiscount_IsAllowed()
    {
        var item = Item.Create(100.0m, 1, 100);

        Assert.Equal(100.0m, item.Price);
        Assert.Equal(100.0m, item.Discount);
    }
}
