namespace Laboratory_3.Features.Books;

public record UpdateBookRequest(Guid Id, string Title, string Author, int Year);
