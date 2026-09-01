using System;

namespace Domain;

public class PromoCode
{
    public string Code { get; }
    public decimal DiscountPercent { get; }
    public bool IsActive { get; }
    public DateTime? ExpirationDate { get; }

    private PromoCode(string code, decimal discountPercent, DateTime? expirationDate)
    {
        Code = code;
        DiscountPercent = discountPercent;
        ExpirationDate = expirationDate;
        IsActive = true;
    }

    public static PromoCode Create(string code, decimal discountPercent, DateTime? expirationDate = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Promo code cannot be empty");

        if (discountPercent < 1)
            throw new ArgumentException("Promo discount must be at least 1%");

        if (discountPercent > 100)
            throw new ArgumentException("Promo discount cannot exceed 100%");

        return new PromoCode(code, discountPercent, expirationDate);
    }

    public bool IsValid(DateTime currentDate)
    {
        if (!IsActive)
            return false;

        if (ExpirationDate.HasValue && currentDate > ExpirationDate.Value)
            return false;

        return true;
    }
}