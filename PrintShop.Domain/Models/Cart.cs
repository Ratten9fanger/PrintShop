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

        public static (string? Error, Cart? Cart) Create(Guid id, List<CartPosition>? cartPositions)
        {
            if (cartPositions.Count <= 10)
            {
                return ("Cart positions count can't be more than 10", null);
            }

            var cart = new Cart(id, cartPositions);

            return (null, cart);
        }

        public string InsertPosition(CartPosition cartPosition)
        {
            if (CartPositions.Count == 10)
            {
                return "Cart positions count can't be more than 10";
            }

            CartPositions.Add(cartPosition);
        }

        public static Cart IncreaseQIfPositionExists(Guid productId)
        {
            var existingPosition = CartPositions.Where(x => x.ProductId == productId).FirstOrDefault();

            if (existingPosition != null)
            {
                existingPosition.IncreaseQuantity();
            }
            


        }

        public static decimal CalculateTotal(List<CartPosition>? cartPositions)
        {
            if (cartPositions == null) return 0;

            var total = cartPositions.Sum(x => x.PriceAtMoment);
            
            return total;
        }

    }
}
