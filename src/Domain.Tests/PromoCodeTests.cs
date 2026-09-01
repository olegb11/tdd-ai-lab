using System;
using Xunit;
using Domain;

namespace Domain.Tests;

public class PromoCodeTests
{
    [Theory]
    [InlineData("", 10, "Promo code cannot be empty")]
    [InlineData("   ", 10, "Promo code cannot be empty")]
    [InlineData("SAVE10", 0, "Discount percentage must be between 1 and 100")]
    [InlineData("SAVE10", -5, "Discount percentage must be between 1 and 100")]
    [InlineData("SAVE10", 101, "Discount percentage must be between 1 and 100")]
    public void Create_InvalidParameters_ThrowsArgumentException(
        string code, decimal discountPercent, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            PromoCode.Create(code, discountPercent, DateTime.UtcNow.AddDays(7)));

        Assert.Contains(expectedMessage, ex.Message);
    }

    [Fact]
    public void IsValid_ExpiredCode_ReturnsFalse()
    {
        var expiredCode = PromoCode.Create("SUMMER", 15, expirationDate: DateTime.UtcNow.AddDays(-1));

        Assert.False(expiredCode.IsValid(DateTime.UtcNow));
    }

    [Fact]
    public void IsValid_ActiveAndNotExpired_ReturnsTrue()
    {
        var validCode = PromoCode.Create("WINTER", 20, expirationDate: DateTime.UtcNow.AddDays(10));

        Assert.True(validCode.IsValid(DateTime.UtcNow));
    }

    [Theory]
    [InlineData("SHORT")]
    [InlineData("TOOLONG1")]
    [InlineData("")]
    public void Create_InvalidCodeLength_ThrowsArgumentException(string code)
    {
        // Arrange & Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            PromoCode.Create(code, 10, DateTime.UtcNow.AddDays(7)));

        if (string.IsNullOrWhiteSpace(code))
        {
            Assert.Equal("Promo code cannot be empty", ex.Message);
        }
        else
        {
            Assert.Equal("Promo code must be 6 characters", ex.Message);
        }
    }
}