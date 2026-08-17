namespace PrintShop.Application.Dtos
{
    public record CartPositionDto(Guid Id, Guid ProductId, int Quantity, decimal PriceAtMoment);
    public record CartDto(Guid Id, List<CartPositionDto> Positions);
}
