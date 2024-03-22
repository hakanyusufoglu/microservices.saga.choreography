using MassTransit;
using Stock.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMq"]);
    });
});

builder.Services.AddSingleton<MongoDbService>();

var app = builder.Build();

app.Run();
