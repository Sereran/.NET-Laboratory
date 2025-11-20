using OrdersExercise.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OrdersExercise.Attributes
{
    public class OrderCategoryAttribute : ValidationAttribute
    {
        private readonly OrderCategory[] _allowedCategories;
        
        public OrderCategoryAttribute(params OrderCategory[] allowedCategories)
        {
            _allowedCategories = allowedCategories;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is OrderCategory category)
            {
                if (_allowedCategories.Contains(category))
                {
                    return ValidationResult.Success;
                }
            }

            string allowedString = string.Join(", ", _allowedCategories);
            return new ValidationResult($"Invalid category. Allowed categories are: {allowedString}.");
        }
    }
}