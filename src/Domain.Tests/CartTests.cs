using System;
using Xunit;
using Domain;

namespace Domain.Tests;

public class CartTests
{
    [Fact]
    public void CalculateTotal_WithoutPromoCode_ReturnsStandardTotal()
    {
        // Arrange
        var cart = new Cart();
        cart.AddItem(Item.Create(100.0m, 2, 0));

        // Act
        var total = cart.CalculateTotal();

        // Assert
        Assert.Equal(200.0m, total);
    }

    [Fact]
    public void ApplyPromoCode_ValidCode_AppliesDiscountToTotal()
    {
        // Arrange
        var cart = new Cart();
        cart.AddItem(Item.Create(100.0m, 2, 0));
        var promo = PromoCode.Create("SAVE10", 10, DateTime.UtcNow.AddDays(1));

        // Act
        cart.ApplyPromoCode(promo, DateTime.UtcNow);

        // Assert
        Assert.Equal(180.0m, cart.CalculateTotal());
    }

    [Fact]
    public void ApplyPromoCode_ExpiredCode_ThrowsInvalidOperationException()
    {
        // Arrange
        var cart = new Cart();
        var expiredPromo = PromoCode.Create("OLDD10", 20, DateTime.UtcNow.AddDays(-1));

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() =>
            cart.ApplyPromoCode(expiredPromo, DateTime.UtcNow));

        Assert.Contains("Cannot apply invalid or expired promo code", ex.Message);
    }

    [Fact]
    public void CalculateTotal_MultipleItems_SumsEachItemTotal()
    {
        // Arrange
        var cart = new Cart();
        cart.AddItem(Item.Create(100.0m, 1, 0));
        cart.AddItem(Item.Create(50.0m, 2, 0));

        // Act
        var total = cart.CalculateTotal();

        // Assert
        Assert.Equal(200.0m, total);
    }

    [Fact]
    public void CalculateTotal_ItemWithDiscount_AppliesItemDiscount()
    {
        // Arrange
        var cart = new Cart();
        cart.AddItem(Item.Create(100.0m, 1, 10));

        // Act
        var total = cart.CalculateTotal();

        // Assert
        Assert.Equal(90.0m, total);
    }

    [Fact]
    public void CalculateTotal_MultipleDiscountedItems_SumsCorrectly()
    {
        // Arrange
        var cart = new Cart();
        cart.AddItem(Item.Create(100.0m, 1, 10));  // 90.0
        cart.AddItem(Item.Create(50.0m, 2, 20));   // 80.0

        // Act
        var total = cart.CalculateTotal();

        // Assert
        Assert.Equal(170.0m, total);
    }

    [Fact]
    public void AddItem_NullItem_ThrowsArgumentNullException()
    {
        // Arrange
        var cart = new Cart();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => cart.AddItem(null!));
    }

    [Fact]
    public void ApplyPromoCode_NullPromoCode_ThrowsArgumentNullException()
    {
        // Arrange
        var cart = new Cart();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            cart.ApplyPromoCode(null!, DateTime.UtcNow));
    }

    [Fact]
    public void CalculateTotal_DiscountedItemWithPromo_AppliesPromoOnDiscountedSubtotal()
    {
        // Arrange
        var cart = new Cart();
        cart.AddItem(Item.Create(100.0m, 1, 10));   // 90.0
        var promo = PromoCode.Create("SAVE10", 10, DateTime.UtcNow.AddDays(1));

        // Act
        cart.ApplyPromoCode(promo, DateTime.UtcNow);

        // Assert
        Assert.Equal(81.0m, cart.CalculateTotal());
    }
}