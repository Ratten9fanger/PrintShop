namespace PrintShop.DataAccess.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int StockQuantity { get; set; } 

        public decimal Price { get; set; }
        public decimal PriceAtMoment { get; set; }

        public string Description { get; set; } = string.Empty;

        //Зависим от категории
        public Guid CategoryId { get; set; }
        public CategoryEntity Category { get; set; } = null!;

    }
}
