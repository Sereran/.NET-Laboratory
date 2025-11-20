using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersExercise.Data;
using OrdersExercise.Dtos;
using OrdersExercise.Handlers;
using OrdersExercise.Mapping;
using OrdersExercise.Middleware;
using OrdersExercise.Models;
using OrdersExercise.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options => 
    options.UseInMemoryDatabase("OrdersDb"));

builder.Services.AddAutoMapper(typeof(AdvancedOrderMappingProfile).Assembly); 

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderProfileValidator>();

builder.Services.AddScoped<CreateOrderHandler>();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();


var app = builder.Build();

app.UseCorrelationIdMiddleware();

app.UseHttpsRedirection();

app.MapPost("/orders", async (
    [FromBody] CreateOrderProfileRequest request,
    [FromServices] IValidator<CreateOrderProfileRequest> validator,
    [FromServices] CreateOrderHandler handler) =>
{
    var validationResult = await validator.ValidateAsync(request);
    
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    var result = await handler.Handle(request);

    return Results.Created($"/orders/{result.Id}", result);
})
.WithName("CreateOrder")
.WithSummary("Create a new Order Profile")
.WithDescription("Validates input against business rules and creates a new order entry.")
.Produces<OrderProfileDto>(StatusCodes.Status201Created)
.ProducesValidationProblem();

app.Run();