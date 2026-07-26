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

        private readonly List<CartPosition> _positions;

        public IReadOnlyList<CartPosition> Positions => _positions.AsReadOnly();

        private Cart(Guid id, List<CartPosition>? positions)
        {
            Id = id;
            _positions = positions ?? new List<CartPosition>(); // обрабатываем пустой список из бд
        }

        public static (string? Error, Cart? Cart) Create(Guid id, List<CartPosition>? positions = null)
        {
            return (null, new Cart(id, positions));
        }

        public (string? Error, bool isNew) AddOrUpdatePosition(Guid productId, int quantity, decimal price)
        {
            if (quantity <= 0) 
                return ("Invalid quantity", false);

            if (_positions.Count == 10)
                return ("Cart positions count can't be more than 10", false);

            var existingPosition = _positions.Where(x => x.ProductId == productId).FirstOrDefault();

            if (existingPosition != null)
            {
                existingPosition.IncreaseQuantity();
                return (null, false); // Позиция не новая, просто обновили количество
            }

            var newPosition = CartPosition.Create(Guid.NewGuid(), this.Id, productId, quantity, price);

            if (newPosition.Error != null)
                return (newPosition.Error, false);   

            _positions.Add(newPosition.CartPosition!);

            return (null, true);
        }
         
        public decimal CalculateTotal()
        {
            return _positions.Sum(x => x.PriceAtMoment * x.Quantity);
        }

    }
}
