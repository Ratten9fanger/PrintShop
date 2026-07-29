using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.DataAccess.Entities
{
    public class CartEntity
    {
        public Guid Id { get; set; }

        public Guid? UserId { get; set; }
        public UserEntity? User { get; set; }

        //Родители для продукта в корзине
        public List<CartPositionEntity> CartPositions { get; set; } = new List<CartPositionEntity>();
    }
}
