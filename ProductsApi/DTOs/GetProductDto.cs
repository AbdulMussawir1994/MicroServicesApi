namespace ProductsApi.DTOs;

public readonly record struct GetProductDto(string id, string product, string description, string category, decimal qty,
                                                                                    decimal price, DateTime created, string createdBy, string image);

