using System;
using Xunit;
using Domain;

namespace Domain.Tests;

public class PromoCodeTests
{
    [Theory]
    [InlineData("", 10, "Promo code cannot be empty")]
    [InlineData("   ", 10, "Promo code cannot be empty")]
    [InlineData("SAVE10", 0, "Promo discount must be at least 1%")]
    [InlineData("SAVE10", 101, "Promo discount cannot exceed 100%")]
    public void Create_InvalidParameters_ThrowsArgumentException(
        string code, decimal discountPercent, string expectedMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            PromoCode.Create(code, discountPercent));

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
}