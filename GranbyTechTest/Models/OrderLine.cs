namespace GranbyTechTest.Models
{
    public class OrderLine
    {
        public int ProductId { get; }
        public int Quantity { get; }

        public OrderLine(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}