using DesafioBMG.Data;
using DesafioBMG.Models;

namespace DesafioBMG.Repositories
{
    public class EfProductRepository(AppDbContext db) : IProductRepository
    {
        private readonly AppDbContext _db = db;

        public Product? GetById(Guid id) => _db.Products.Find(id);

        public void Add(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
        }

        public void Update(Product product)
        {
            _db.Products.Update(product);
            _db.SaveChanges();
        }

        public void Delete(Product product)
        {
            _db.Products.Remove(product);
            _db.SaveChanges();
        }

        public List<Product> GetAll() => _db.Products.ToList();
    }
}
