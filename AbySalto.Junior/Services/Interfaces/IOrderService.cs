using AbySalto.Junior.DTO;

namespace AbySalto.Junior.Services.Interfaces;

public interface IOrderService
{
    Task Create(CreateOrderDto order, CancellationToken ct);
    Task<IEnumerable<OrderDto>> GetAll(bool orderByTotal, bool descending, CancellationToken ct);
    Task UpdateStatus(int orderId, UpdateOrderDto updateOrderDto, CancellationToken ct);
    Task<OrderTotalDto> GetOrderTotal(int id, CancellationToken ct);
}
