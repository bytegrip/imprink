using Imprink.Application.Commands.Categories;
using Imprink.Application.Exceptions;
using Imprink.Domain.Entities;
using Imprink.Domain.Repositories;
using Moq;

namespace Imprink.Application.Tests;

public class UpdateCategoryTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly UpdateCategory _handler;

    public UpdateCategoryTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        
        _unitOfWorkMock.Setup(x => x.CategoryRepository)
            .Returns(_categoryRepositoryMock.Object);
        
        _handler = new UpdateCategory(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldUpdateCategoryAndReturnDto()
    {
        var categoryId = Guid.NewGuid();
        var parentCategoryId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow.AddDays(-10);
        var modifiedAt = DateTime.UtcNow;

        var existingCategory = new Category
        {
            Id = categoryId,
            Name = "Old Name",
            Description = "Old Description",
            ImageUrl = "old-image.jpg",
            SortOrder = 1,
            IsActive = true,
            ParentCategoryId = null,
            CreatedAt = createdAt,
            ModifiedAt = createdAt
        };

        var updatedCategory = new Category
        {
            Id = categoryId,
            Name = "New Name",
            Description = "New Description",
            ImageUrl = "new-image.jpg",
            SortOrder = 2,
            IsActive = false,
            ParentCategoryId = parentCategoryId,
            CreatedAt = createdAt,
            ModifiedAt = modifiedAt
        };

        var command = new UpdateCategoryCommand
        {
            Id = categoryId,
            Name = "New Name",
            Description = "New Description",
            ImageUrl = "new-image.jpg",
            SortOrder = 2,
            IsActive = false,
            ParentCategoryId = parentCategoryId
        };

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        _categoryRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedCategory);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(categoryId, result.Id);
        Assert.Equal("New Name", result.Name);
        Assert.Equal("New Description", result.Description);
        Assert.Equal("new-image.jpg", result.ImageUrl);
        Assert.Equal(2, result.SortOrder);
        Assert.False(result.IsActive);
        Assert.Equal(parentCategoryId, result.ParentCategoryId);
        Assert.Equal(createdAt, result.CreatedAt);
        Assert.Equal(modifiedAt, result.ModifiedAt);

        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        
        _categoryRepositoryMock.Verify(x => x.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()), Times.Once);
        _categoryRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CategoryNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new UpdateCategoryCommand
        {
            Id = categoryId,
            Name = "Test Name",
            Description = "Test Description",
            SortOrder = 1,
            IsActive = true
        };

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Equal($"Category with ID {categoryId} not found.", exception.Message);

        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        
        _categoryRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdateThrowsException_ShouldRollbackTransactionAndRethrow()
    {
        var categoryId = Guid.NewGuid();
        var existingCategory = new Category
        {
            Id = categoryId,
            Name = "Existing Name",
            Description = "Existing Description",
            SortOrder = 1,
            IsActive = true
        };

        var command = new UpdateCategoryCommand
        {
            Id = categoryId,
            Name = "New Name",
            Description = "New Description",
            SortOrder = 2,
            IsActive = false
        };

        var expectedException = new InvalidOperationException("Database error");

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        _categoryRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        var thrownException = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        Assert.Same(expectedException, thrownException);

        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_CategoryWithNullableFields_ShouldHandleNullValues()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow.AddDays(-5);
        var modifiedAt = DateTime.UtcNow;

        var existingCategory = new Category
        {
            Id = categoryId,
            Name = "Test Category",
            Description = "Test Description",
            ImageUrl = "test-image.jpg",
            SortOrder = 1,
            IsActive = true,
            ParentCategoryId = Guid.NewGuid(),
            CreatedAt = createdAt,
            ModifiedAt = createdAt
        };

        var updatedCategory = new Category
        {
            Id = categoryId,
            Name = "Updated Category",
            Description = "Updated Description",
            ImageUrl = null,
            SortOrder = 5,
            IsActive = false,
            ParentCategoryId = null,
            CreatedAt = createdAt,
            ModifiedAt = modifiedAt
        };

        var command = new UpdateCategoryCommand
        {
            Id = categoryId,
            Name = "Updated Category",
            Description = "Updated Description",
            ImageUrl = null,
            SortOrder = 5,
            IsActive = false,
            ParentCategoryId = null
        };

        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        _categoryRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedCategory);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categoryId, result.Id);
        Assert.Equal("Updated Category", result.Name);
        Assert.Equal("Updated Description", result.Description);
        Assert.Null(result.ImageUrl);
        Assert.Equal(5, result.SortOrder);
        Assert.False(result.IsActive);
        Assert.Null(result.ParentCategoryId);
        Assert.Equal(createdAt, result.CreatedAt);
        Assert.Equal(modifiedAt, result.ModifiedAt);

        _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}