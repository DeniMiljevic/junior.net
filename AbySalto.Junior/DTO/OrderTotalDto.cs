using AbySalto.Junior.Enums;

namespace AbySalto.Junior.DTO;

public class OrderTotalDto
{
    public decimal TotalAmount { get; set; }
    public Currency Currency { get; set; }
}
