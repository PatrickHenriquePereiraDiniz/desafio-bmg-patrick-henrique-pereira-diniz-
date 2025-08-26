using System.Text.Json.Serialization;

namespace DesafioBMG.Models
{
    public abstract class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
