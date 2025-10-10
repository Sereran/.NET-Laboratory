
using System;
using System.Collections.Generic;
using Laboratory_2.Methods;
using Laboratory_2.Records;

WithClone.RunDemo();

var book = new Book("Foundation", "Isaac Asimov", 1951);
var borrower = new Borrower(201, "Maria Popescu", new List<Book> { book });
var librarian = new Librarian { Name = "Mr. Jones", Email = "jones@lib.com", LibrarySection = "Non-Fiction"};

Console.WriteLine("--- Inspecting Objects ---");
ObjectInspector.Inspect(book);
ObjectInspector.Inspect(borrower);
ObjectInspector.Inspect(librarian);

List<Book> books = new();

Console.WriteLine("Enter book details. Type 'done' when you are finished.");

while (true)
{
    Console.Write("Enter Title (or 'done'): ");
    string title = Console.ReadLine();

    if (title.ToLower() == "done")
    {
        break;
    }

    Console.Write("Enter Author: ");
    string author = Console.ReadLine();

    Console.Write("Enter Year Published: ");
    int year = int.Parse(Console.ReadLine());
    
    books.Add(new Book(title, author, year));
    Console.WriteLine("Book added!");
    Console.WriteLine();
}

Console.WriteLine("\n--- Here is your book collection ---");
if (books.Count == 0)
{
    Console.WriteLine("No books were entered.");
}
else
{
    foreach (var carte in books)
    {
        Console.WriteLine(book);
    }
}