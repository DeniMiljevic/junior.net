using AbySalto.Junior.Enums;


namespace AbySalto.Junior.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string BuyerName { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDateTime { get; set; }
        public PaymentType PaymentType { get; set; }
        public string DeliveryAddress { get; set; }
        public string ContactNumber { get; set; }
        public string Remark { get; set; }
        public Currency Currency { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
