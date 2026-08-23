namespace PrintShop.DataAccess.Entities
{
    public class CategoryEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public List<ProductEntity>? Products { get; set; }
    }
}
