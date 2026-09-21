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
        private readonly string _receiptsFolder;

        public PdfHelper(ILogger<PdfHelper> logger)
        {
            _logger = logger;
            // Абсолютный путь на основе рабочей директории приложения
            _receiptsFolder = Path.Combine(AppContext.BaseDirectory, "Receipts");

            // Создаём папку, если её нет (один раз при создании хелпера)
            Directory.CreateDirectory(_receiptsFolder);
        }

        public void CreateReceiptPdf(OrderDto order)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            // Безопасное имя файла: убираем символы, недопустимые в Windows/Linux
            var safeDate = order.CreatedAt.ToString("yyyy-MM-dd_HH-mm-ss");
            var fileName = $"{order.Id}-{safeDate}.pdf";
            var fullPath = Path.Combine(_receiptsFolder, fileName);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4); // A3 слишком большой для чека, советую A4
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text($"Order #{order.Id}")
                        .SemiBold().FontSize(18).FontColor(Colors.Black);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(100);
                            });

                            table.Header(header =>
                            {
                                header.Cell().BorderBottom(2).Padding(8).Text("#");
                                header.Cell().BorderBottom(2).Padding(8).Text("Product");
                                header.Cell().BorderBottom(2).Padding(8).AlignRight().Text("Qty");
                                header.Cell().BorderBottom(2).Padding(8).AlignRight().Text("Price");
                            });

                            int i = 1;
                            foreach (var item in order.Items)
                            {
                                table.Cell().Padding(8).Text($"{i++}");
                                table.Cell().BorderBottom(1).Padding(8).Text(item.ProductName);
                                table.Cell().BorderBottom(1).Padding(8).AlignRight().Text(item.Quantity.ToString());
                                table.Cell().BorderBottom(1).Padding(8).AlignRight().Text($"{item.PriceAtMoment:F2} ₽");
                            }

                            table.Footer(footer =>
                            {
                                footer.Cell().AlignRight().PaddingTop(20).Text("Total:").SemiBold();
                                footer.Cell().AlignRight().PaddingTop(20).Text($"{order.TotalAmount:F2} ₽").SemiBold();
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text("PrintShop API");
                });
            }).GeneratePdf(fullPath);

            _logger.LogInformation("Чек для заказа {OrderId} сохранен: {Path}", order.Id, fullPath);
        }
    }
}
