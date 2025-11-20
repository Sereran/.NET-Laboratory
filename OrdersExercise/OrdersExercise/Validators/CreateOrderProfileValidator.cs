using FluentValidation;
using Microsoft.Extensions.Logging;
using OrdersExercise.Data;
using OrdersExercise.Dtos;
using OrdersExercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrdersExercise.Validators
{
    public class CreateOrderProfileValidator : AbstractValidator<CreateOrderProfileRequest>
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<CreateOrderProfileValidator> _logger;

        private readonly List<string> _technicalKeywords = new() 
        { 
            "Guide", "Manual", "Introduction", "Advanced", "Programming", "C#", "Java", "System", "Architecture" 
        };

        private readonly List<string> _inappropriateChildrenWords = new() 
        { 
            "Kill", "Blood", "Horror", "Death", "Dark", "Violence", "Curse" 
        };

        public CreateOrderProfileValidator(ApplicationContext context, ILogger<CreateOrderProfileValidator> logger)
        {
            _context = context;
            _logger = logger;
            
            When(x => x.Category == OrderCategory.Technical, () =>
            {
                RuleFor(x => x.Price)
                    .GreaterThanOrEqualTo(20)
                    .WithMessage("Technical books require a minimum price of $20.00.");

                RuleFor(x => x.Title)
                    .Must(ContainTechnicalKeywords)
                    .WithMessage($"Technical titles must contain a keyword (e.g., {string.Join(", ", _technicalKeywords.Take(3))}...).");

                RuleFor(x => x.PublishedDate)
                    .GreaterThan(DateTime.UtcNow.AddYears(-5))
                    .WithMessage("Technical content must be recent (published within the last 5 years).");
            });

            When(x => x.Category == OrderCategory.Children, () =>
            {
                RuleFor(x => x.Price)
                    .LessThanOrEqualTo(50)
                    .WithMessage("Children's books cannot cost more than $50.00.");

                RuleFor(x => x.Title)
                    .Must(BeAppropriateForChildren)
                    .WithMessage("Children's book titles cannot contain inappropriate words.");
            });

            When(x => x.Category == OrderCategory.Fiction, () =>
            {
                RuleFor(x => x.Author)
                    .MinimumLength(5)
                    .WithMessage("Fiction authors must display their full name (at least 5 characters).");
            });

            When(x => x.Price > 100, () =>
            {
                RuleFor(x => x.StockQuantity)
                    .LessThanOrEqualTo(20)
                    .WithMessage("High-value items (over $100) cannot have more than 20 items in stock.");
            });
        }

        private bool ContainTechnicalKeywords(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return false;
            
            return _technicalKeywords.Any(keyword => 
                title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private bool BeAppropriateForChildren(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return false;

            bool hasBadWords = _inappropriateChildrenWords.Any(word => 
                title.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0);

            return !hasBadWords;
        }
    }
}