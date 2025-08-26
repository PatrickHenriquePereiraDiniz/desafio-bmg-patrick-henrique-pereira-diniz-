namespace DesafioBMG.Models
{
    public class OrderItem : Entity
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
