using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Zvezdichka.Web.Models.AccountViewModels.Validations
{
    [AttributeUsage(AttributeTargets.Property |
                    AttributeTargets.Field)]
    public class ForbiddenCharactersAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string username = value.ToString();
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            return regexItem.IsMatch(username);
        }

        public override string FormatErrorMessage(string name)
        {
            this.ErrorMessage = "The usage of special characters in {0} is forbidden.";
            return String.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name);
        }
    }
}