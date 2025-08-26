namespace DesafioBMG.Models
{
    public class Product : Entity
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
