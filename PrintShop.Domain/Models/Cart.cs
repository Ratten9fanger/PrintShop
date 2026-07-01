using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.Domain.Models
{
    public class Cart
    {
        public Guid Id { get; }

        private readonly List<CartPosition>? CartPositions = new();

        private Cart(Guid id, List<CartPosition>? cartPositions)
        {
            Id = id;
            CartPositions = cartPositions;
        }

        public static Cart Create(Guid id, List<CartPosition>? cartPositions)
        {
            Cart cart = new Cart(id, cartPositions);

            return cart;
        }

        public static decimal CalculateTotal(List<CartPosition>? cartPositions)
        {
            if (cartPositions == null) return 0;

            var total = cartPositions.Sum(x => x.PriceAtMoment);
            
            return total;
        }

    }
}
