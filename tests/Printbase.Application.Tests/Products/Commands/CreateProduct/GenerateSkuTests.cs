using Printbase.Application.Products.Commands.CreateProduct;

namespace Printbase.Application.Tests.Products.Commands.CreateProduct;

using Xunit;

public class GenerateSkuTests
{
    [Fact]
    public void GenerateSku_WithValidInputs_ReturnsCorrectFormat()
    {
        // Arrange
        const string productName = "Shirt";
        const string color = "Blue";
        const string size = "Medium";

        // Act
        var sku = CreateProductCommandHandler.GenerateSku(productName, color, size);
        var parts = sku.Split('-');

        // Assert
        Assert.Equal(4, parts.Length);
        Assert.Equal("SHI", parts[0]); 
        Assert.Equal("BLU", parts[1]); 
        Assert.Equal("MEDIUM", parts[2]); 
        Assert.Matches(@"^\d{3}$", parts[3]); 
    }

    [Fact]
    public void GenerateSku_WithShortProductName_UsesEntireProductName()
    {
        // Arrange
        const string productName = "CU";
        const string color = "Black";
        const string size = "Large";

        // Act
        var sku = CreateProductCommandHandler.GenerateSku(productName, color, size);
        var parts = sku.Split('-');

        // Assert
        Assert.Equal("CU", parts[0]); 
        Assert.Equal("BLA", parts[1]);
        Assert.Equal("LARGE", parts[2]);
    }

    [Fact]
    public void GenerateSku_WithShortColor_UsesEntireColor()
    {
        // Arrange
        const string productName = "Mug";
        const string color = "Red";
        const string size = "Small";

        // Act
        var sku = CreateProductCommandHandler.GenerateSku(productName, color, size);
        var parts = sku.Split('-');

        // Assert
        Assert.Equal("MUG", parts[0]);
        Assert.Equal("RED", parts[1]); 
        Assert.Equal("SMALL", parts[2]);
    }

    [Fact]
    public void GenerateSku_WithNullColor_UsesXXX()
    {
        // Arrange
        const string productName = "Case";
        string? color = null;
        const string size = "Standard";

        // Act
        var sku = CreateProductCommandHandler.GenerateSku(productName, color, size);
        var parts = sku.Split('-');

        // Assert
        Assert.Equal("CAS", parts[0]);
        Assert.Equal("XXX", parts[1]); 
        Assert.Equal("STANDARD", parts[2]);
    }

    [Fact]
    public void GenerateSku_WithEmptyColor_UsesXXX()
    {
        // Arrange
        const string productName = "Notebook";
        const string color = "";
        const string size = "Standard";

        // Act
        var sku = CreateProductCommandHandler.GenerateSku(productName, color, size);
        var parts = sku.Split('-');

        // Assert
        Assert.Equal("NOT", parts[0]);
        Assert.Equal("XXX", parts[1]); 
        Assert.Equal("STANDARD", parts[2]);
    }

    [Fact]
    public void GenerateSku_WithNullSize_UsesOS()
    {
        // Arrange
        const string productName = "Watch";
        const string color = "Silver";
        string? size = null;

        // Act
        var sku = CreateProductCommandHandler.GenerateSku(productName, color, size);
        var parts = sku.Split('-');

        // Assert
        Assert.Equal("WAT", parts[0]);
        Assert.Equal("SIL", parts[1]);
        Assert.Equal("OS", parts[2]); 
    }

    [Fact]
    public void GenerateSku_WithEmptySize_UsesOS()
    {
        // Arrange
        const string productName = "Shirt";
        const string color = "Black";
        const string size = "";

        // Act
        var sku = CreateProductCommandHandler.GenerateSku(productName, color, size);
        var parts = sku.Split('-');

        // Assert
        Assert.Equal("SHI", parts[0]);
        Assert.Equal("BLA", parts[1]);
        Assert.Equal("OS", parts[2]);
    }

    [Fact]
    public void GenerateSku_CaseInsensitivity_OutputsUppercase()
    {
        // Arrange
        const string productName = "hat";
        const string color = "red";
        const string size = "small";

        // Act
        var sku = CreateProductCommandHandler.GenerateSku(productName, color, size);
        var parts = sku.Split('-');

        // Assert
        Assert.Equal("HAT", parts[0]);
        Assert.Equal("RED", parts[1]); 
        Assert.Equal("SMALL", parts[2]); 
    }

    [Fact]
    public void GenerateSku_RandomPartIsInRange()
    {
        // Arrange
        const string productName = "Book";
        const string color = "Green";
        const string size = "Medium";

        // Act
        var sku = CreateProductCommandHandler.GenerateSku(productName, color, size);
        var parts = sku.Split('-');
        var randomPart = int.Parse(parts[3]);

        // Assert
        Assert.InRange(randomPart, 100, 999);
    }

    [Fact]
    public void GenerateSku_GeneratesUniqueSKUs()
    {
        // Arrange
        const string productName = "Mug";
        const string color = "White";
        const string size = "Regular";

        // Act
        var sku1 = CreateProductCommandHandler.GenerateSku(productName, color, size);
        var sku2 = CreateProductCommandHandler.GenerateSku(productName, color, size);

        // Assert
        Assert.NotEqual(sku1, sku2);
    }
}