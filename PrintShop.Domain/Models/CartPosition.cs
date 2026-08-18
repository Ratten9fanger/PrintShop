namespace PrintShop.Domain.Models
{
    public class CartPosition
    {
        public Guid Id { get; }
        public Guid ProductId { get; }
        public int Quantity { get; private set; }
        public decimal PriceAtMoment { get; private set; }

        private CartPosition(
            Guid id,
            Guid productId,
            int quantity,
            decimal priceAtMoment)
        {
            Id = id;
            ProductId = productId;
            Quantity = quantity;
            PriceAtMoment = priceAtMoment;
        }


        public static (string? Error, CartPosition? CartPosition) Create(
            Guid id,
            Guid productId,
            int quantity,
            decimal priceAtMoment)
        { 
            if (quantity <= 0) return ("Quantity can't be equal or less than 0", null);

            var cartPosition = new CartPosition(id, productId, quantity, priceAtMoment);

            return (null, cartPosition);

        }

        public void IncreaseQuantity()
        {
            Quantity++;
        }

        public bool DecreaseQuantity()
        {
            Quantity--;

            if (Quantity <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
