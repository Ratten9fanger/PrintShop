namespace PrintShop.Application.Dtos
{
    public record OrderEventDto(
        Guid Id,
        DateTime CreatedAt,
        decimal TotalAmount,
        List<OrderItemDto> Items,
        string UserEmail);
}
