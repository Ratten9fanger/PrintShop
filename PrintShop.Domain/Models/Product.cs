namespace PrintShop.Domain.Models
{
    public class Product
    {
        public const int TITLE_MAX_LENGTH = 100;

        public Guid Id { get; }
        public string Title { get; }
        public string? Description { get; }
        public decimal Price { get; }
        public int StockQuantity { get; }
        public Guid CategoryId { get; }

        private Product(
            Guid id,
            string title,
            string description,
            decimal price,
            int stockQuantity,
            Guid categoryId)
        {
            Id = id; 
            Title = title;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            CategoryId = categoryId;
        }

        public (string? error, Product? product) Create(
            Guid id,
            string title,
            string description,
            decimal price,
            int stockQuantity,
            Guid categoryId)
        {

            if (string.IsNullOrWhiteSpace(title))
                return("The title is null", null);

            if (title.Length > TITLE_MAX_LENGTH)
                return($"The title can't be more than {TITLE_MAX_LENGTH} chars", null);

            if (price <= 0)
                return("Price can't be equal or less than 0", null);

            if (stockQuantity < 0)
                return("Stock can't be less than 0", null);

            var product = new Product(id, title, description, price, stockQuantity, categoryId);

            return (null, product);

        }

    }
}
