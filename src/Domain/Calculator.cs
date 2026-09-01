using System.Collections.Generic;
using System.Linq;

namespace Domain;

public class Calculator
{
    public decimal GetTotalByProducts(IEnumerable<Item> items)
    {
        if (items == null || !items.Any())
            return 0m;

        return items.Sum(item =>
        {
            var itemTotal = item.Price * item.Quantity;
            return itemTotal - (itemTotal * (item.Discount / 100m));
        });
    }
}