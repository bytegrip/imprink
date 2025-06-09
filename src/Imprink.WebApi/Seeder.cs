using Imprink.Domain.Entities.Product;
using Imprink.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Imprink.WebApi;

public class Seeder(ApplicationDbContext context)
{
    private readonly Random _random = new();
    
    private readonly string[] _categoryImages =
    [
        "https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=500",
        "https://images.unsplash.com/photo-1560472354-b33ff0c44a43?w=500",
        "https://images.unsplash.com/photo-1586953208448-b95a79798f07?w=500",
        "https://images.unsplash.com/photo-1503602642458-232111445657?w=500"
    ];
    
    private readonly string[] _textileImages =
    [
        "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=500",
        "https://images.unsplash.com/photo-1583743814966-8936f37f4ad2?w=500",
        "https://images.unsplash.com/photo-1571945153237-4929e783af4a?w=500",
        "https://images.unsplash.com/photo-1618354691373-d851c5c3a990?w=500",
        "https://images.unsplash.com/photo-1576566588028-4147f3842f27?w=500"
    ];
    
    private readonly string[] _hardSurfaceImages =
    [
        "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=500",
        "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=500",
        "https://images.unsplash.com/photo-1544966503-7cc5ac882d2e?w=500",
        "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500"
    ];
    
    private readonly string[] _paperImages =
    [
        "https://images.unsplash.com/photo-1586281010691-79ab3d0f2102?w=500",
        "https://images.unsplash.com/photo-1594736797933-d0401ba2fe65?w=500",
        "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=500",
        "https://images.unsplash.com/photo-1584464491033-06628f3a6b7b?w=500"
    ];

    public async Task SeedAsync()
    {
        try
        {
            Console.WriteLine("Starting database seeding...");
            
            var categories = await SeedCategories();
            Console.WriteLine($"Created {categories.Count} categories");
            
            var products = await SeedProducts(categories);
            Console.WriteLine($"Created {products.Count} products");
            
            await SeedProductVariants(products);
            Console.WriteLine("Created product variants");
            
            Console.WriteLine("Database seeding completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during seeding: {ex.Message}");
            throw;
        }
    }

    private async Task<List<Category>> SeedCategories()
    {
        var categories = new List<Category>();
        var now = DateTime.UtcNow;
        
        var categoryData = new Dictionary<string, List<string>>
        {
            ["Textile"] =
            [
                "T-Shirts", "Hoodies", "Tank Tops", "Long Sleeves", "Polo Shirts",
                "Sweatshirts", "Jackets", "Caps & Hats", "Bags", "Towels",
                "Aprons", "Baby Clothing", "Youth Clothing", "Women's Apparel", "Men's Apparel"
            ],
            ["Hard Surfaces"] =
            [
                "Mugs", "Water Bottles", "Phone Cases", "Laptop Cases", "Keychains",
                "Mouse Pads", "Coasters", "Picture Frames", "Awards & Trophies", "Signs",
                "Magnets", "Buttons & Pins", "Clocks", "Tiles", "Metal Prints"
            ],
            ["Paper"] =
            [
                "Business Cards", "Flyers", "Brochures", "Posters", "Banners",
                "Stickers", "Labels", "Notebooks", "Calendars", "Greeting Cards",
                "Postcards", "Bookmarks", "Menu Cards", "Invitations", "Certificates"
            ]
        };

        var existingMainCategories = await context.Set<Category>()
            .Where(c => c.ParentCategoryId == null)
            .ToListAsync();

        var mainCategoryMap = existingMainCategories.ToDictionary(c => c.Name, c => c);

        foreach (var mainCategoryName in categoryData.Keys)
        {
            Category mainCategory;
            if (mainCategoryMap.TryGetValue(mainCategoryName, out var value))
            {
                mainCategory = value;
            }
            else
            {
                mainCategory = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = mainCategoryName,
                    Description = $"{mainCategoryName} products and materials",
                    ImageUrl = _categoryImages[_random.Next(_categoryImages.Length)],
                    SortOrder = categories.Count + 1,
                    IsActive = true,
                    CreatedAt = now,
                    ModifiedAt = now,
                    CreatedBy = "seeder@system.com",
                    ModifiedBy = "seeder@system.com"
                };
                context.Set<Category>().Add(mainCategory);
                await context.SaveChangesAsync();
            }
            
            categories.Add(mainCategory);

            var subcategoryNames = categoryData[mainCategoryName];
            for (var i = 0; i < subcategoryNames.Count; i++)
            {
                var subcategory = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = subcategoryNames[i],
                    Description = $"High-quality {subcategoryNames[i].ToLower()} for custom printing",
                    ImageUrl = GetImageForCategory(mainCategoryName),
                    SortOrder = i + 1,
                    IsActive = true,
                    ParentCategoryId = mainCategory.Id,
                    CreatedAt = now,
                    ModifiedAt = now,
                    CreatedBy = "seeder@system.com",
                    ModifiedBy = "seeder@system.com"
                };
                
                categories.Add(subcategory);
                context.Set<Category>().Add(subcategory);
            }
        }

        await context.SaveChangesAsync();
        return categories;
    }

    private async Task<List<Product>> SeedProducts(List<Category> categories)
    {
        var products = new List<Product>();
        var now = DateTime.UtcNow;
        
        var subcategories = categories.Where(c => c.ParentCategoryId.HasValue).ToList();

        foreach (var category in subcategories)
        {
            var productCount = _random.Next(15, 35); 
            
            for (var i = 0; i < productCount; i++)
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = GenerateProductName(category.Name, i + 1),
                    Description = GenerateProductDescription(category.Name),
                    BasePrice = GenerateBasePrice(category.Name),
                    IsCustomizable = _random.NextDouble() > 0.2, 
                    IsActive = _random.NextDouble() > 0.05, 
                    ImageUrl = GetImageForCategory(GetMainCategoryName(category)),
                    CategoryId = category.Id,
                    Category = category,
                    CreatedAt = now.AddDays(-_random.Next(0, 365)),
                    ModifiedAt = now.AddDays(-_random.Next(0, 30)),
                    CreatedBy = "seeder@system.com",
                    ModifiedBy = "seeder@system.com"
                };
                
                products.Add(product);
                context.Set<Product>().Add(product);
            }
        }

        await context.SaveChangesAsync();
        return products;
    }

    private async Task SeedProductVariants(List<Product> products)
    {
        var now = DateTime.UtcNow;
        var skuCounter = 1;

        foreach (var variant in from product in products let mainCategory = GetMainCategoryName(product.Category) select GenerateVariantsForProduct(product, mainCategory, ref skuCounter, now) into variants from variant in variants select variant)
        {
            context.Set<ProductVariant>().Add(variant);
        }

        await context.SaveChangesAsync();
    }

    private List<ProductVariant> GenerateVariantsForProduct(Product product, string mainCategory, ref int skuCounter, DateTime now)
    {
        var variants = new List<ProductVariant>();
        
        var sizes = GetSizesForCategory(mainCategory);
        var colors = GetColorsForCategory();
        
        var variantCount = Math.Min(_random.Next(3, 9), sizes.Count * colors.Count);
        var usedCombinations = new HashSet<string>();
        
        for (var i = 0; i < variantCount; i++)
        {
            string size, color;
            string combination;
            
            do
            {
                size = sizes[_random.Next(sizes.Count)];
                color = colors[_random.Next(colors.Count)];
                combination = $"{size}-{color}";
            } while (usedCombinations.Contains(combination));
            
            usedCombinations.Add(combination);
            
            var variant = new ProductVariant
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Product = product,
                Size = size,
                Color = color,
                Price = product.BasePrice + (_random.Next(-500, 1500) / 100m), 
                ImageUrl = GetImageForCategory(mainCategory),
                Sku = $"SKU{skuCounter:D6}",
                StockQuantity = _random.Next(0, 500),
                IsActive = _random.NextDouble() > 0.03, 
                CreatedAt = now.AddDays(-_random.Next(0, 300)),
                ModifiedAt = now.AddDays(-_random.Next(0, 30)),
                CreatedBy = "seeder@system.com",
                ModifiedBy = "seeder@system.com"
            };
            
            variants.Add(variant);
            skuCounter++;
        }
        
        return variants;
    }

    private string GenerateProductName(string categoryName, int index)
    {
        var adjectives = new[] { "Premium", "Classic", "Modern", "Vintage", "Professional", "Casual", "Deluxe", "Standard", "Economy", "Luxury" };
        var materials = new Dictionary<string, string[]>
        {
            ["T-Shirts"] = ["Cotton", "Blend", "Organic", "Bamboo", "Performance"],
            ["Mugs"] = ["Ceramic", "Stainless", "Glass", "Enamel", "Porcelain"],
            ["Business Cards"] = ["Matte", "Glossy", "Textured", "Recycled", "Premium"]
        };
        
        var adjective = adjectives[_random.Next(adjectives.Length)];
        var material = materials.TryGetValue(categoryName, out var value) ? value[_random.Next(value.Length)] : 
            "Quality";
            
        return $"{adjective} {material} {categoryName.TrimEnd('s')} #{index:D3}";
    }

    private string GenerateProductDescription(string categoryName)
    {
        var descriptions = new[]
        {
            $"High-quality {categoryName.ToLower()} perfect for custom printing and personalization.",
            $"Professional-grade {categoryName.ToLower()} designed for durability and print quality.",
            $"Premium {categoryName.ToLower()} suitable for both small and large print runs.",
            $"Versatile {categoryName.ToLower()} ideal for promotional materials and custom designs.",
            $"Top-tier {categoryName.ToLower()} offering excellent print adhesion and longevity."
        };
        
        return descriptions[_random.Next(descriptions.Length)];
    }

    private decimal GenerateBasePrice(string categoryName)
    {
        var priceRanges = new Dictionary<string, (decimal min, decimal max)>
        {
            ["T-Shirts"] = (8.99m, 24.99m),
            ["Hoodies"] = (19.99m, 49.99m),
            ["Mugs"] = (4.99m, 15.99m),
            ["Business Cards"] = (9.99m, 39.99m),
            ["Phone Cases"] = (12.99m, 29.99m)
        };
        
        var (min, max) = priceRanges.TryGetValue(categoryName, value: out var range) ? 
            range : (5.99m, 29.99m);
            
        return Math.Round(min + (max - min) * (decimal)_random.NextDouble(), 2);
    }

    private string GetImageForCategory(string mainCategory)
    {
        return mainCategory switch
        {
            "Textile" => _textileImages[_random.Next(_textileImages.Length)],
            "Hard Surfaces" => _hardSurfaceImages[_random.Next(_hardSurfaceImages.Length)],
            "Paper" => _paperImages[_random.Next(_paperImages.Length)],
            _ => _categoryImages[_random.Next(_categoryImages.Length)]
        };
    }

    private static string GetMainCategoryName(Category category)
    {
        if (category.ParentCategoryId == null)
            return category.Name;
            
        var textileCategories = new[] { "T-Shirts", "Hoodies", "Tank Tops", "Long Sleeves", "Polo Shirts", "Sweatshirts", "Jackets", "Caps & Hats", "Bags", "Towels", "Aprons", "Baby Clothing", "Youth Clothing", "Women's Apparel", "Men's Apparel" };
        var hardSurfaceCategories = new[] { "Mugs", "Water Bottles", "Phone Cases", "Laptop Cases", "Keychains", "Mouse Pads", "Coasters", "Picture Frames", "Awards & Trophies", "Signs", "Magnets", "Buttons & Pins", "Clocks", "Tiles", "Metal Prints" };
        var paperCategories = new[] { "Business Cards", "Flyers", "Brochures", "Posters", "Banners", "Stickers", "Labels", "Notebooks", "Calendars", "Greeting Cards", "Postcards", "Bookmarks", "Menu Cards", "Invitations", "Certificates" };
        
        if (textileCategories.Contains(category.Name)) return "Textile";
        if (hardSurfaceCategories.Contains(category.Name)) return "Hard Surfaces";
        return paperCategories.Contains(category.Name) ? "Paper" : "Textile";
    }

    private static List<string> GetSizesForCategory(string mainCategory)
    {
        return mainCategory switch
        {
            "Textile" => ["XS", "S", "M", "L", "XL", "XXL", "XXXL"],
            "Hard Surfaces" => ["Small", "Medium", "Large", "XL", "Standard"],
            "Paper" => ["A4", "A5", "Letter", "Legal", "Custom", "Standard"],
            _ => ["S", "M", "L", "XL"]
        };
    }

    private static List<string> GetColorsForCategory()
    {
        return
        [
            "White", "Black", "Red", "Blue", "Green", "Yellow", "Purple", "Orange",
            "Pink", "Gray", "Navy", "Maroon", "Forest", "Royal", "Sky", "Lime"
        ];
    }
}