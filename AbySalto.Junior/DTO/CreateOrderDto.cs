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
    public Currency Currency { get; set; } = Currency.EUR;
    public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
}
