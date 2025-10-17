using Laboratory_3.Persistence;
using Laboratory_3.Validators;

namespace Laboratory_3.Features.Books;

public class CreateBookHandler(BookManagementContext context)
{
    private readonly BookManagementContext _context = context;
    
    public async Task<IResult> Handle(CreateBookRequest request)
    {
        var validator = new CreateBook();
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }
        var book = new Book(Guid.NewGuid(), request.Title, request.Author, request.Year);
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        
        return Results.Created($"/books/{book.Id}", book);
    }
}