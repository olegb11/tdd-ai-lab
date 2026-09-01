using System;

namespace Domain;

public class Item
{
    public decimal Price { get; }
    public int Quantity { get; }
    public decimal Discount { get; }

    private Item(decimal price, int quantity, decimal discount)
    {
        Price = price;
        Quantity = quantity;
        Discount = discount;
    }

    public static Item Create(decimal price, int quantity, decimal discount)
    {
        if (price < 0)
            throw new ArgumentException("Price cannot be negative");

        if (quantity < 1)
            throw new ArgumentException("Quantity must be at least 1");

        if (discount < 0)
            throw new ArgumentException("Discount cannot be negative");

        if (discount > 100)
            throw new ArgumentException("Discount cannot exceed 100%");

        return new Item(price, quantity, discount);
    }
}