using FluentValidation;
using MassTransit.Demo.Communication.Extensions;
using MassTransit.Demo.Customers.Saga;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host
    .AddConfiguration()
    .AddSerilog();

builder
    .Services
    .AddMassTransitMiddleware((busRegConfig, config) =>
    {
        // MassTransit PubSub
        busRegConfig.ConfigureBus(config);

        // MassTransit Saga
        busRegConfig.ConfigureSaga<CustomerStateMachine, Customer>(config);
    })
    .AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program
{ }