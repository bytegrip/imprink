namespace Imprink.Domain.Models;

public class OrderFilterParameters
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? UserId { get; set; }

    public string? OrderNumber { get; set; }

    public int? OrderStatusId { get; set; }

    public int? ShippingStatusId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? MinTotalPrice { get; set; }

    public decimal? MaxTotalPrice { get; set; }

    public string SortBy { get; set; } = "OrderDate";

    public string SortDirection { get; set; } = "DESC";

    public bool IsValidDateRange()
    {
        if (StartDate.HasValue && EndDate.HasValue)
        {
            return StartDate.Value <= EndDate.Value;
        }
        return true;
    }

    public bool IsValidPriceRange()
    {
        if (MinTotalPrice.HasValue && MaxTotalPrice.HasValue)
        {
            return MinTotalPrice.Value <= MaxTotalPrice.Value;
        }
        return true;
    }
}