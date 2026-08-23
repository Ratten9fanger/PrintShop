namespace PrintShop.DataAccess.Entities
{
    public class OrderItemEntity
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtMoment { get; set; }

        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public ProductEntity Product { get; set; } = null!;

        //Зависим от какого-то Заказа
        public Guid OrderId { get; set; }
        public OrderEntity Order { get; set; } = null!;

    }
}
