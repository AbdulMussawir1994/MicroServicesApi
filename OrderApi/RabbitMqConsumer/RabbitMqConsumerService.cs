using KmacHelper.ConsumerModel;
using OrderApi.Repository;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OrderApi.RabbitMqConsumer
{
    public class RabbitMqConsumerService : IRabbitMqConsumerService
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMqConsumerService> _logger;

        public RabbitMqConsumerService(IOrderService orderService, IConfiguration configuration, ILogger<RabbitMqConsumerService> logger)
        {
            _orderService = orderService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> ConfirmOrderAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"],
                Port = int.Parse(_configuration["RabbitMQ:Port"]),
                UserName = _configuration["RabbitMQ:Username"],
                Password = _configuration["RabbitMQ:Password"]
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("ProductQueue", durable: true, exclusive: false, autoDelete: false);
            channel.BasicQos(0, 1, false); // One message at a time

            var result = channel.BasicGet(queue: "ProductQueue", autoAck: false);

            if (result == null)
            {
                _logger.LogWarning("No message in queue.");
                return "No order to process.";
            }

            var body = result.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            _logger.LogInformation("Fetched message: " + json);

            try
            {
                var product = JsonSerializer.Deserialize<ProductDto>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (product == null)
                {
                    channel.BasicAck(result.DeliveryTag, false); // Prevent stuck messages
                    return "Invalid message format.";
                }

                //     await _orderService.AutoCreateAsync(product);

                channel.BasicAck(result.DeliveryTag, false);
                return $"Order created successfully for Product: {product.ProductName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process order.");
                channel.BasicNack(result.DeliveryTag, false, true); // Retry later
                return "Failed to process order.";
            }
        }

        //private async Task<bool> CallOrderConfirmationApiAsync(Product product)
        //{
        //    var client = _httpClientFactory.CreateClient();
        //    var apiUrl = _configuration["ExternalServices:OrderApiUrl"]; // e.g., http://orderapi/api/orders/confirm

        //    var response = await client.PostAsJsonAsync(apiUrl, product);
        //    return response.IsSuccessStatusCode;

        //    "ExternalServices": {
        //        "OrderApiUrl": "http://orderapi/api/orders/confirm"
        //    }
        //}
    }
}
