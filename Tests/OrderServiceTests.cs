using Microsoft.EntityFrameworkCore;
using DesafioBMG.Models;
using DesafioBMG.Repositories;
using DesafioBMG.Services;
using DesafioBMG.DTOs;
using Xunit;

namespace DesafioBMG.Tests
{
    public class OrderServiceTests
    {
        private readonly OrderService _service;
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;

        public OrderServiceTests()
        {
            var options = new DbContextOptionsBuilder<Data.AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var db = new Data.AppDbContext(options);

            _orderRepo = new EfOrderRepository(db);
            _productRepo = new EfProductRepository(db);
            _service = new OrderService(_orderRepo, _productRepo);

            db.Products.Add(new Product { Name = "Produto Teste", Price = 10, Stock = 100 });
            db.SaveChanges();
        }

        [Fact]
        public void CreateOrder_ShouldReduceStock()
        {
            var product = _productRepo.GetAll().First();

            var items = new List<CreateOrderItemRequest>
            {
                new CreateOrderItemRequest { ProductId = product.Id, Quantity = 5 }
            };

            var order = _service.CreateOrder(Guid.NewGuid(), items);

            var updatedProduct = _productRepo.GetById(product.Id);
            Assert.Equal(95, updatedProduct!.Stock);
            Assert.Single(order.Items);

            Assert.Equal(product.Price, order.Items.First().Price);
        }
    }
}
