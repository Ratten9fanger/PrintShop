namespace PrintShop.DataAccess.Entities
{
    public class OrderEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }

        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;

        public List<OrderItemEntity> OrderItems { get; set; } = null!;
    }
}
