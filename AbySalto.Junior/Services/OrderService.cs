using AbySalto.Junior.DTO;
using AbySalto.Junior.Infrastructure.Database;
using AbySalto.Junior.Models;
using AbySalto.Junior.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AbySalto.Junior.Enums;

namespace AbySalto.Junior.Services;

public class OrderService : IOrderService
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CreateOrderDto> _validator;

    private static readonly Dictionary<Currency, decimal> ExchangeRates = new()
    {
        { Currency.EUR, 1.0m },
        { Currency.USD, 1.08m },
        { Currency.GBP, 0.86m },
        { Currency.HRK, 7.53m }
    };

    public OrderService(IApplicationDbContext context, IValidator<CreateOrderDto> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task Create(CreateOrderDto order, CancellationToken ct)
    {
        var result = await _validator.ValidateAsync(order, ct);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var itemIds = order.Items.Select(i => i.ItemId).Distinct().ToList();
        var itemPrices = await _context.Items
            .Where(i => itemIds.Contains(i.Id))
            .ToDictionaryAsync(i => i.Id, i => i.Price, ct);

        var missingIds = itemIds.Where(id => !itemPrices.ContainsKey(id)).ToList();
        if (missingIds.Any())
            throw new ArgumentException($"Items not found: {string.Join(", ", missingIds)}");

        var entity = new Order
        {
            BuyerName = order.BuyerName,
            PaymentType = order.PaymentType,
            DeliveryAddress = order.DeliveryAddress,
            ContactNumber = order.ContactNumber,
            Remark = order.Remark,
            Currency = order.Currency,
            OrderDateTime = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            Items = order.Items
                .Select(i => new OrderItem
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    Price = itemPrices[i.ItemId] * ExchangeRates[order.Currency]
                })
                .ToList()
        };

        _context.Orders.Add(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<OrderDto>> GetAll(bool orderByTotal, bool descending, CancellationToken ct)
    {
        var query = _context.Orders
            .AsNoTracking()
            .Select(o => new OrderDto
            {
                Id = o.Id,
                BuyerName = o.BuyerName,
                PaymentType = o.PaymentType,
                DeliveryAddress = o.DeliveryAddress,
                ContactNumber = o.ContactNumber,
                Remark = o.Remark,
                OrderDateTime = o.OrderDateTime,
                Status = o.Status,
                Currency = o.Currency,
                TotalAmount = o.Items.Sum(i => i.Price * i.Quantity),
                Items = o.Items
                    .Select(i => new OrderItemDto
                    {
                        ItemId = i.ItemId,
                        ItemName = i.Item.Name,
                        Quantity = i.Quantity,
                        Price = i.Price
                    })
                    .ToList()
            });

        if (orderByTotal)
            query = descending
                ? query.OrderByDescending(o => o.TotalAmount)
                : query.OrderBy(o => o.TotalAmount);

        return await query.ToListAsync(ct);
    }

    public async Task UpdateStatus(int orderId, UpdateOrderDto updateOrderDto, CancellationToken ct)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId, ct);
        if (order == null)
            throw new Exception($"Order with id {orderId} not found.");

        order.Status = updateOrderDto.Status;
        await _context.SaveChangesAsync(ct);
    }

    public async Task<OrderTotalDto> GetOrderTotal(int id, CancellationToken ct)
    {
        var result = await _context.Orders
            .AsNoTracking()
            .Where(o => o.Id == id)
            .Select(o => new OrderTotalDto
            {
                TotalAmount = o.Items.Sum(i => i.Price * i.Quantity),
                Currency = o.Currency
            })
            .FirstOrDefaultAsync(ct);

        return result ?? new OrderTotalDto();
    }
}
