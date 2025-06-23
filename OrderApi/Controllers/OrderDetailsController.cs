using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Dtos;
using OrderApi.RabbitMqConsumer;
using OrderApi.Repository;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IRabbitMqConsumerService _consumerService;

        public OrderDetailsController(IOrderService orderService, IRabbitMqConsumerService consumerService)
        {
            _orderService = orderService;
            _consumerService = consumerService;
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmOrder()
        {
            var result = await _consumerService.ConfirmOrderAsync();
            return Ok(result);
        }

        [HttpGet("List")]
        public async Task<ActionResult> GetAll(CancellationToken ctx)
        {
            var response = await _orderService.GetAllAsync(ctx);
            return response.Status ? Ok(response) : NotFound(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id, CancellationToken ctx)
        {
            if (id <= 0)
                return BadRequest("Order ID must be greater than 0.");

            var response = await _orderService.GetByIdAsync(id, ctx);
            return response.Status ? Ok(response) : NotFound(response);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] CreateOrderDetailsDto model, CancellationToken ctx)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _orderService.CreateAsync(model, ctx);
            return response.Status ? CreatedAtAction(nameof(Create), response) : BadRequest(response);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] CreateOrderDetailsDto model, CancellationToken ctx)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _orderService.UpdateAsync(id, model, ctx);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult> Delete(int id, CancellationToken ctx)
        {
            if (id <= 0)
                return BadRequest("Invalid Order ID.");

            var response = await _orderService.DeleteAsync(id, ctx);
            return response.Status ? Ok(response) : NotFound(response);
        }
    }
}
