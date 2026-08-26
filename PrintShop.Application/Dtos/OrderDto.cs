namespace PrintShop.Application.Dtos
{

        public record OrderDto(Guid Id, DateTime CreatedAt, decimal TotalAmount, List<OrderItemDto> Items);

}
