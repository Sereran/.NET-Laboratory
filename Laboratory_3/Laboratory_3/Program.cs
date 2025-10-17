using Laboratory_3.Features.Books;
using Laboratory_3.Persistence;
using Laboratory_3.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<BookManagementContext>(options =>
    options.UseSqlite("Data Source=bookmanagement.db"));
builder.Services.AddScoped<CreateBookHandler>();
builder.Services.AddScoped<GetAllBooksHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBook>();
builder.Services.AddScoped<DeleteBookHandler>();
builder.Services.AddScoped<UpdateBookHandler>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BookManagementContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/books", async (CreateBookRequest req, CreateBookHandler handler) =>
    await handler.Handle(req));
app.MapGet("/books", async (GetAllBooksHandler handler) =>
    await handler.Handle(new GetAllBooksRequest()));
app.MapPut("/books/{id:guid}", async (Guid id, UpdateBookRequest req, UpdateBookHandler handler) =>
    await handler.Handle(id, req));
app.MapDelete("/books/{id:guid}", async (Guid id, DeleteBookHandler handler) =>
{
    await handler.Handle(new DeleteBookRequest(id));
});
    
app.Run();
