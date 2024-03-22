using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Api.Enums;
using Order.Api.Models.Contexts;
using Order.Api.ViewModels;
using Shared.Events;
using Shared.Messages;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMq"]);
    });
});

builder.Services.AddDbContext<OrderApiDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/create-order", async (CreateOrderVm model, OrderApiDbContext _context, IPublishEndpoint _publishEndPoint) =>
{
    Order.Api.Models.Order order = new()
    {
        //D��ar�dan gelecek olan veri Guid tipinde oldu�u i�in Guid.TryParse ile kontrol ediyoruz.
        BuyerId = Guid.TryParse(model.BuyerId, out Guid _buyerId) ? _buyerId : Guid.NewGuid(),

        OrderItems = model.OrderItems.Select(oi => new Order.Api.Models.OrderItem()
        {
            Count = oi.Count,
            Price = oi.Price,
            ProductId = Guid.TryParse(oi.ProductId, out Guid _productId) ? _productId : Guid.NewGuid()
        }).ToList(),
        OrderStatus=OrderStatus.Suspend,
        CreatedDate=DateTime.UtcNow,
        TotalPice=model.OrderItems.Sum(oi=>oi.Price*oi.Count)
    };

    await _context.Orders.AddAsync(order);
    await _context.SaveChangesAsync();

    OrderCreatedEvent orderCreatedEvent = new()
    {
        BuyerId = order.BuyerId,
        OrderId = order.Id,
        TotalPrice = order.TotalPice,

        OrderItems = order.OrderItems.Select(oi => new OrderItemMessage()
        {
            Count=oi.Count,
            Price=oi.Price,
            ProductId=oi.ProductId
        }).ToList()
    };

   await _publishEndPoint.Publish(order);
});

app.Run();
