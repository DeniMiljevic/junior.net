using AbySalto.Junior.Enums;

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
    public Currency Currency { get; set; } = Currency.EUR;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
