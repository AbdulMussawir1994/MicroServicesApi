namespace ProductsApi.RabbitMqProducer;

public interface IRabbitMqService
{
    void PublishMessage<T>(string queueName, T message);
}
