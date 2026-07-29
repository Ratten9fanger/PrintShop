using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.Application.Dtos
{
    public record PoductRequest(
        [Required]
        string Title,
        [Required]
        string Description,
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Значение не может быть меньше нуля.")]
        decimal Price,
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Значение не может быть меньше нуля.")]
        int StockQuantity,
        [Required]
        Guid CategoryId
        );
}
