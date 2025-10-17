using Laboratory_3.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Laboratory_3.Features.Books;

public class DeleteBookHandler(BookManagementContext dbContext)
{
    private readonly BookManagementContext _dbContext = dbContext;
    
    public async Task<IResult> Handle(DeleteBookRequest request)
    {
        var book = await _dbContext.Books.FirstOrDefaultAsync(u => u.Id == request.Id);
        if (book == null)
        {
            return Results.NotFound();
        }
        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();
        return Results.NoContent();
    }
}