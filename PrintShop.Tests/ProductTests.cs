using PrintShop.Domain.Models;

namespace PrintShop.Tests;

public class ProductTests
{
    [Fact]
    public void ProductName_ShouldBe_NotNull()
    {
        // Arrange
        var p_id = Guid.NewGuid();
        var c_id = Guid.NewGuid();

        // Act
        var (error, product) = Product.Create(p_id, "", "22", 10, 10, c_id);

        // Assert
        Assert.NotNull(error);
        Assert.Null(product);
    }
}
