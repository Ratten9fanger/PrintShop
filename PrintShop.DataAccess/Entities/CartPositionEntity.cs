using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.DataAccess.Entities
{
    public class CartPositionEntity
    {
        public Guid Id { get; set; }
        //public Guid UserId { get; set; }
        //public UserEntity? User { get; set; }

        //Зависимы от корзины
        public Guid CartId { get; set; }
        public CartEntity Cart { get; set; } = null!;

        //Зависимы от продукта
        public Guid ProductId { get; set; }
        public ProductEntity Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal PriceAtMoment { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
