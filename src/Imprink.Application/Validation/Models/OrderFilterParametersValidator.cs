using FluentValidation;
using Imprink.Domain.Models;

namespace Imprink.Application.Validation.Models;

public class OrderFilterParametersValidator : AbstractValidator<OrderFilterParameters>
{
    public OrderFilterParametersValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("PageNumber must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

        RuleFor(x => x.UserId)
            .Length(1, 450).WithMessage("UserId length must be between 1 and 450 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.UserId));

        RuleFor(x => x.OrderNumber)
            .Length(1, 50).WithMessage("OrderNumber length must be between 1 and 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.OrderNumber));

        RuleFor(x => x.OrderStatusId)
            .GreaterThan(0).When(x => x.OrderStatusId.HasValue)
            .WithMessage("OrderStatusId must be greater than 0.");

        RuleFor(x => x.ShippingStatusId)
            .GreaterThan(0).When(x => x.ShippingStatusId.HasValue)
            .WithMessage("ShippingStatusId must be greater than 0.");

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1)).When(x => x.StartDate.HasValue)
            .WithMessage("StartDate cannot be in the future.");

        RuleFor(x => x.EndDate)
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1)).When(x => x.EndDate.HasValue)
            .WithMessage("EndDate cannot be in the future.");

        RuleFor(x => x)
            .Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue || x.StartDate <= x.EndDate)
            .WithMessage("StartDate cannot be greater than EndDate.");

        RuleFor(x => x.MinTotalPrice)
            .GreaterThanOrEqualTo(0).When(x => x.MinTotalPrice.HasValue)
            .WithMessage("MinTotalPrice cannot be negative.");

        RuleFor(x => x.MaxTotalPrice)
            .GreaterThanOrEqualTo(0).When(x => x.MaxTotalPrice.HasValue)
            .WithMessage("MaxTotalPrice cannot be negative.");

        RuleFor(x => x)
            .Must(x => !x.MinTotalPrice.HasValue || !x.MaxTotalPrice.HasValue || x.MinTotalPrice <= x.MaxTotalPrice)
            .WithMessage("MinTotalPrice cannot be greater than MaxTotalPrice.");

        RuleFor(x => x.SortBy)
            .NotEmpty().WithMessage("SortBy is required.")
            .Must(value => AllowedSortColumns.Contains(value, StringComparer.OrdinalIgnoreCase))
            .WithMessage("SortBy must be one of: OrderDate, TotalPrice, OrderNumber.");

        RuleFor(x => x.SortDirection)
            .NotEmpty().WithMessage("SortDirection is required.")
            .Must(value => value.Equals("ASC", StringComparison.OrdinalIgnoreCase) 
                           || value.Equals("DESC", StringComparison.OrdinalIgnoreCase))
            .WithMessage("SortDirection must be 'ASC' or 'DESC'.");
    }

    private static readonly string[] AllowedSortColumns = ["OrderDate", "TotalPrice", "OrderNumber"];
}