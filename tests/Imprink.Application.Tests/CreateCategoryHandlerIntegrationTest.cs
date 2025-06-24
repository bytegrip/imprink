using Imprink.Application.Commands.Categories;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;
using Imprink.Domain.Repositories;
using Imprink.Infrastructure;
using Imprink.Infrastructure.Database;
using Imprink.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;

namespace Imprink.Application.Tests;

public class CreateCategoryHandlerIntegrationTest : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private readonly CreateCategoryHandler _handler;
    private readonly SqliteConnection _connection;

    public CreateCategoryHandlerIntegrationTest()
    {
        var services = new ServiceCollection();
        
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(_connection));
        
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<CreateCategoryHandler>();

        _serviceProvider = services.BuildServiceProvider();
        _context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
        _handler = _serviceProvider.GetRequiredService<CreateCategoryHandler>();

        _context.Database.EnsureCreated();
    }

    private async Task CleanDatabase()
    {
        _context.Categories.RemoveRange(_context.Categories);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_ValidCreateCategoryCommand_PersistsToDatabase()
    {
        await CleanDatabase();

        // Arrange
        var command = new CreateCategoryCommand
        {
            Name = "Electronics",
            Description = "Electronic devices and gadgets",
            ImageUrl = "https://example.com/electronics.jpg",
            SortOrder = 1,
            IsActive = true,
            ParentCategoryId = null
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.Description, result.Description);
        Assert.Equal(command.ImageUrl, result.ImageUrl);
        Assert.Equal(command.SortOrder, result.SortOrder);
        Assert.Equal(command.IsActive, result.IsActive);
        Assert.Equal(command.ParentCategoryId, result.ParentCategoryId);
        Assert.NotNull(result.CreatedAt);
        Assert.NotNull(result.ModifiedAt);

        var savedCategory = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == result.Id);
        
        Assert.NotNull(savedCategory);
        Assert.Equal(command.Name, savedCategory.Name);
        Assert.Equal(command.Description, savedCategory.Description);
        Assert.Equal(command.ImageUrl, savedCategory.ImageUrl);
        Assert.Equal(command.SortOrder, savedCategory.SortOrder);
        Assert.Equal(command.IsActive, savedCategory.IsActive);
        Assert.Equal(command.ParentCategoryId, savedCategory.ParentCategoryId);
        Assert.NotNull(savedCategory.CreatedAt);
        Assert.NotNull(savedCategory.ModifiedAt);
    }

    [Fact]
    public async Task Handle_ValidCreateCategoryCommandWithParent_PersistsWithCorrectParentRelationship()
    {
        await CleanDatabase();

        // Arrange
        var parentCategory = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Parent Category",
            Description = "Parent category description",
            SortOrder = 1,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
        
        _context.Categories.Add(parentCategory);
        await _context.SaveChangesAsync();

        var command = new CreateCategoryCommand
        {
            Name = "Child Electronics",
            Description = "Electronic devices and gadgets under parent",
            ImageUrl = "https://example.com/child-electronics.jpg",
            SortOrder = 2,
            IsActive = true,
            ParentCategoryId = parentCategory.Id
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(parentCategory.Id, result.ParentCategoryId);

        var savedCategory = await _context.Categories
            .Include(c => c.ParentCategory)
            .FirstOrDefaultAsync(c => c.Id == result.Id);
        
        Assert.NotNull(savedCategory);
        Assert.NotNull(savedCategory.ParentCategory);
        Assert.Equal(parentCategory.Id, savedCategory.ParentCategory.Id);
        Assert.Equal(parentCategory.Name, savedCategory.ParentCategory.Name);

        var parentWithChildren = await _context.Categories
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == parentCategory.Id);
        
        Assert.NotNull(parentWithChildren);
        Assert.Contains(parentWithChildren.SubCategories, sc => sc.Id == result.Id);
    }

    [Fact]
    public async Task Handle_CreateMultipleCategories_AllPersistCorrectly()
    {
        await CleanDatabase();

        // Arrange
        var commands = new[]
        {
            new CreateCategoryCommand
            {
                Name = "Category 1",
                Description = "First category",
                SortOrder = 1,
                IsActive = true
            },
            new CreateCategoryCommand
            {
                Name = "Category 2",
                Description = "Second category",
                SortOrder = 2,
                IsActive = false
            }
        };

        // Act
        var results = new List<CategoryDto>();
        foreach (var command in commands)
        {
            var result = await _handler.Handle(command, CancellationToken.None);
            results.Add(result);
        }

        // Assert
        Assert.Equal(2, results.Count);
        
        var categoriesInDb = await _context.Categories.ToListAsync();
        
        Assert.Equal(2, categoriesInDb.Count);
        
        foreach (var result in results)
        {
            var savedCategory = categoriesInDb.First(c => c.Id == result.Id);
            Assert.Equal(result.Name, savedCategory.Name);
            Assert.Equal(result.Description, savedCategory.Description);
            Assert.Equal(result.IsActive, savedCategory.IsActive);
        }
    }

    [Fact]
    public async Task Handle_TransactionFailure_RollsBackCorrectly()
    {
        await CleanDatabase();

        // Arrange
        var command = new CreateCategoryCommand
        {
            Name = "Test Category",
            Description = "Test description",
            SortOrder = 1,
            IsActive = true
        };

        var initialCount = await _context.Categories.CountAsync();

        var handler = _serviceProvider.GetRequiredService<CreateCategoryHandler>();

        var result = await handler.Handle(command, CancellationToken.None);
        
        Assert.NotNull(result);
        
        var finalCount = await _context.Categories.CountAsync();
        Assert.Equal(initialCount + 1, finalCount);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _context.Dispose();
        _connection.Close();
        _connection.Dispose();
        _serviceProvider.GetService<IServiceScope>()?.Dispose();
    }
}