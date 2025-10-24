using OrdersExercise.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Register AutoMapper profiles
builder.Services.AddAutoMapper(typeof(OrdersExercise.Mapping.AdvancedOrderMappingProfile));
// Add in-memory cache
builder.Services.AddMemoryCache();
// Register a simple in-memory order repository for demo/testing
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
