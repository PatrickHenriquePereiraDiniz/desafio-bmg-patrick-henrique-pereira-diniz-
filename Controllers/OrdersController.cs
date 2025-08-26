using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DesafioBMG.DTOs;
using DesafioBMG.Services;
using DesafioBMG.Models.Enum;

namespace DesafioBMG.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize(Roles = "Customer")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _service;
        public OrdersController(OrderService service) => _service = service;

        [HttpPost]
        public IActionResult Create([FromBody] CreateOrderRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("ID do usuário não encontrado no token.");

            var userId = Guid.Parse(userIdClaim.Value);

            var order = _service.CreateOrder(userId, request.Items);
            return Ok(order);
        }

        [HttpPost("{orderId}/pay")]
        public IActionResult Pay(Guid orderId, [FromQuery] PaymentMethodEnum method)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("ID do usuário não encontrado no token.");

            var userId = Guid.Parse(userIdClaim.Value);

            var result = _service.PayOrder(orderId, userId, method);
            if (!result)
                return BadRequest("Falha no pagamento ou pedido não encontrado.");

            return Ok(new { orderId, Status = OrderStatusEnum.Pago.ToString(), MetodoPagamento = method.ToString() });
        }

        [HttpGet]
        public IActionResult GetHistory()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("ID do usuário não encontrado no token.");

            var userId = Guid.Parse(userIdClaim.Value);
            var orders = _service.GetOrdersByUser(userId);
            return Ok(orders);
        }
    }
}
