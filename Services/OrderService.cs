using DesafioBMG.Models;
using DesafioBMG.Repositories;
using DesafioBMG.DTOs;
using DesafioBMG.Models.Enum;

namespace DesafioBMG.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;

        public OrderService(IOrderRepository orderRepo, IProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
        }

        public Order CreateOrder(Guid userId, List<CreateOrderItemRequest> items)
        {
            var orderItems = new List<OrderItem>();

            foreach (var item in items)
            {
                var product = _productRepo.GetById(item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                    throw new Exception($"Estoque insuficiente para {product?.Name ?? "produto desconhecido"}");

                product.Stock -= item.Quantity;
                _productRepo.Update(product);

                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                });
            }

            var order = new Order
            {
                UserId = userId,
                Items = orderItems,
                Status = OrderStatusEnum.Pendente.ToString()
            };

            _orderRepo.Add(order);
            Console.WriteLine($"[INFO] Pedido {order.Id} criado para o usuário {userId}");
            return order;
        }

        public bool PayOrder(Guid orderId, Guid userId, PaymentMethodEnum method)
        {
            var order = _orderRepo.GetById(orderId);
            if (order == null)
            {
                Console.WriteLine($"[ERRO] Pedido {orderId} não encontrado.");
                return false;
            }

            if (order.UserId != userId)
            {
                Console.WriteLine($"[ERRO] Usuário {userId} não tem permissão para pagar o pedido {orderId}.");
                return false;
            }

            if (order.Status == OrderStatusEnum.Pago.ToString())
            {
                Console.WriteLine($"[AVISO] Pedido {orderId} já foi pago.");
                return false;
            }

            order.Status = OrderStatusEnum.Pago.ToString();
            order.Comments = $"Pago via {method}";
            _orderRepo.Update(order);

            Console.WriteLine($"[INFO] Pedido {order.Id} pago via {method} pelo usuário {userId}");
            return true;
        }

        public List<Order> GetOrdersByUser(Guid userId)
        {
            var orders = _orderRepo.GetByUserId(userId);
            Console.WriteLine($"[INFO] Recuperados {orders.Count} pedidos para o usuário {userId}");
            return orders;
        }
    }
}
