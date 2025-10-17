namespace Laboratory_3.Validators;
using Laboratory_3.Features.Books;
using FluentValidation;

public class CreateBook : AbstractValidator<CreateBookRequest>
{
    public CreateBook()
    {
        RuleFor(x => x.Title)
            .NotNull().NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Author)
            .NotNull().NotEmpty().WithMessage("Author is required.");
        RuleFor(x=> x.Year)
            .NotNull().NotEmpty().WithMessage("Year is required.");
    }
    
}