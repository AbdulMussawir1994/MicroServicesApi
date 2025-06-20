namespace OrderApi.Dtos;


public readonly record struct OrderDetailsDto(string OrderId, string productId, DateTime created, int count, string PName, decimal price);
