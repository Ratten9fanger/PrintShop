using PrintShop.Application.Dtos;

namespace PrintShop.Infrastructure
{
    public interface IPdfHelper
    {
        void CreateReceiptPdf(OrderDto order);
    }
}