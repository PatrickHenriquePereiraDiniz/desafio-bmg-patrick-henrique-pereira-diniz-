namespace DesafioBMG.Models
{
    public class User : Entity
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "Customer"; // Admin ou Customer
    }
}
