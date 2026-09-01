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

    [Fact]
    public void Create_InvalidQuantity_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => Item.Create(100.0m, -1, 0));
    }
}
