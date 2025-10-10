namespace Laboratory_2.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using Laboratory_2.Records;

public static class WithClone
{
    public static void RunDemo()
    {
        Console.WriteLine("--- Running the Borrower Cloning Demonstration ---");

        var book1 = new Book("The Hobbit", "J.R.R. Tolkien", 1937);
        var book2 = new Book("Dune", "Frank Herbert", 1965);
        var originalBorrower = new Borrower(101, "Jane Doe", new List<Book> { book1 });

        var updatedBorrower = originalBorrower with
        {
            BorrowedBooks = originalBorrower.BorrowedBooks.Append(book2).ToList()
        };

        Console.WriteLine("Original Borrower: " + originalBorrower);
        Console.WriteLine("Updated Borrower: " + updatedBorrower);
        Console.WriteLine("---------------------------------------------\n");
    }
}