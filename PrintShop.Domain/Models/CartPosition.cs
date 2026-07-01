using System.ComponentModel.DataAnnotations;

namespace PrintShop.Domain.Models
{
    public class CartPosition
    {
        public Guid Id { get; }
        public Guid CartId { get; }
        public Guid ProductId { get; }
        public int Quantity { get; }
        public decimal PriceAtMoment { get; }

        private CartPosition(Guid id, Guid cartId, Guid productId, int quantity, decimal priceAtMoment)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Количество должно быть больше 0");

            Id = id;
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            PriceAtMoment = priceAtMoment;
        }

        internal CartPosition(Guid id, Guid cartId, Guid productId, int quantity, decimal priceAtMoment)
        {
            Id = id;
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            PriceAtMoment = priceAtMoment;
        }

        public (string? error, CartPosition? сartPosition) Create(
            Guid id,
            string email,
            string role,
            string passwordHash)
        { 
            if (string.IsNullOrWhiteSpace(email))
                return("The email is null", null);

            var user = new User(id, email, role, passwordHash);

            return (null, user);

        }

}
}
