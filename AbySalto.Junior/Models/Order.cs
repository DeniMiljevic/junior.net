using AbySalto.Junior.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbySalto.Junior.Models;

public class Order
{
    public int Id { get; set; }
    public string BuyerName { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDateTime { get; set; }
    public PaymentType PaymentType { get; set; }
    public string DeliveryAddress { get; set; }
    public string ContactNumber { get; set; }
    public string Remark { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    [NotMapped]
    public decimal TotalAmount => Items?.Sum(i => i.Price * i.Quantity) ?? 0;

}
