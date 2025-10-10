namespace Laboratory_2.Records;
using System.Collections.Generic;

public record Borrower(int Id, string Name, List<Book> BorrowedBooks)
{
    public override string ToString()
    {
        var booksString = $"[{string.Join(", ", BorrowedBooks)}]";

        return $"Borrower {{ Id = {Id}, Name = {Name}, BorrowedBooks = {booksString} }}";
    }
}