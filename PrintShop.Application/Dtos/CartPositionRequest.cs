using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.Application.Dtos
{
    public record CartPositionRequest(
        Guid productId,
        [Range(0, int.MaxValue, ErrorMessage = "Значение не может быть меньше нуля.")]
        int quantity
        );
}
