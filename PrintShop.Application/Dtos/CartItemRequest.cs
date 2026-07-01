using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.Application.Dtos
{
    public record CartItemRequest(Guid itemId, int quantity);
}
