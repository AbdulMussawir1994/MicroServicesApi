namespace OrderApi.Dtos;


public readonly record struct OrderDetailsDto(string OrderId, string productId, DateTime created, decimal qty,
                                                                                      string PName, decimal price, string consumer, string status, string userId);
