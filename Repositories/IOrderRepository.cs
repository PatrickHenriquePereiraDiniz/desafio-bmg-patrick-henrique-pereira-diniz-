using DesafioBMG.Models;

namespace DesafioBMG.Repositories
{
    public interface IOrderRepository
    {
        Order? GetById(Guid id);
        void Add(Order order);
        void Update(Order order);
        List<Order> GetByUserId(Guid userId);
    }
}
