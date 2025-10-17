using Microsoft.EntityFrameworkCore;
using Laboratory_3.Features.Books;

namespace Laboratory_3.Persistence;

public class BookManagementContext (DbContextOptions<BookManagementContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
}