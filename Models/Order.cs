using DesafioBMG.Models.Enum;

namespace DesafioBMG.Models
{
    public class Order : Entity
    {
        public Guid UserId { get; set; }
        public string? Comments { get; set; }
        public List<OrderItem> Items { get; set; } = [];
        public decimal Total => Items.Sum(i => i.Price * i.Quantity);
        public string Status { get; set; } = OrderStatusEnum.Pendente.ToString();
    }
}
