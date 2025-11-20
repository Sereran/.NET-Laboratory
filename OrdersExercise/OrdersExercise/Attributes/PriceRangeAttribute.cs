using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace OrdersExercise.Attributes
{
    public class PriceRangeAttribute : ValidationAttribute
    {
        private readonly decimal _minPrice;
        private readonly decimal _maxPrice;

        public PriceRangeAttribute(double min, double max)
        {
            _minPrice = (decimal)min;
            _maxPrice = (decimal)max;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (decimal.TryParse(value.ToString(), out decimal price))
            {
                if (price >= _minPrice && price <= _maxPrice)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture,
                "The field {0} must be between {1:C} and {2:C}.",
                name, _minPrice, _maxPrice);
        }
    }
}