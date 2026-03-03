using Xunit;
using Pr1.MinWebService.Errors;
using Pr1.MinWebService.Services;

namespace Pr1.MinWebService.Tests;

public class ItemValidationTests
{
    private readonly IItemRepository _repository;

    public ItemValidationTests()
    {
        // Arrange: Create a fresh repository for each test
        _repository = new InMemoryItemRepository();
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsValidationException()
    {
        // Act & Assert: Try to create with empty name
        var exception = Assert.Throws<ValidationException>(
            () => _repository.Create("", 10)
        );

        // Verify the error code is correct
        Assert.Equal("validation", exception.Code);
        Assert.Contains("name", exception.Message.ToLower());
    }

    [Fact]
    public void Create_WithNegativePrice_ThrowsValidationException()
    {
        // Act & Assert: Try to create with negative price
        var exception = Assert.Throws<ValidationException>(
            () => _repository.Create("Valid Item", -5)
        );

        // Verify the error code is correct
        Assert.Equal("validation", exception.Code);
        Assert.Contains("price", exception.Message.ToLower());
    }

    [Fact]
    public void Create_WithValidData_ReturnsItem()
    {
        // Act: Create with valid data
        var item = _repository.Create("Programming Book", 49.99m);

        // Assert: Verify the item was created correctly
        Assert.NotEqual(Guid.Empty, item.Id);
        Assert.Equal("Programming Book", item.Name);
        Assert.Equal(49.99m, item.Price);
    }

    [Fact]
    public void GetById_WithNonExistentId_ThrowsNotFoundException()
    {
        // Act & Assert: Try to get item that doesn't exist
        var exception = Assert.Throws<NotFoundException>(
            () => _repository.GetById(Guid.NewGuid())
        );

        // Verify the error code is correct
        Assert.Equal("not_found", exception.Code);
    }
}