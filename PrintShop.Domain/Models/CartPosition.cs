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
        public DateTime AddedAt { get; }

        private CartPosition(Guid id, Guid cartId, Guid productId, int quantity, decimal priceAtMoment, DateTime addedAt)
        {
            Id = id;
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            PriceAtMoment = priceAtMoment;
            AddedAt = addedAt;
        }

        internal CartPosition(Guid id, Guid cartId, Guid productId, int quantity, decimal priceAtMoment, DateTime addedAt)
        {
            Id = id;
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            PriceAtMoment = priceAtMoment;
            AddedAt = addedAt;
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
