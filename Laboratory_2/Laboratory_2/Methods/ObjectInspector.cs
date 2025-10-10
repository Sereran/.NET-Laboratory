using System;
using Laboratory_2.Records;

public static class ObjectInspector
{
    public static void Inspect(object item)
    {
        switch (item)
        {
            // If the item is a Book
            case Book b:
                Console.WriteLine($"Book: '{b.Title}' published in {b.YearPublished}.");
                break;

            // If the item is a Borrower
            case Borrower br:
                Console.WriteLine($"Borrower: {br.Name} has {br.BorrowedBooks.Count} book(s).");
                break;
            
            // Default
            default:
                Console.WriteLine("Object is of an unknown type.");
                break;
        }
    }
}