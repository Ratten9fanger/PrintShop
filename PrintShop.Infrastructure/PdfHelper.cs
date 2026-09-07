using Microsoft.Extensions.Logging;
using PrintShop.Application.Dtos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PrintShop.Infrastructure
{
    public class PdfHelper : IPdfHelper
    {
        private readonly ILogger<PdfHelper> _logger;

        public PdfHelper(ILogger<PdfHelper> logger)
        {
            _logger = logger;
        }

        public void CreateReceiptPdf(OrderDto order)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

                    page.Header()
                        .Text($"Order #{order.Id}")
                        .SemiBold().FontSize(36).FontColor(Colors.Black);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            // 1. Определение колонок
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(150);   // Фиксированная ширина в пунктах
                                columns.RelativeColumn();     // Занимает всю оставшуюся ширину
                                columns.ConstantColumn(200);  // Фиксированная ширина
                            });

                            table.Header(header =>
                            {
                                header.Cell().BorderBottom(2).Padding(8).Text("#");
                                header.Cell().BorderBottom(2).Padding(8).Text("Product Name");
                                header.Cell().BorderBottom(2).Padding(8).Text("Quantity");
                                header.Cell().BorderBottom(2).Padding(8).AlignRight().Text("Price");
                            });

                            int i = 0;

                            foreach (var item in order.Items)
                            {
                                table.Cell().Padding(8).Text($"{i++}");

                                table.Cell().BorderBottom(2).Padding(8).Text(item.ProductName);
                                table.Cell().BorderBottom(2).Padding(8).Text(item.Quantity.ToString());
                                table.Cell().BorderBottom(2).Padding(8).AlignRight().Text(item.PriceAtMoment.ToString());
                            }

                            table.Footer(footer =>
                            {
                                footer.Cell().AlignCenter().PaddingTop(20).Text(order.TotalAmount.ToString());
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text("PrintShop API");
                });
            }).GeneratePdf($"./Receipts/{order.Id}-{order.CreatedAt:yyyy-MM-dd_HH-mm-ss}.pdf");

            _logger.LogInformation("Чек для заказа {order} сохранен", order);
        }
    }
}
