namespace OrderApi.RabbitMqConsumer
{
    public interface IRabbitMqConsumerService
    {
        Task<string> ConfirmOrderAsync();
    }
}
