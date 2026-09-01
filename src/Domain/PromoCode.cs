using System;

namespace Domain;

public class PromoCode
{
    public string Code { get; }
    public decimal DiscountPercent { get; }
    public DateTime ExpirationDate { get; }

    private PromoCode(string code, decimal discountPercent, DateTime expirationDate)
    {
        Code = code;
        DiscountPercent = discountPercent;
        ExpirationDate = expirationDate;
    }

    public static PromoCode Create(string code, decimal discountPercent, DateTime expirationDate)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Promo code cannot be empty");
        }

        if (code.Length != 6)
        {
            throw new ArgumentException("Promo code must be 6 characters");
        }

        if (discountPercent is < 1 or > 100)
        {
            throw new ArgumentException("Discount percentage must be between 1 and 100");
        }

        return new PromoCode(code, discountPercent, expirationDate);
    }

    public bool IsValid(DateTime currentDate)
    {
        return currentDate.Date <= ExpirationDate.Date;
    }
}