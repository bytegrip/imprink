using FluentValidation;
using Imprink.Application.Commands.Addresses;

namespace Imprink.Application.Validation.Addresses;

public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressCommandValidator()
    {
        RuleFor(x => x.AddressType)
            .NotEmpty()
            .WithMessage("Address type is required.")
            .MaximumLength(50)
            .WithMessage("Address type must not exceed 50 characters.")
            .Must(BeValidAddressType)
            .WithMessage("Address type must be one of: Home, Work, Billing, Shipping, Other.");

        RuleFor(x => x.FirstName)
            .MaximumLength(100)
            .WithMessage("First name must not exceed 100 characters.")
            .Matches(@"^[a-zA-Z\s\-'\.]*$")
            .When(x => !string.IsNullOrEmpty(x.FirstName))
            .WithMessage("First name can only contain letters, spaces, hyphens, apostrophes, and periods.");

        RuleFor(x => x.LastName)
            .MaximumLength(100)
            .WithMessage("Last name must not exceed 100 characters.")
            .Matches(@"^[a-zA-Z\s\-'\.]*$")
            .When(x => !string.IsNullOrEmpty(x.LastName))
            .WithMessage("Last name can only contain letters, spaces, hyphens, apostrophes, and periods.");

        RuleFor(x => x.Company)
            .MaximumLength(200)
            .WithMessage("Company name must not exceed 200 characters.");

        RuleFor(x => x.AddressLine1)
            .NotEmpty()
            .WithMessage("Address line 1 is required.")
            .MaximumLength(255)
            .WithMessage("Address line 1 must not exceed 255 characters.");

        RuleFor(x => x.AddressLine2)
            .MaximumLength(255)
            .WithMessage("Address line 2 must not exceed 255 characters.");

        RuleFor(x => x.ApartmentNumber)
            .MaximumLength(20)
            .WithMessage("Apartment number must not exceed 20 characters.");

        RuleFor(x => x.BuildingNumber)
            .MaximumLength(20)
            .WithMessage("Building number must not exceed 20 characters.");

        RuleFor(x => x.Floor)
            .MaximumLength(20)
            .WithMessage("Floor must not exceed 20 characters.");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required.")
            .MaximumLength(100)
            .WithMessage("City must not exceed 100 characters.")
            .Matches(@"^[a-zA-Z\s\-'\.]*$")
            .WithMessage("City can only contain letters, spaces, hyphens, apostrophes, and periods.");

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage("State is required.")
            .MaximumLength(100)
            .WithMessage("State must not exceed 100 characters.");

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage("Postal code is required.")
            .MaximumLength(20)
            .WithMessage("Postal code must not exceed 20 characters.")
            .Matches(@"^[a-zA-Z0-9\s\-]*$")
            .WithMessage("Postal code can only contain letters, numbers, spaces, and hyphens.");

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required.")
            .MaximumLength(100)
            .WithMessage("Country must not exceed 100 characters.")
            .Matches(@"^[a-zA-Z\s\-'\.]*$")
            .WithMessage("Country can only contain letters, spaces, hyphens, apostrophes, and periods.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .WithMessage("Phone number must not exceed 20 characters.")
            .Matches(@"^[\+]?[0-9\s\-\(\)\.]*$")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Phone number format is invalid. Use numbers, spaces, hyphens, parentheses, periods, and optional + prefix.");

        RuleFor(x => x.Instructions)
            .MaximumLength(500)
            .WithMessage("Instructions must not exceed 500 characters.");

        RuleFor(x => x)
            .Must(HaveNameOrCompany)
            .WithMessage("Either first name and last name, or company name must be provided.")
            .WithName("Address");
    }

    private static bool BeValidAddressType(string addressType)
    {
        var validTypes = new[] { "Home", "Work", "Billing", "Shipping", "Other" };
        return validTypes.Contains(addressType, StringComparer.OrdinalIgnoreCase);
    }

    private static bool HaveNameOrCompany(CreateAddressCommand command)
    {
        var hasName = !string.IsNullOrWhiteSpace(command.FirstName) && !string.IsNullOrWhiteSpace(command.LastName);
        var hasCompany = !string.IsNullOrWhiteSpace(command.Company);
        return hasName || hasCompany;
    }
}