using AbySalto.Junior.DTO;
using AbySalto.Junior.Infrastructure.Database;
using AbySalto.Junior.Models;
using AbySalto.Junior.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AbySalto.Junior.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;

namespace AbySalto.Junior.Services;

public class OrderService : IOrderService
{
    private readonly IApplicationDbContext _context;

    public OrderService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Create(CreateOrderDto order, CancellationToken ct)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        var entity = new Order
        {
            BuyerName = order.BuyerName,
            PaymentType = order.PaymentType,
            DeliveryAddress = order.DeliveryAddress,
            ContactNumber = order.ContactNumber,
            Remark = order.Remark,
            OrderDateTime = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            Items = order.Items
                .Select(i => new OrderItem
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    Price = i.Price
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
                Items = o.Items
                    .Select(i => new OrderItemDto
                    {
                        ItemId = i.ItemId,
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

    public async Task Update(int orderId, UpdateOrderDto updateOrderDto, CancellationToken ct)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId, ct);
        if (order == null)
            throw new Exception($"Order with id {orderId} not found.");

        order.Status = updateOrderDto.Status;
        await _context.SaveChangesAsync(ct);
    }

    public async Task<OrderTotalDto> GetOrderTotal(int id, CancellationToken ct)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, ct);

        return order?.TotalAmount ?? 0;
    }
}
