namespace PrintShop.NotificationService.Contracts
{
    public record OrderDto(Guid Id, DateTime CreatedAt, decimal TotalAmount, List<OrderItemDto> Items);
    public record OrderItemDto(string ProductName, int Quantity, decimal PriceAtMoment);
}
