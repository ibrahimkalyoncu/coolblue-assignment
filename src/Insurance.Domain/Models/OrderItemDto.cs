namespace Insurance.Domain.Models
{
    public class OrderItemDto
    {
        public OrderItemDto(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
        
        public int ProductId { get; }
        public int Quantity { get; }
    }
}