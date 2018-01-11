using System.ComponentModel.DataAnnotations;
using Zvezdichka.Common;

namespace Zvezdichka.Data.Models.ValidationAttributes
{
    public class StockQuantityAttribute : ValidationAttribute
    {
        private const byte minValue = 0;
        private const byte maxValue = byte.MaxValue;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            byte quantity = (byte)validationContext.ObjectInstance;

            if (quantity >= minValue && quantity <= maxValue)
            {
                return new ValidationResult(CommonConstants.WrongStockAmount);
            }

            return ValidationResult.Success;
        }
    }
}
