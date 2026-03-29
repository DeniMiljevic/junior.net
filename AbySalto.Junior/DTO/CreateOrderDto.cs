namespace AbySalto.Junior.DTO;

using AbySalto.Junior.Enums;
using System.Collections.Generic;

public class CreateOrderDto
{
    public string BuyerName { get; set; }
    public PaymentType PaymentType { get; set; }
    public string DeliveryAddress { get; set; }
    public string ContactNumber { get; set; }
    public string Remark { get; set; }
    public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
}

public class OrderItemDto
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
