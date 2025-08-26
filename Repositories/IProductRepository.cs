using DesafioBMG.Models;

namespace DesafioBMG.Repositories
{
    public interface IProductRepository
    {
        Product? GetById(Guid id);
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
        List<Product> GetAll();
    }
}
