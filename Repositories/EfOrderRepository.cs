using Microsoft.EntityFrameworkCore;
using DesafioBMG.Data;
using DesafioBMG.Models;

namespace DesafioBMG.Repositories
{
    public class EfOrderRepository(AppDbContext db) : IOrderRepository
    {
        private readonly AppDbContext _db = db;

        public Order? GetById(Guid id) => _db.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == id);

        public void Add(Order order)
        {
            _db.Orders.Add(order);
            _db.SaveChanges();
        }

        public void Update(Order order)
        {
            _db.Orders.Update(order);
            _db.SaveChanges();
        }

        public List<Order> GetByUserId(Guid userId) => _db.Orders.Include(o => o.Items).Where(o => o.UserId == userId).ToList();
    }
}
