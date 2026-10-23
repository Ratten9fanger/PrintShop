namespace PrintShop.NotificationService.Contracts
{
    public record OrderEventDto(
        Guid Id,
        DateTime CreatedAt,
        decimal TotalAmount,
        List<OrderItemDto> Items,
        string UserEmail);

    public record OrderItemDto(
        string ProductName,
        int Quantity,
        decimal PriceAtMoment);
}
