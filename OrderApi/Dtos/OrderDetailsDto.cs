namespace OrderApi.Dtos;


public readonly record struct OrderDetailsDto(string OrderId, string productId, DateTime created, decimal stock,
                                                                                      string PName, decimal price, string consumer, string status, string userId);
