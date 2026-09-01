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
        // Temporary stub without validation for RED phase verification
        return new Item(price, quantity, discount);
    }
}
