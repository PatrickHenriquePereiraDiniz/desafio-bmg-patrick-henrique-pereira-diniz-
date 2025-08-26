using DesafioBMG.Models;
using DesafioBMG.Repositories;

namespace DesafioBMG.Services
{

    public class ProductService(IProductRepository repo)
    {
        private readonly IProductRepository _repo = repo;

        public Product CreateProduct(string name, decimal price, int stock)
        {
            var product = new Product { Name = name, Price = price, Stock = stock };
            _repo.Add(product);
            return product;
        }

        public void UpdateProduct(Product product) => _repo.Update(product);

        public void DeleteProduct(Guid id)
        {
            var product = _repo.GetById(id);
            if (product == null) throw new Exception("Produto não encontrado");
            _repo.Delete(product);
            Console.WriteLine($"[INFO] Produto {product.Name} deletado.");
        }

        public List<Product> GetAllProducts() => _repo.GetAll();
    }
}
