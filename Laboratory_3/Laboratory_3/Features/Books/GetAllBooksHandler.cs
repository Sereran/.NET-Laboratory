using Laboratory_3.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Laboratory_3.Features.Books;

public class GetAllBooksHandler(BookManagementContext context)
{
    private readonly BookManagementContext _context = context;
    
    public async Task<IResult> Handle(GetAllBooksRequest request)
    {
        var books =  await _context.Books.ToListAsync();
        return Results.Ok(books);
    }
}