using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.DataAccess.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public int StockQuantity { get; set; } = null!

        public decimal Price { get; set; }
        public decimal PriceAtMoment { get; set; }

        public string? Description { get; set; }

        //Зависим от категории
        public Guid CategoryId { get; set; }
        public CategoryEntity Category { get; set; } = null!;

        //Родители для продуктов в корзине
        public List<CartProductEntity> cartProducts { get; set; } = new();
    }
}
