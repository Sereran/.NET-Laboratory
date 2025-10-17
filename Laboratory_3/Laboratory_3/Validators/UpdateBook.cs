namespace Laboratory_3.Validators;

using Laboratory_3.Features.Books;
using FluentValidation;

public class UpdateBook : AbstractValidator<UpdateBookRequest>
{
    public UpdateBook()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Title)
            .NotNull().NotEmpty().WithMessage("Title is required.");

        RuleFor(x => x.Author)
            .NotNull().NotEmpty().WithMessage("Author is required.");

        RuleFor(x => x.Year)
            .NotEmpty().WithMessage("Year is required.");
    }
}

