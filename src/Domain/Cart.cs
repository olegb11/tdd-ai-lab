using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain;

public class Cart
{
    private readonly List<Item> _items = new();
    private PromoCode? _appliedPromoCode;

    public IReadOnlyCollection<Item> Items => _items.AsReadOnly();
    public PromoCode? AppliedPromoCode => _appliedPromoCode;

    public void AddItem(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }

    public void ApplyPromoCode(PromoCode promoCode, DateTime currentDate)
    {
        ArgumentNullException.ThrowIfNull(promoCode);

        if (!promoCode.IsValid(currentDate))
        {
            throw new InvalidOperationException("Cannot apply invalid or expired promo code");
        }

        _appliedPromoCode = promoCode;
    }

    public decimal CalculateTotal()
    {
        // Calculate item total using Discount property
        var itemsTotal = _items.Sum(item => 
        {
            var discountFraction = item.Discount / 100m;
            return item.Price * item.Quantity * (1m - discountFraction);
        });

        if (_appliedPromoCode == null)
        {
            return decimal.Round(itemsTotal, 2, MidpointRounding.AwayFromZero);
        }

        var promoDiscountFraction = _appliedPromoCode.DiscountPercent / 100m;
        var finalTotal = itemsTotal * (1m - promoDiscountFraction);

        return decimal.Round(finalTotal, 2, MidpointRounding.AwayFromZero);
    }
}