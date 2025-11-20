using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OrdersExercise.Attributes
{
    public class ValidISBNAttribute : ValidationAttribute, IClientModelValidator
    {
        public ValidISBNAttribute()
        {
            // Default error message if none is provided
            if (ErrorMessage == null)
            {
                ErrorMessage = "The ISBN must be a valid 10 or 13 digit number.";
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            string isbn = value.ToString();

            string cleanIsbn = isbn.Replace("-", "").Replace(" ", "");

            // Check if all characters are digits and length is correct
            bool isNumeric = cleanIsbn.All(char.IsDigit);
            if (!isNumeric || (cleanIsbn.Length != 10 && cleanIsbn.Length != 13))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-isbn", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
        }

        private void MergeAttribute(System.Collections.Generic.IDictionary<string, string> attributes, string key, string value)
        {
            if (!attributes.ContainsKey(key))
            {
                attributes.Add(key, value);
            }
        }
    }
}