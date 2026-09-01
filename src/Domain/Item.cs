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
        // TODO: Enforce domain invariants (Price >= 0, Quantity >= 1, Discount 0..100)
        return new Item(price, quantity, discount);
    }
}
