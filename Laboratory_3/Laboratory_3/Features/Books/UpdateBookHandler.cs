namespace Laboratory_3.Features.Books;

using Laboratory_3.Persistence;
using Laboratory_3.Validators;

public class UpdateBookHandler(BookManagementContext context)
{
    private readonly BookManagementContext _context = context;
    
    public async Task<IResult> Handle(Guid id, UpdateBookRequest request)
    {
        if (request.Id != Guid.Empty && request.Id != id)
        {
            return Results.BadRequest("Id in route and body must match.");
        }

        // If body omitted Id, populate it from the route so validators that require Id pass
        if (request.Id == Guid.Empty)
        {
            request = request with { Id = id };
        }

        var validator = new UpdateBook();
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        var existingBook = await _context.Books.FindAsync(id);
        if (existingBook is null)
        {
            return Results.NotFound();
        }

        var updated = new Book(existingBook.Id, request.Title, request.Author, request.Year);
        
        _context.Entry(existingBook).CurrentValues.SetValues(updated);

        await _context.SaveChangesAsync();

        return Results.NoContent();
    }
}